using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamSpeak.Sdk.Server
{
    /// <summary>
    /// The TeamSpeak SDK includes the ability to support filetransfer,
    /// like the regular TeamSpeak server and client offer. The server can function as
    /// a file storage, which can be accessed by clients who can up- and download files.
    /// Files are stored on the filesystem where the server is running.
    /// </summary>
    public class FileManager
    {
        /// <summary>
        /// If <see langword="true"/> the TeamSpeak FileManager will be initialized
        /// </summary>
        public bool Enabled { get; }
        /// <summary>
        /// The base folder where to save the files to. 
        /// </summary>
        /// <remarks>
        /// If this is initalized to "sdk_files", we would get the following directory structure:
        /// * All files for virtual server with id 1 will be in "sdk_files/virtualserver_1".
        /// * Files in channel 1 in virtual server 1 will be in "sdk_files/virtualserver_1/channel_1"
        /// * These directories will automatically be created by the server if they do not exist. It is the responsibility of the application (not serverlib) to delete these directories when they are no longer needed.
        /// </remarks>
        public string FileBase { get; }
        /// <summary>
        /// List of IPs to bind to. This can be <see langword="null"/> or empty, in which case it will bind to 0.0.0.0.
        /// </summary>
        /// <remarks>
        /// Only add ips, not hostnames
        /// </remarks>
        public ICollection<string> IPs { get; }
        /// <summary>
        /// Specifies the TCP port used for filetransfer. Free to choose between 1 and 65535. TeamSpeak defaults to 30033.
        /// </summary>
        public int Port { get; }
        /// <summary>
        /// Download limit in bytes/s or <see cref="ulong.MaxValue"/> for unlimited/>
        /// </summary>
        public ulong DownloadBandwitdh { get; }
        /// <summary>
        /// Download limit in bytes/s or <see cref="ulong.MaxValue"/> for unlimited/>
        /// </summary>
        public ulong UploadBandwidth { get; }

        internal FileManager(FileManagerParameters parameters)
        {
            Enabled = parameters.Enabled;
            FileBase = parameters.FileBase;
            IPs = new List<string>(parameters.IPs).AsReadOnly();
            Port = parameters.Port;
            DownloadBandwitdh = parameters.DownloadBandwitdh;
            UploadBandwidth = parameters.UploadBandwidth;
        }
    }

    /// <summary>
    /// The TeamSpeak SDK includes the ability to support filetransfer,
    /// like the regular TeamSpeak server and client offer. The server can function as
    /// a file storage, which can be accessed by clients who can up- and download files.
    /// Files are stored on the filesystem where the server is running.
    /// </summary>
    public class FileManagerParameters
    {
        /// <summary>
        /// If <see langword="true"/> the TeamSpeak FileManager will be initialized
        /// </summary>
        public bool Enabled { get; set; } = false;
        /// <summary>
        /// The base folder where to save the files to. 
        /// </summary>
        /// <remarks>
        /// If this is initalized to "sdk_files", we would get the following directory structure:
        /// * All files for virtual server with id 1 will be in "sdk_files/virtualserver_1".
        /// * Files in channel 1 in virtual server 1 will be in "sdk_files/virtualserver_1/channel_1"
        /// * These directories will automatically be created by the server if they do not exist. It is the responsibility of the application (not serverlib) to delete these directories when they are no longer needed.
        /// </remarks>
        public string FileBase { get; set; } = "sdk_files";
        /// <summary>
        /// List of IPs to bind to. This can be <see langword="null"/> or empty, in which case it will bind to 0.0.0.0.
        /// </summary>
        /// <remarks>
        /// Only add ips, not hostnames
        /// </remarks>
        public List<string> IPs { get; set; } = new List<string>();
        /// <summary>
        /// Specifies the TCP port used for filetransfer. Free to choose between 1 and 65535. TeamSpeak defaults to 30033.
        /// </summary>
        public int Port { get; set; } = 30033;
        /// <summary>
        /// Download limit in bytes/s or <see cref="ulong.MaxValue"/> for unlimited/>
        /// </summary>
        public ulong DownloadBandwitdh { get; set; } = ulong.MaxValue;
        /// <summary>
        /// Download limit in bytes/s or <see cref="ulong.MaxValue"/> for unlimited/>
        /// </summary>
        public ulong UploadBandwidth { get; set; } = ulong.MaxValue;
    }
}
