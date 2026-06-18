using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TeamSpeak.Sdk.Server
{
    public class PermissionChecks
    {
        public event PermissionCheckHandler<PermissionCheckArgs> CanConnect;
        public event PermissionCheckHandler<PermissionCheckArgs> CanGetChannelDescription;
        public event PermissionCheckHandler<KickPermissionArgs> CanKickFromChannel;
        public event PermissionCheckHandler<KickPermissionArgs> CanKickFromServer;
        public event PermissionCheckHandler<MoveClientPermissionArgs> CanMoveClient;
        public event PermissionCheckHandler<MoveChannelPermissionArgs> CanMoveChannel;
        public event PermissionCheckHandler<SendClientTextMessagePermissionArgs> CanSendClientTextMessage;
        public event PermissionCheckHandler<SendChannelTextMessagePermissionArgs> CanSendChannelTextMessage;
        public event PermissionCheckHandler<SendTextMessagePermissionArgs> CanSendServerTextMessage;
        public event PermissionCheckHandler<PermissionCheckArgs> CanGetServerConnectionInfo;
        public event PermissionCheckHandler<ClientConnectionInfoPermissionArgs> CanGetClientConnectionInfo;
        public event PermissionCheckHandler<DeleteChannelPermissionArgs> CanDeleteChannel;
        public event PermissionCheckHandler<SubscribeChannelPermissionArgs> CanSubscribeChannel;
        public event PermissionCheckHandler<InitUploadPermissionArgs> CanInitUpload;
        public event PermissionCheckHandler<InitDownloadPermissionArgs> CanInitDownload;
        public event PermissionCheckHandler<GetFileInfoPermissionArgs> CanGetFileInfo;
        public event PermissionCheckHandler<GetFileListPermissionArgs> CanGetFileList;
        public event PermissionCheckHandler<DeleteFilePermissionArgs> CanDeleteFile;
        public event PermissionCheckHandler<CreateDirectoryPermissionArgs> CanCreateDirectory;
        public event PermissionCheckHandler<RenameFilePermissionArgs> CanRenameFile;
        public event PermissionCheckHandler<UpdateClientPermissionArgs> CanUpdateClient;
        public event PermissionCheckHandler<CreateChannelPermissionArgs> CanCreateChannel;
        public event PermissionCheckHandler<EditChannelPermissionArgs> CanEditChannel;

        protected internal virtual  Error OnCanConnect(Client invoker)
        {
            var args = new PermissionCheckArgs(invoker);
            CanConnect?.Invoke(args);
            return args.Result;
        }

        protected internal virtual  Error OnCanGetChannelDescription(Client invoker)
        {
            var args = new PermissionCheckArgs(invoker);
            CanGetChannelDescription?.Invoke(args);
            return args.Result;
        }

        protected internal virtual  Error OnCanKickFromChannel(Client invoker, ReadOnlyCollection<Client> clients, string reason)
        {
            var args = new KickPermissionArgs(invoker, clients, reason);
            CanKickFromChannel?.Invoke(args);
            return args.Result;
        }

        protected internal virtual  Error OnCanKickFromServer(Client invoker, ReadOnlyCollection<Client> clients, string reason)
        {
            var args = new KickPermissionArgs(invoker, clients, reason);
            CanKickFromServer?.Invoke(args);
            return args.Result;
        }

        protected internal virtual  Error OnCanMoveClient(Client invoker, ReadOnlyCollection<Client> clients, Channel channel, string reason)
        {
            var args = new MoveClientPermissionArgs(invoker, clients, channel, reason);
            CanMoveClient?.Invoke(args);
            return args.Result;
        }

        protected internal virtual  Error OnCanMoveChannel(Client invoker, Channel channel, Channel newParent)
        {
            var args = new MoveChannelPermissionArgs(invoker, channel, newParent);
            CanMoveChannel?.Invoke(args);
            return args.Result;
        }

        protected internal virtual  Error OnCanSendServerTextMessage(Client invoker, string message)
        {
            var args = new SendTextMessagePermissionArgs(invoker, message);
            CanSendServerTextMessage?.Invoke(args);
            return args.Result;
        }

        protected internal virtual  Error OnCanSendChannelTextMessage(Client invoker, Channel channel, string message)
        {
            var args = new SendChannelTextMessagePermissionArgs(invoker, message, channel);
            CanSendChannelTextMessage?.Invoke(args);
            return args.Result;
        }

        protected internal virtual  Error OnCanSendClientTextMessage(Client invoker, Client client, string message)
        {
            var args = new SendClientTextMessagePermissionArgs(invoker, message, client);
            CanSendClientTextMessage?.Invoke(args);
            return args.Result;
        }

        protected internal virtual  Error OnCanGetServerConnectionInfo(Client invoker)
        {
            var args = new PermissionCheckArgs(invoker);
            CanGetServerConnectionInfo?.Invoke(args);
            return args.Result;
        }

        protected internal virtual  Error OnCanGetClientConnectionInfo(Client invoker, ref bool mayViewIpPort, Client targetClient)
        {
            var args = new ClientConnectionInfoPermissionArgs(invoker, mayViewIpPort, targetClient);
            CanGetClientConnectionInfo(args);
            mayViewIpPort = args.MayViewIpPort;
            return args.Result;
        }

        protected internal virtual  Error OnCanDeleteChannel(Client invoker, Channel channel)
        {
            var args = new DeleteChannelPermissionArgs(invoker, channel);
            CanDeleteChannel?.Invoke(args);
            return args.Result;
        }

        protected internal virtual  Error OnCanSubscribeChannel(Client invoker, Channel channel)
        {
            var args = new SubscribeChannelPermissionArgs(invoker, channel);
            CanSubscribeChannel?.Invoke(args);
            return args.Result;
        }

        protected internal virtual  Error OnCanInitUpload(Client invoker, string fileName, ulong fileSize, Channel channel, bool overwrite, bool resume)
        {
            var args = new InitUploadPermissionArgs(invoker, fileName, fileSize, channel, overwrite, resume);
            CanInitUpload?.Invoke(args);
            return args.Result;
        }

        protected internal virtual  Error OnCanInitDownload(Client invoker, string fileName, Channel channel)
        {
            var args = new InitDownloadPermissionArgs(invoker, fileName, channel);
            CanInitDownload?.Invoke(args);
            return args.Result;
        }

        protected internal virtual  Error OnCanGetFileInfo(Client invoker, ReadOnlyCollection<GetFileInfoPermissionArgs.Request> requests)
        {
            var args = new GetFileInfoPermissionArgs(invoker, requests);
            CanGetFileInfo?.Invoke(args);
            return args.Result;
        }

        protected internal virtual  Error OnCanGetFileList(Client invoker, Channel channel, string path)
        {
            var args = new GetFileListPermissionArgs(invoker, path, channel);
            CanGetFileList?.Invoke(args);
            return args.Result;
        }

        protected internal virtual  Error OnCanDeleteFile(Client invoker, Channel channel, ReadOnlyCollection<string> fileNames)
        {
            var args = new DeleteFilePermissionArgs(invoker, channel, fileNames);
            CanDeleteFile(args);
            return args.Result;
        }

        protected internal virtual  Error OnCanCreateDirectory(Client invoker, Channel channel, string directoryName)
        {
            var args = new CreateDirectoryPermissionArgs(invoker, channel, directoryName);
            CanCreateDirectory?.Invoke(args);
            return args.Result;
        }

        protected internal virtual Error OnCanRenameFile(Client invoker, Channel fromChannel, Channel toChannel, string oldFileName, string newFileName)
        {
            var args = new RenameFilePermissionArgs(invoker, fromChannel, toChannel, oldFileName, newFileName);
            CanRenameFile?.Invoke(args);
            return args.Result;
        }

        protected internal virtual Error OnCanUpdateClient(Client invoker, ReadOnlyCollection<ExportedVariable<ClientProperty>> properties)
        {
            var args = new UpdateClientPermissionArgs(invoker, properties);
            CanUpdateClient?.Invoke(args);
            return args.Result;
        }

        internal Error OnCanCreateChannel(Client invoker, Channel parent, ReadOnlyCollection<ExportedVariable<ChannelProperty>> properties)
        {
            var args = new CreateChannelPermissionArgs(invoker, parent, properties);
            CanCreateChannel?.Invoke(args);
            return args.Result;
        }

        internal Error OnCanEditChannel(Client invoker, Channel channel, ReadOnlyCollection<ExportedVariable<ChannelProperty>> properties)
        {
            var args = new EditChannelPermissionArgs(invoker, channel, properties);
            CanEditChannel?.Invoke(args);
            return args.Result;
        }
    }

    public delegate void PermissionCheckHandler<TArgs>(TArgs args) where TArgs : PermissionCheckArgs;

    public class PermissionCheckArgs
    {
        public Client Invoker { get; }
        public Error Result { get; set; }

        public PermissionCheckArgs(Client invoker)
        {
            Invoker = invoker;
            Result = Error.Ok;
        }
    }

    public class KickPermissionArgs : PermissionCheckArgs
    {
        public ReadOnlyCollection<Client> Clients { get; }
        public string Reason { get; }

        public KickPermissionArgs(Client invoker, ReadOnlyCollection<Client> clients, string reason)
            : base(invoker)
        {
            Clients = clients;
            Reason = reason;
        }
    }

    public class MoveClientPermissionArgs : PermissionCheckArgs
    {
        public ReadOnlyCollection<Client> Clients { get; }
        public Channel Channel { get; }
        public string Reason { get; }

        public MoveClientPermissionArgs(Client invoker, ReadOnlyCollection<Client> clients, Channel channel, string reason)
            : base(invoker)
        {
            Clients = clients;
            Channel = channel;
            Reason = reason;
        }
    }

    public class MoveChannelPermissionArgs : PermissionCheckArgs
    {
        public Channel Channel { get; }
        public Channel NewParent { get; }

        public MoveChannelPermissionArgs(Client invoker, Channel channel, Channel newParent)
            : base(invoker)
        {
            Channel = channel;
            NewParent = newParent;
        }
    }

    public class SendTextMessagePermissionArgs : PermissionCheckArgs
    {
        public string TextMessage { get; }

        public SendTextMessagePermissionArgs(Client invoker, string textMessage)
            : base(invoker)
        {
            TextMessage = textMessage;
        }
    }

    public class SendClientTextMessagePermissionArgs : SendTextMessagePermissionArgs
    {
        public Client Client { get; }

        public SendClientTextMessagePermissionArgs(Client invoker, string textMessage, Client client)
            : base(invoker, textMessage)
        {
            Client = client;
        }
    }

    public class SendChannelTextMessagePermissionArgs : SendTextMessagePermissionArgs
    {
        public Channel Channel { get; }

        public SendChannelTextMessagePermissionArgs(Client invoker, string textMessage, Channel channel)
            : base(invoker, textMessage)
        {
            Channel = channel;
        }
    }

    public class ClientConnectionInfoPermissionArgs : PermissionCheckArgs
    {
        public bool MayViewIpPort { get; set; }
        public Client Client { get; }

        public ClientConnectionInfoPermissionArgs(Client invoker, bool mayViewIpPort, Client client)
            : base(invoker)
        {
            MayViewIpPort = mayViewIpPort;
            Client = client;
        }
    }

    public class DeleteChannelPermissionArgs : PermissionCheckArgs
    {
        public Channel Channel { get; }

        public DeleteChannelPermissionArgs(Client invoker, Channel channel)
            : base(invoker)
        {
            Channel = channel;
        }
    }

    public class SubscribeChannelPermissionArgs : PermissionCheckArgs
    {
        public Channel Channel { get; }

        public SubscribeChannelPermissionArgs(Client invoker, Channel channel)
            : base(invoker)
        {
            Channel = channel;
        }
    }

    public class InitUploadPermissionArgs : PermissionCheckArgs
    {
        /// <summary>
        /// The file name
        /// </summary>
        public string FileName { get; }
        /// <summary>
        /// The file size
        /// </summary>
        public ulong FileSize { get; }
        /// <summary>
        /// The <see cref="Channel"/> where the file is to be uploaded
        /// </summary>
        public Channel TargetChannel { get; }
        /// <summary>
        /// If <see langword="true"/> to overwrite files; otherwise <see langword="false"/>: prevent overwrites
        /// </summary>
        public bool Overwrite { get; }
        /// <summary>
        /// if <see langword="true"/>: resumes an existing upload; otherwise <see langword="false"/> starts a new upload
        /// </summary>
        public bool Resume { get; }

        public InitUploadPermissionArgs(Client invoker, string fileName, ulong fileSize, Channel channel, bool overwrite, bool resume)
            : base(invoker)
        {
            FileName = fileName;
            FileSize = fileSize;
            TargetChannel = channel;
            Overwrite = overwrite;
            Resume = resume;
        }
    }

    public class InitDownloadPermissionArgs : PermissionCheckArgs
    {
        /// <summary>
        /// The file name
        /// </summary>
        public string FileName { get; }
        /// <summary>
        /// The <see cref="Channel"/> where the file is to be downloaded from
        /// </summary>
        public Channel Channel { get; }

        public InitDownloadPermissionArgs(Client invoker, string fileName, Channel channel)
            : base(invoker)
        {
            FileName = FileName;
            Channel = channel;
        }
    }

    public class GetFileInfoPermissionArgs : PermissionCheckArgs
    {
        public class Request
        {
            /// <summary>
            /// The file name
            /// </summary>
            public string FileName { get; }
            /// <summary>
            /// The <see cref="Channel"/> where the file is located
            /// </summary>
            public Channel Channel { get; }

            public Request(string fileName, Channel channel)
            {
                FileName = fileName;
                Channel = channel;
            }
        }

        public ReadOnlyCollection<Request> Requests { get; }

        public GetFileInfoPermissionArgs(Client invoker, ReadOnlyCollection<Request> requests)
            : base(invoker)
        {
            Requests = requests;
        }
    }

    public class GetFileListPermissionArgs : PermissionCheckArgs
    {
        /// <summary>
        /// The path where to get the list for.
        /// </summary>
        public string Path { get; }
        /// <summary>
        /// The <see cref="Channel"/>
        /// </summary>
        public Channel Channel { get; }

        public GetFileListPermissionArgs(Client invoker, string path, Channel channel)
            : base(invoker)
        {
            Path = path;
            Channel = channel;
        }
    }

    public class DeleteFilePermissionArgs : PermissionCheckArgs
    {
        /// <summary>
        /// The <see cref="Channel"/> where the file is to be deleted.
        /// </summary>
        public Channel Channel { get; }

        /// <summary>
        /// The file names to be deleted.
        /// </summary>
        public ReadOnlyCollection<string> FileNames { get; }

        public DeleteFilePermissionArgs(Client invoker, Channel channel, ReadOnlyCollection<string> fileNames)
            : base(invoker)
        {
            Channel = channel;
            FileNames = fileNames;
        }
    }

    public class CreateDirectoryPermissionArgs : PermissionCheckArgs
    {
        /// <summary>
        /// The <see cref="Channel"/> where the directory is to be created in.
        /// </summary>
        public Channel Channel { get; }
        /// <summary>
        /// The name of the directory to be created
        /// </summary>
        public string DirectoryName { get; }

        public CreateDirectoryPermissionArgs(Client invoker, Channel channel, string directoryName)
            : base(invoker)
        {
            Channel = channel;
            DirectoryName = directoryName;
        }
    }

    public class RenameFilePermissionArgs : PermissionCheckArgs
    {
        /// <summary>
        /// The <see cref="Channel"/> where the file is located now.
        /// </summary>
        public Channel FromChannel { get; }
        /// <summary>
        /// The <see cref="Channel"/> where the file is to be moved to.
        /// </summary>
        public Channel ToChannel { get; }
        /// <summary>
        /// The current file name.
        /// </summary>
        public string OldFileName { get; }
        /// <summary>
        /// The new file name.
        /// </summary>
        public string NewFileName { get; }

        public RenameFilePermissionArgs(Client invoker, Channel fromChannel, Channel toChannel, string oldFileName, string newFileName)
            : base(invoker)
        {
            FromChannel = fromChannel;
            ToChannel = toChannel;
            OldFileName = oldFileName;
            NewFileName = newFileName;
        }
    }

    public class UpdateClientPermissionArgs : PermissionCheckArgs
    {
        /// <summary>
        /// The properties to be updated
        /// </summary>
        public ReadOnlyCollection<ExportedVariable<ClientProperty>> Properties { get; }

        public UpdateClientPermissionArgs(Client invoker, ReadOnlyCollection<ExportedVariable<ClientProperty>> properties)
            : base(invoker)
        {
            Properties = properties;
        }
    }

    public class CreateChannelPermissionArgs : PermissionCheckArgs
    {
        public Channel Parent { get; }

        /// <summary>
        /// The properties of the new channel
        /// </summary>
        public ReadOnlyCollection<ExportedVariable<ChannelProperty>> Properties { get; }

        public CreateChannelPermissionArgs(Client invoker, Channel parent, ReadOnlyCollection<ExportedVariable<ChannelProperty>> properties)
            : base(invoker)
        {
            Parent = parent;
            Properties = properties;
        }
    }
    public class EditChannelPermissionArgs : PermissionCheckArgs
    {
        public Channel Channel { get; }

        /// <summary>
        /// The properties of the new channel
        /// </summary>
        public ReadOnlyCollection<ExportedVariable<ChannelProperty>> Properties { get; }

        public EditChannelPermissionArgs(Client invoker, Channel channel, ReadOnlyCollection<ExportedVariable<ChannelProperty>> properties)
            : base(invoker)
        {
            Channel = channel;
            Properties = properties;
        }
    }
}
