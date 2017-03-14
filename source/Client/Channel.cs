using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TeamSpeak.Sdk.Client
{
    /// <summary>
    /// Represents a Channel on a TeamSpeak-Server
    /// </summary>
    public class Channel: IEquatable<Channel>
    {
        /// <summary>
        /// ID of the channel
        /// </summary>
        public ulong ID { get; }
        /// <summary>
        /// Server Connection
        /// </summary>
        public Connection Connection { get; }
        /// <summary>
        /// The parent channel
        /// </summary>
        public Channel Parent { get { return Library.Api.GetParentChannelOfChannel(this); } }
        /// <summary>
        /// List of all clients in the channel, if the channel is currently subscribed.
        /// </summary>
        public ICollection<Client> Clients { get { return Library.Api.GetChannelClientList(this)?.AsReadOnly(); } }
        /// <summary>
        /// List of all subchannels in the channel
        /// </summary>
        public ReadonlyChannelCollection Channels { get { return Connection.ChannelTracker.GetChannels(ID); } }

        // cached name and topic, so that they can survive the channels death
        private string CachedName;
        private string CachedTopic;

        /// <summary>
        /// Creates a new Channel-Object.
        /// </summary>
        /// <remarks>does not create a new channel on the server. To create a new channel use <see cref="Connection.CreateChannel(string, Channel, string, string, string, Channel, bool?, bool?, CodecType?, int?, bool?, int?)"/></remarks>
        /// <param name="connection">the server connection</param>
        /// <param name="id">ID of the channel</param>
        public Channel(Connection connection, ulong id)
            : this(connection, id, waitForProperties: false)
        {
        }

        internal Channel(Connection connection, ulong id, bool waitForProperties)
        {
            Require.NotNull(nameof(connection), connection);
            ID = id;
            Connection = connection;
            RefreshProperties(waitForProperties);
        }

        /// <summary>
        /// Name of the channel
        /// </summary>
        public string Name
        {
            get { return CachedName; }
            set
            {
                Require.NotNull(nameof(value), value);
                SetString(ChannelProperty.Name, value);
            }
        }

        /// <summary>
        /// Single-line channel topic
        /// </summary>
        public string Topic
        {
            get { return CachedTopic; }
            set
            {
                Require.NotNull(nameof(value), value);
                SetString(ChannelProperty.Topic, value);
            }
        }

        /// <summary>
        /// Optional channel description. Can have multiple lines. Needs to be request with <see cref="GetChannelDescription"/>.
        /// </summary>
        public string Description
        {
            get { return GetString(ChannelProperty.Description); }
            set
            {
                Require.NotNull(nameof(value), value);
                SetString(ChannelProperty.Description, value);
            }
        }

        /// <summary>
        /// Optional password for password-protected channels.
        /// </summary>
        public string Password
        {
            set
            {
                Require.NotNull(nameof(value), value);
                SetString(ChannelProperty.Password, value);
            }
        }

        /// <summary>
        /// Codec used for this channel
        /// </summary>
        public CodecType Codec
        {
            get { return (CodecType)GetInt(ChannelProperty.Codec); }
            set { SetInt(ChannelProperty.Codec, (int)value); }
        }

        /// <summary>
        /// Quality of channel codec of this channel. Valid values range from 0 to 10, default is 7. Higher values result in better speech quality but more bandwidth usage
        /// </summary>
        public int CodecQuality
        {
            get { return GetInt(ChannelProperty.CodecQuality); }
            set { SetInt(ChannelProperty.CodecQuality, value); }
        }

        /// <summary>
        /// Number of maximum clients who can join this channel
        /// </summary>
        public int MaxClients
        {
            get { return GetInt(ChannelProperty.Maxclients); }
            set { SetInt(ChannelProperty.Maxclients, value); }
        }

        /// <summary>
        /// Number of maximum clients who can join this channel and all subchannels
        /// </summary>
        public int MaxFamilyClients
        {
            get { return GetInt(ChannelProperty.Maxfamilyclients); }
            set { SetInt(ChannelProperty.Maxfamilyclients, value); }
        }

        /// <summary>
        /// <see cref="Order"/> is the <see cref="Channel"/> after which this channel is sorted. <see langword="null"/> meaning its going to be the first <see cref="Channel"/> under <see cref="Parent"/>
        /// </summary>
        public Channel Order
        {
            get { return Connection.Cache.GetChannel(GetUInt64(ChannelProperty.Order)); }
            set
            {
                Require.SameConnection(nameof(value), Connection, value);
                SetUInt64(ChannelProperty.Order, value?.ID ?? 0);
            }
        }

        /// <summary>
        /// Permanent channels will be restored when the server restarts.
        /// </summary>
        public bool IsPermanent
        {
            get { return GetInt(ChannelProperty.FlagPermanent) != 0; }
            set { SetInt(ChannelProperty.FlagPermanent, value ? 1 : 0); }
        }

        /// <summary>
        /// Semi-permanent channels are not automatically deleted when the last user left but will not be restored when the server restarts.
        /// </summary>
        public bool IsSemiPermanent
        {
            get { return GetInt(ChannelProperty.FlagSemiPermanent) != 0; }
            set { SetInt(ChannelProperty.FlagSemiPermanent, value ? 1 : 0); }
        }

        /// <summary>
        /// Channel is the default channel. There can only be one default channel per server. New users who did not configure a channel to join on login in ts3client_startConnection will automatically join the default channel.
        /// </summary>
        public bool IsDefault
        {
            get { return GetInt(ChannelProperty.FlagDefault) != 0; }
            set { SetInt(ChannelProperty.FlagDefault, value ? 1 : 0); }
        }

        /// <summary>
        /// If set, channel is password protected. The password itself is stored in ChannelPassword
        /// </summary>
        public bool IsPasswordProtected
        {
            get { return GetInt(ChannelProperty.FlagPassword) != 0; }
        }

        /// <summary>
        /// Latency of this channel.
        /// </summary>
        /// <remarks>
        /// Allows to increase the packet size resulting in less bandwidth usage at the cost of higher latency. A value of 1 (default) is the best setting for lowest latency and best quality. If bandwidth or network quality are restricted, increasing the latency factor can help stabilize the connection. Higher latency values are only possible for low-quality codec and codec quality settings.
        /// </remarks>
        public int CodecLatencyFactor
        {
            get { return GetInt(ChannelProperty.CodecLatencyFactor); }
            set { SetInt(ChannelProperty.CodecLatencyFactor, value); }
        }

        /// <summary>
        /// If true, this channel is not using encrypted voice data. If false, voice data is encrypted for this channel.
        /// </summary>
        /// <remarks>
        /// Note that channel voice data encryption can be globally disabled or enabled for the virtual server. Changing this flag makes only sense if global voice data encryption is set to be configured per channel.
        /// </remarks>
        public bool CodecIsUnencrypted
        {
            get { return GetInt(ChannelProperty.CodecIsUnencrypted) != 0; }
            set { SetInt(ChannelProperty.CodecIsUnencrypted, value ? 1 : 0); }
        }

        /// <summary>
        /// This parameter defines how many seconds the server waits until a temporary channel is deleted when empty.
        /// When a temporary channel is created, a timer is started. If a user joins the channel before the countdown is finished, the channel is not deleted. After the last person has left the channel, the countdown starts again. DeleteDelay defines the length of this countdown in seconds.
        /// The time since the last client has left the temporary channel can be queried with ts3client_getChannelEmptySecs.
        /// </summary>
        public TimeSpan DeleteDelay
        {
            get { return TimeSpan.FromSeconds(GetInt(ChannelProperty.DeleteDelay)); }
            set
            {
                double seconds = value.TotalSeconds;
                try
                {
                    SetInt(ChannelProperty.DeleteDelay, Convert.ToInt32(seconds));
                }
                catch (OverflowException)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"TotalSeconds must fit into a Int32");
                }
            }
        }

        /// <summary>
        /// Time since the last client has left a temporary channel
        /// </summary>
        public TimeSpan ChannelEmptyTime
        {
            get
            {
                int seconds;
                Error error = Library.Api.TryGetChannelEmptySecs(this, out seconds);
                switch (error)
                {
                    case Error.Ok: return TimeSpan.FromSeconds(seconds);
                    case Error.ChannelNotEmpty: return TimeSpan.Zero;
                    default: throw Library.CreateException(error);
                }
            }
        }

        /// <summary>
        /// Uploads a local file to the server
        /// </summary>
        /// <param name="file">Path of the local file, which is to be uploaded.</param>
        /// <param name="overwrite">when false, upload will abort if remote file exists</param>
        /// <param name="resume">If we have a previously halted transfer: true = resume, false = restart transfer</param>
        /// <param name="channelPassword">Optional channel password. Pass empty string or null if unused.</param>
        /// <returns>A task that represents the asynchronous upload operation.</returns>
        public Task SendFile(string file, bool overwrite = false, bool resume = false, string channelPassword = null)
        {
            Require.NotNull(nameof(file), file);
            return SendFile(Path.GetDirectoryName(file), "/" + Path.GetFileName(file), overwrite, resume, channelPassword);
        }

        /// <summary>
        /// Uploads a local file to the server
        /// </summary>
        /// <param name="sourceDirectory">Local directory where the file to upload is located.</param>
        /// <param name="fileName">Filename of the local file, which is to be uploaded.</param>
        /// <param name="overwrite">when false, upload will abort if remote file exists</param>
        /// <param name="resume">If we have a previously halted transfer: true = resume, false = restart transfer</param>
        /// <param name="channelPassword">Optional channel password. Pass empty string or null if unused.</param>
        /// <returns>A task that represents the asynchronous upload operation.</returns>
        public Task SendFile(string sourceDirectory, string fileName, bool overwrite = false, bool resume = false, string channelPassword = null)
        {
            return SendFile(sourceDirectory, fileName, overwrite, resume, channelPassword, CancellationToken.None);
        }

        /// <summary>
        /// Uploads a local file to the server
        /// </summary>
        /// <param name="sourceDirectory">Local directory where the file to upload is located.</param>
        /// <param name="fileName">Filename of the local file, which is to be uploaded.</param>
        /// <param name="overwrite">when false, upload will abort if remote file exists</param>
        /// <param name="resume">If we have a previously halted transfer: true = resume, false = restart transfer</param>
        /// <param name="channelPassword">Optional channel password. Pass empty string or null if unused.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/></param>
        /// <returns>A task that represents the asynchronous upload operation.</returns>
        public Task SendFile(string sourceDirectory, string fileName, bool overwrite, bool resume, string channelPassword, CancellationToken cancellationToken)
        {
            Require.NotNull(nameof(sourceDirectory), sourceDirectory);
            Require.NotNull(nameof(fileName), fileName);
            return SimplifiedTransferRequest(Library.Api.SendFile, fileName, sourceDirectory, channelPassword, overwrite, resume, cancellationToken);
        }

        /// <summary>
        /// Request uploading a local file to the server
        /// </summary>
        /// <param name="sourceDirectory">Local directory where the file to upload is located.</param>
        /// <param name="fileName">Filename of the local file, which is to be uploaded.</param>
        /// <param name="overwrite">when false, upload will abort if remote file exists</param>
        /// <param name="resume">If we have a previously halted transfer: true = resume, false = restart transfer</param>
        /// <param name="channelPassword">Optional channel password. Pass empty string or null if unused.</param>
        /// <param name="returnCode">a custom string. The <see cref="Connection.ServerError"/>-Event will receive the same custom string in its returnCode parameter. If no error occurred, <see cref="Connection.ServerError"/> will indicate success by passing <see cref="Error.Ok"/>.</param>
        /// <returns>A FileTransfer that represents the asynchronous upload operation.</returns>
        public FileTransfer SendFile(string sourceDirectory, string fileName, bool overwrite, bool resume, string channelPassword, string returnCode)
        {
            Require.NotNull(nameof(sourceDirectory), sourceDirectory);
            Require.NotNull(nameof(fileName), fileName);
            return Library.Api.SendFile(this, channelPassword, fileName, overwrite, resume, sourceDirectory, returnCode);
        }

        /// <summary>
        /// Download a file from the server.
        /// </summary>
        /// <param name="fileName">Filename of the remote file, which is to be downloaded.</param>
        /// <param name="destinationDirectory">Local target directory name where the download file should be saved.</param>
        /// <param name="overwrite">when false, download will abort if local file exists</param>
        /// <param name="resume">If we have a previously halted transfer: true = resume, false = restart transfer</param>
        /// <param name="channelPassword">Optional channel password. Pass empty string or null if unused.</param>
        /// <returns>A task that represents the asynchronous download operation.</returns>
        public Task RequestFile(string fileName, string destinationDirectory, bool overwrite = false, bool resume = false, string channelPassword = null)
        {
            return RequestFile(fileName, destinationDirectory, overwrite, resume, channelPassword, CancellationToken.None);
        }

        /// <summary>
        /// Download a file from the server.
        /// </summary>
        /// <param name="fileName">Filename of the remote file, which is to be downloaded.</param>
        /// <param name="destinationDirectory">Local target directory name where the download file should be saved.</param>
        /// <param name="overwrite">when false, download will abort if local file exists</param>
        /// <param name="resume">If we have a previously halted transfer: true = resume, false = restart transfer</param>
        /// <param name="channelPassword">Optional channel password. Pass empty string or null if unused.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/></param>
        /// <returns>A task that represents the asynchronous download operation.</returns>
        public Task RequestFile(string fileName, string destinationDirectory, bool overwrite, bool resume, string channelPassword, CancellationToken cancellationToken)
        {
            Require.NotNull(nameof(fileName), fileName);
            Require.NotNull(nameof(destinationDirectory), destinationDirectory);
            return SimplifiedTransferRequest(Library.Api.RequestFile, fileName, destinationDirectory, channelPassword, overwrite, resume, cancellationToken);
        }
        
        /// <summary>
        /// Request downloading a file from the server.
        /// </summary>
        /// <param name="fileName">Filename of the remote file, which is to be downloaded.</param>
        /// <param name="destinationDirectory">Local target directory name where the download file should be saved.</param>
        /// <param name="overwrite">when false, download will abort if local file exists</param>
        /// <param name="resume">If we have a previously halted transfer: true = resume, false = restart transfer</param>
        /// <param name="channelPassword">Optional channel password. Pass empty string or null if unused.</param>
        /// <param name="returnCode">a custom string. The <see cref="Connection.ServerError"/>-Event will receive the same custom string in its returnCode parameter. If no error occurred, <see cref="Connection.ServerError"/> will indicate success by passing <see cref="Error.Ok"/>.</param>
        /// <returns>A FileTransfer that represents the asynchronous download operation.</returns>
        public FileTransfer RequestFile(string fileName, string destinationDirectory, bool overwrite, bool resume, string channelPassword, string returnCode)
        {
            Require.NotNull(nameof(fileName), fileName);
            Require.NotNull(nameof(destinationDirectory), destinationDirectory);
            return Library.Api.RequestFile(this, channelPassword, fileName, overwrite, resume, destinationDirectory, returnCode);
        }

        /// <summary>
        /// Implementation for both upload and download. Since both APIs need a thick wrapper, and are nearly identically => the code was merged.
        /// </summary>
        private delegate FileTransfer TransferMethod(Channel channel, string channelPW, string file, bool overwrite, bool resume, string directory, string returnCode);
        private Task SimplifiedTransferRequest(TransferMethod method, string fileName, string directory, string channelPassword, bool overwrite, bool resume, CancellationToken cancellationToken)
        {
            TaskCompletionSource<Error> taskCompletionSource = new TaskCompletionSource<Error>();
            // check if task is already canceled.
            if (cancellationToken.IsCancellationRequested)
            {
                // if yes, exit early.
                taskCompletionSource.SetCanceled();
                return taskCompletionSource.Task;
            }

            // the return code can show up in FileTransferStatusReceived, and ServerError.
            // and both can signal the end of the operation.
            // that is why ServerError is mapped on to the handler for FileTransferStatusReceived
            // this way, the clean up code has to be written once.
            // CancellationToken does not directly signal the abort, but instead calls TryHaltTransfer.
            // which will result in a FileTransferStatusReceived.

            FileTransfer self = null;
            Task startTask;
            string returnCode = Connection.GetNextReturnCode(out startTask);
            // = null, to make the c# compiler happy.
            FileTransferStatusEventHandler eventHandler = null;
            eventHandler = (transfer, status) =>
            {
                if (transfer != self) return;
                // any event for transfer signals the end of the operation
                Connection.FileTransferStatusReceived -= eventHandler;
                // translate the error code into task state
                switch (status)
                {
                    case Error.FileTransferComplete: taskCompletionSource.SetResult(Error.Ok); break;
                    case Error.FileTransferCanceled: taskCompletionSource.SetCanceled(); break;
                    default: taskCompletionSource.SetException(Library.CreateException(status)); break;
                }
            };
            startTask.ContinueWith(antecendent =>
            {
                // failed to initiate a transfer
                Connection.FileTransferStatusReceived -= eventHandler;
                // pass exception to user
                taskCompletionSource.SetException(antecendent.Exception);
            }, cancellationToken, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Current);
            Connection.FileTransferStatusReceived += eventHandler;

            self = method(this, channelPassword, fileName, overwrite, resume, directory, returnCode);

            if (cancellationToken != CancellationToken.None)
            {
                // register cancellationToken
                cancellationToken.Register(() => Library.Api.TryHaltTransfer(self, true, null));
            }

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Query list of files in a directory.
        /// </summary>
        /// <param name="path">Path inside the channel, defining the subdirectory. Top level path is “/”</param>
        /// <param name="channelPassword">Optional channel password. Pass empty string or null if unused.</param>
        /// <returns>A task that returns the list of files contained in path</returns>
        public Task<ICollection<FileInfo>> GetFileList(string path = "/", string channelPassword = null)
        {
            Require.NotNull(nameof(path), path);
            Task task;
            string returnCode = Connection.GetNextReturnCode(out task);
            Connection.Cache.FileListBuilder.Register(returnCode);
            Library.Api.RequestFileList(this, channelPassword, path, returnCode);
            return Connection.CollectList(task, Connection.Cache.FileListBuilder, returnCode);
        }

        /// <summary>
        /// Query information of a specified file. The answer from the server will trigger <see cref="Connection.FileInfoReceived"/> with the requested information.
        /// </summary>
        /// <param name="file">File name we want to request info from, needs to include the full path within the channel, e.g. “/file” for a top-level file or “/dir1/dir2/file” for a file located in a subdirectory.</param>
        /// <param name="channelPassword">Optional channel password. Pass empty string or null if unused.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task GetFileInfo(string file, string channelPassword = null)
        {
            Require.NotNull(nameof(file), file);
            Task task;
            Library.Api.RequestFileInfo(this, channelPassword, file, Connection.GetNextReturnCode(out task));
            return task;
        }

        /// <summary>
        /// Create a directory.
        /// </summary>
        /// <param name="directoryPath">Name of the directory to create. The directory name needs to include the full path within the channel, e.g. “/file” for a top-level file or “/dir1/dir2/file” for a file located in a subdirectory.</param>
        /// <param name="channelPassword">Optional channel password. Pass empty string or null if unused.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task CreateDirectory(string directoryPath, string channelPassword = null)
        {
            Require.NotNull(nameof(directoryPath), directoryPath);
            Task task;
            Library.Api.RequestCreateDirectory(this, channelPassword, directoryPath, Connection.GetNextReturnCode(out task));
            return task;
        }

        /// <summary>
        /// Moves or renames a file. If the source and target paths are the same, the file will simply be renamed.
        /// </summary>
        /// <param name="file">Old name of the file. The file name needs to include the full path within the channel, e.g. “/file” for a top-level file or “/dir1/dir2/file” for a file located in a subdirectory.</param>
        /// <param name="toFile">New name of the file. The new name needs to include the full path within the channel, e.g. “/file” for a top-level file or “/dir1/dir2/file” for a file located in a subdirectory.</param>
        /// <param name="channelPassword">Optional channel password. Pass empty string or null if unused.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task MoveFile(string file, string toFile, string channelPassword = null)
        {
            return MoveFile(file, this, toFile, channelPassword, channelPassword);
        }

        /// <summary>
        /// Moves or renames a file. If the source and target channels and paths are the same, the file will simply be renamed.
        /// </summary>
        /// <param name="file">Old name of the file. The file name needs to include the full path within the channel, e.g. “/file” for a top-level file or “/dir1/dir2/file” for a file located in a subdirectory.</param>
        /// <param name="channelPassword">Optional channel password. Pass empty string or null if unused.</param>
        /// <param name="toFile">New name of the file. The new name needs to include the full path within the channel, e.g. “/file” for a top-level file or “/dir1/dir2/file” for a file located in a subdirectory.</param>
        /// <param name="toChannel">Target channel, to which we want to move the file.</param>
        /// <param name="toChannelPassword">Optional channel password. Pass empty string or null if unused.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task MoveFile(string file, Channel toChannel, string toFile, string channelPassword = null, string toChannelPassword = null)
        {
            Require.NotNull(nameof(file), file);
            Require.NotNull(nameof(toFile), toFile);
            Require.NotNull(nameof(toChannel), toChannel);
            Require.SameConnection(nameof(toChannel), Connection, toChannel);
            Task task;
            Library.Api.RequestRenameFile(this, channelPassword, toChannel, toChannelPassword, file, toFile, Connection.GetNextReturnCode(out task));
            return task;
        }

        /// <summary>
        /// Deleting a remote files on the server
        /// </summary>
        /// <param name="file">File we request to be deleted. The file name needs to include the full path within the channel, e.g. “/file” for a top-level file or “/dir1/dir2/file” for a file located in a subdirectory.</param>
        /// <param name="channelPassword">Optional channel password. Pass empty string or null if unused.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task DeleteFile(string file, string channelPassword = null)
        {
            Require.NotNull(nameof(file), file);
            return DeleteFile(new string[1] { file }, channelPassword);
        }

        /// <summary>
        /// Delete one or more remote files on the server.
        /// </summary>
        /// <param name="files">List of files we request to be deleted. The file names need to include the full path within the channel, e.g. “/file” for a top-level file or “/dir1/dir2/file” for a file located in a subdirectory.</param>
        /// <param name="channelPassword">Optional channel password. Pass empty string or null if unused.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task DeleteFile(string[] files, string channelPassword = null)
        {
            Require.NotNullOrEmpty(nameof(files), files);
            Task task;
            Library.Api.RequestDeleteFile(this, channelPassword, files, Connection.GetNextReturnCode(out task));
            return task;
        }
        /// <summary>
        /// Request updating the channel description 
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task GetChannelDescription()
        {
            Task task;
            Library.Api.RequestChannelDescription(this, Connection.GetNextReturnCode(out task));
            return task;
        }

        /// <summary>
        /// Removes the channel from the server
        /// </summary>
        /// <param name="force">If true, the channel will be deleted even when it is not empty. Clients within the deleted channel are transfered to the default channel. Any contained subchannels are removed as well. If false, the server will refuse to delete a channel that is not empty.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task Delete(bool force = false)
        {
            Task task;
            Library.Api.RequestChannelDelete(this, force, Connection.GetNextReturnCode(out task));
            return task;
        }

        /// <summary>
        /// Send a text message to the channel
        /// </summary>
        /// <param name="message">The text message</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task SendTextMessage(string message)
        {
            Require.NotNull(nameof(message), message);
            Task task;
            Library.Api.RequestSendChannelTextMsg(this, message, Connection.GetNextReturnCode(out task));
            return task;
        }

        /// <summary>
        /// Move the channel to a new parent channel
        /// </summary>
        /// <param name="newParent">The parent channel where the moved channel is to be inserted as child. Use null to insert as top-level channel.</param>
        /// <param name="newChannelOrder">the <see cref="Channel"/> after which <see langword="this"/> <see cref="Channel"/> is sorted. <see langword="null"/> meaning its going to be the first <see cref="Channel"/> under <paramref name="newParent"/>.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task MoveTo(Channel newParent, Channel newChannelOrder = null)
        {
            Require.NotNull(nameof(newParent), newParent);
            Require.SameConnection(nameof(newParent), Connection, newParent);
            Require.SameConnection(nameof(newChannelOrder), Connection, newChannelOrder);
            Task task;
            Library.Api.RequestChannelMove(this, newParent, newChannelOrder, Connection.GetNextReturnCode(out task));
            return task;
        }

        /// <summary>
        /// Compares two <see cref="Channel"/> for equality. 
        /// </summary>
        /// <param name="a">The first <see cref="Channel"/> structure to compare.</param>
        /// <param name="b">The second <see cref="Channel"/> structure to compare.</param>
        /// <returns>true if the ID and Mode of a and b are equal; otherwise, false.</returns>
        public static bool operator ==(Channel a, Channel b)
        {
            if (object.ReferenceEquals(a, b)) return true;
            if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null)) return false;
            return a.Connection == b.Connection && a.ID == b.ID;
        }

        /// <summary>
        /// Compares two <see cref="Channel"/> for inequality. 
        /// </summary>
        /// <param name="a">The first <see cref="Channel"/> structure to compare.</param>
        /// <param name="b">The second <see cref="Channel"/> structure to compare.</param>
        /// <returns>true if the ID and Mode of a and b are different; otherwise, false.</returns>
        public static bool operator !=(Channel a, Channel b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            string result = Name;
            if (string.IsNullOrEmpty(result) == false)
                return result;
            else return base.ToString();
        }

        /// <summary>
        /// Indicates whether this instance and a another instance  are equal.
        /// </summary>
        /// <param name="other">Another instance to compare to.</param>
        /// <returns>true if this and  the other instance represent the same value; otherwise, false.</returns>
        public bool Equals(Channel other)
        {
            return this == other;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>true if obj and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return obj is Channel && this == (Channel)obj;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return Connection.ID.GetHashCode() * 7 + ID.GetHashCode();
        }

        private int GetInt(ChannelProperty flag)
        {
            return Library.Api.GetChannelVariableAsInt(this, flag);
        }
        private ulong GetUInt64(ChannelProperty flag)
        {
            return Library.Api.GetChannelVariableAsUInt64(this, flag);
        }
        private string GetString(ChannelProperty flag)
        {
            return Library.Api.GetChannelVariableAsString(this, flag);
        }
        private void SetInt(ChannelProperty flag, int value)
        {
            Library.Api.SetChannelVariableAsInt(this, flag, value);
            if (ID != 0) FlushChannelUpdates();
        }
        private void SetUInt64(ChannelProperty flag, ulong value)
        {
            Library.Api.SetChannelVariableAsUInt64(this, flag, value);
            if (ID != 0) FlushChannelUpdates();
        }
        private void SetString(ChannelProperty flag, string value)
        {
            Library.Api.SetChannelVariableAsString(this, flag, value);
            if (ID != 0) FlushChannelUpdates();
        }
        private void FlushChannelUpdates()
        {
            Task task;
            Library.Api.FlushChannelUpdates(this, Connection.GetNextReturnCode(out task));
            try
            {
                task.Wait();
            }
            catch (AggregateException e) { throw e.InnerException; }
        }
        internal void RefreshProperties(bool wait)
        {
            if (ID == 0) return;
            if (Library.Api.TryGetChannelVariableAsString(this, ChannelProperty.Name, out CachedName) != Error.Ok && wait)
            {
                System.Diagnostics.Stopwatch timer = System.Diagnostics.Stopwatch.StartNew();
                Error error;
                do Thread.Yield();
                while ((error = Library.Api.TryGetChannelVariableAsString(this, ChannelProperty.Name, out CachedName)) != Error.Ok && timer.ElapsedMilliseconds < 25);
            }
            Library.Api.TryGetChannelVariableAsString(this, ChannelProperty.Topic, out CachedTopic);
        }
    }
}