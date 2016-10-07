using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Teamspeak.Sdk.Client
{
    /// <summary>
    /// A set of values that are used when initializing the client library  
    /// </summary>
    public class LibraryParameters
    {
        /// <summary>
        /// Returns the name of the native sdk binary that fits the current environment
        /// </summary>
        /// <param name="name">name of the native sdk binary</param>
        /// <param name="platform">detected platform</param>
        /// <returns>true if a matching binary exists</returns>
        public static bool TryGetNativeBinaryName(out string name, out SupportedPlatform platform)
        {
            return PlatformSpecific.TryGetNativeBinaryName(out name, out platform);
        }
        
        /// <summary>
        /// Location to the teamspeak library binary.
        /// </summary>
        public string NativeBinary { get; set; }

        /// <summary>
        /// Determines which platform specific code will be executed.
        /// </summary>
        public SupportedPlatform Platform { get; set; }

        /// <summary>
        /// Path pointing to the directory where the soundbackends folder is located.
        /// </summary>
        public string ResourcesFolder { get; set; }

        /// <summary>
        /// The library can output log messages (called by <see cref="Library.Log(LogLevel, string, Connection, string)"/>) to a file (located in the logs directory relative to the client executable), to stdout or to user defined callbacks.
        /// </summary>
        public LogTypes UsedLogTypes { get; set; } = LogTypes.None;

        /// <summary>
        /// Defines the location where the logs are written to. Pass null for the default behavior, which is to use a folder called logs in the current working directory.
        /// </summary>
        public string LogFileFolder { get; set; } = null;

        /// <summary>
        /// Used to hash the password in the same way it is hashed in the outside data store. Or just return the password to send the clear text to the server.
        /// </summary>
        public ClientPasswordEncryptHandler ClientPasswordEncrypt { get; set; } = null;

        /// <summary>
        /// Encrypts outgoing data
        /// </summary>
        public CustomPacketEncryptHandler CustomPacketEncrypt { get; set; } = null;

        /// <summary>
        /// Decrypts incoming data
        /// </summary>
        public CustomPacketDecryptHandler CustomPacketDecrypt { get; set; } = null;

        /// <summary>
        /// Unused by SDK
        /// </summary>
        public IntPtr FunctionRarePointers { get; set; } = IntPtr.Zero;


        /// <summary>
        /// Creates a new <see cref="LibraryParameters"/>-Object.
        /// </summary>
        public LibraryParameters()
            :this (null)
        {
        }

        /// <summary>
        /// Creates a new ClientLibraryLoadInfo-Object.
        /// </summary>
        /// <param name="teamspeakBinaryFolder">location where the native teamspeak sdk files can be found.</param>
        public LibraryParameters(string teamspeakBinaryFolder)
            : this(teamspeakBinaryFolder, null)
        {
        }

        /// <summary>
        /// Creates a new <see cref="LibraryParameters"/>-Object.
        /// </summary>
        /// <param name="teamspeakBinaryFolder">location where the native teamspeak sdk files can be found.</param>
        /// <param name="resourcesFolder">Path pointing to the directory where the soundbackends folder is located.</param>
        public LibraryParameters(string teamspeakBinaryFolder, string resourcesFolder)
        {
            string nativeBinaryName;
            SupportedPlatform platform;
            if (TryGetNativeBinaryName(out nativeBinaryName, out platform) == false)
                throw new NotSupportedException("platform is not supported");

			if (teamspeakBinaryFolder == null || nativeBinaryName == null)
                NativeBinary = nativeBinaryName;
            else
                NativeBinary = System.IO.Path.Combine(teamspeakBinaryFolder, nativeBinaryName);
            Platform = platform;
            ResourcesFolder = resourcesFolder;
        }

        /// <summary>
        /// Creates a new <see cref="LibraryParameters"/>-Object.
        /// </summary>
        /// <param name="nativeBinary">Location to the teamspeak library binary.</param>
        /// <param name="platform">Determines which platform specific code will be executed.</param>
        /// <param name="resourcesFolder">Path pointing to the directory where the soundbackends folder is located.</param>
        public LibraryParameters(string nativeBinary, SupportedPlatform platform, string resourcesFolder)
        {
            NativeBinary = nativeBinary;
            Platform = platform;
            ResourcesFolder = resourcesFolder;
        }
    }
}
