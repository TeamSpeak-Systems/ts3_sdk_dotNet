using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Teamspeak.Sdk.Client
{
    /// <summary>
    /// An ongoing file transfer
    /// </summary>
    public class FileTransfer: IEquatable<FileTransfer>
    {
        /// <summary>
        /// ID of the client
        /// </summary>
        public ushort ID { get; }

        /// <summary>
        /// Server Connection
        /// </summary>
        public Connection Connection { get; }

        /// <summary>
        /// Creates a new FileTransfer-Object
        /// </summary>
        /// <remarks>does not start a transfer. To transfer a file use <see cref="Channel.SendFile(string, string, bool, bool, string, string)"/>  and <see cref="Channel.RequestFile(string, string, bool, bool, string, string)"/></remarks>
        /// <param name="connection">the server connection</param>
        /// <param name="id">ID of the client</param>
        public FileTransfer(Connection connection, ushort id)
        {
            Require.NotNull(nameof(connection), connection);
            ID = id;
            Connection = connection;
        }

        /// <summary>
        /// the file name
        /// </summary>
        public string Name { get { return Library.Api.GetTransferFileName(this); } }

        /// <summary>
        /// the file path
        /// </summary>
        public string Path { get { return Library.Api.GetTransferFilePath(this); } }

        /// <summary>
        /// the remote path on the server
        /// </summary>
        public string RemotePath { get { return Library.Api.GetTransferFileRemotePath(this); } }

        /// <summary>
        /// the file size
        /// </summary>
        public ulong Size { get { return Library.Api.GetTransferFileSize(this); } }

        /// <summary>
        /// the currently transferred file size 
        /// </summary>
        public ulong SizeDone { get { return Library.Api.GetTransferFileSizeDone(this); } }

        /// <summary>
        /// specifies if the transfer an upload or a download.
        /// </summary>
        public TransferMode Mode { get { return Library.Api.IsTransferSender(this) ? TransferMode.Upload : TransferMode.Download; } }

        /// <summary>
        /// the status of the specified transfer
        /// </summary>
        public FileTransferState Status { get { return Library.Api.GetTransferStatus(this); } }

        /// <summary>
        /// the current speed of the transfer
        /// </summary>
        public float CurrentSpeed { get { return Library.Api.GetCurrentTransferSpeed(this); } }

        /// <summary>
        /// the average speed of the specified transfer
        /// </summary>
        public float AverageSpeed { get { return Library.Api.GetAverageTransferSpeed(this); } }

        /// <summary>
        /// the time the transfer has used
        /// </summary>
        public TimeSpan RunTime { get { return TimeSpan.FromMilliseconds(Library.Api.GetTransferRunTime(this)); } }

        /// <summary>
        /// To get the speed limit for the transfer in bytes/s:
        /// </summary>
        public ulong SpeedLimit
        {
            get { return Library.Api.GetTransferSpeedLimit(this); }
            set { Library.Api.SetTransferSpeedLimit(this, value); }
        }

        /// <summary>
        /// Abort the transfer
        /// </summary>
        /// <param name="deleteUnfinishedFile">true to delete the unfinished file; otherwise, keep file so it can be resumed.</param>
        public void Halt(bool deleteUnfinishedFile = false)
        {
            Library.Api.HaltTransfer(this, deleteUnfinishedFile, null);
        }

        /// <summary>
        /// Compares two <see cref="FileTransfer"/> for equality. 
        /// </summary>
        /// <param name="a">The first <see cref="FileTransfer"/> structure to compare.</param>
        /// <param name="b">The second <see cref="FileTransfer"/> structure to compare.</param>
        /// <returns>true if the ID and Mode of a and b are equal; otherwise, false.</returns>
        public static bool operator ==(FileTransfer a, FileTransfer b)
        {
            if (object.ReferenceEquals(a, b)) return true;
            if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null)) return false;
            return a.Connection == b.Connection && a.ID == b.ID;
        }

        /// <summary>
        /// Compares two <see cref="FileTransfer"/> for inequality. 
        /// </summary>
        /// <param name="a">The first <see cref="FileTransfer"/> structure to compare.</param>
        /// <param name="b">The second <see cref="FileTransfer"/> structure to compare.</param>
        /// <returns>true if the ID and Mode of a and b are different; otherwise, false.</returns>
        public static bool operator !=(FileTransfer a, FileTransfer b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Indicates whether this instance and a another instance  are equal.
        /// </summary>
        /// <param name="other">Another instance to compare to.</param>
        /// <returns>true if this and  the other instance represent the same value; otherwise, false.</returns>
        public bool Equals(FileTransfer other)
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
            return obj is FileTransfer && this == (FileTransfer)obj;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return Connection.ID.GetHashCode() * 7 + ID.GetHashCode();
        }
    }
}