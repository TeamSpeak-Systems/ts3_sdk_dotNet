using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Teamspeak.Sdk.Client
{
    /// <summary>
    /// Information about a file or directory stored on a teamspeak server
    /// </summary>
    public class FileInfo
    {
        /// <summary>
        /// <see cref="Channel"/> for which a <see cref="FileInfo"/> was requested.
        /// </summary>
        public Channel Channel { get; }
        /// <summary>
        /// Subdirectory inside the channel for which the FileInfo was requested. “/” indicates the root directory is listed.
        /// </summary>
        public string Path { get; }
        /// <summary>
        /// File name.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// File size,
        /// </summary>
        public ulong Size { get; }
        /// <summary>
        /// The date and time on which the file was last modified.
        /// </summary>
        public DateTimeOffset LastModified { get; }
        /// <summary>
        /// Indicates if this entry is a directory or a file. 
        /// </summary>
        public FileListType Type { get; }
        /// <summary>
        /// If the file is currently still being transferred, this indicates the currently transferred file size.
        /// </summary>
        public ulong IncompleteSize { get; }

        /// <summary>
        /// Creates a <see cref="FileInfo"/>-Object
        /// </summary>
        /// <param name="channel"><see cref="Channel"/> for which a <see cref="FileInfo"/> was requested.</param>
        /// <param name="path">Subdirectory inside the channel for which the FileInfo was requested. “/” indicates the root directory is listed.</param>
        /// <param name="name">File name.</param>
        /// <param name="size">File size.</param>
        /// <param name="lastModified">The date and time on which the file was last modified.</param>
        /// <param name="type">Indicates if this entry is a directory or a file. </param>
        /// <param name="incompleteSize"> If the file is currently still being transferred, this indicates the currently transferred file size.</param>
        public FileInfo(Channel channel, string path, string name, ulong size, DateTimeOffset lastModified, FileListType type, ulong incompleteSize)
        {
            Require.NotNull(nameof(channel), channel);
            Channel = channel;
            Path = path;
            Name = name;
            Size = size;
            LastModified = lastModified;
            Type = type;
            IncompleteSize = incompleteSize;
        }
    }
}
