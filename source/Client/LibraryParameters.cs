using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TeamSpeak.Sdk.Client
{
    /// <summary>
    /// A set of values that are used when initializing the client library  
    /// </summary>
    public class LibraryParameters
    {
        /// <summary>
        /// Returns the name of the native sdk binary that fits the current environment
        /// </summary>
        /// <param name="names">possible names of the native sdk binary</param>
        /// <param name="platform">detected platform</param>
        /// <returns>true if a matching binary exists</returns>
        public static bool TryGetNativeBinaryName(out string[] names, out SupportedPlatform platform)
        {
            return PlatformSpecific.TryGetNativeBinaryName(out names, out platform);
        }
        
        /// <summary>
        /// Possible locations to the TeamSpeak library binary.
        /// </summary>
        public string[] PossibleNativeBinaryLocations { get; set; }

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
        public LogTypes UsedLogTypes { get; set; }

        /// <summary>
        /// Defines the location where the logs are written to. Pass null for the default behavior, which is to use a folder called logs in the current working directory.
        /// </summary>
        public string LogFileFolder { get; set; }

        /// <summary>
        /// Used to hash the password in the same way it is hashed in the outside data store. Or just return the password to send the clear text to the server.
        /// </summary>
        public ClientPasswordEncryptHandler ClientPasswordEncrypt { get; set; }

        /// <summary>
        /// Encrypts outgoing data
        /// </summary>
        public CustomPacketEncryptHandler CustomPacketEncrypt { get; set; }

        /// <summary>
        /// Decrypts incoming data
        /// </summary>
        public CustomPacketDecryptHandler CustomPacketDecrypt { get; set; }

        /// <summary>
        /// Unused by SDK
        /// </summary>
        public IntPtr FunctionRarePointers { get; set; }


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
        /// <param name="TeamSpeakBinaryFolder">location where the native TeamSpeak sdk files can be found.</param>
        public LibraryParameters(string TeamSpeakBinaryFolder)
            : this(TeamSpeakBinaryFolder, null)
        {
        }

        /// <summary>
        /// Creates a new <see cref="LibraryParameters"/>-Object.
        /// </summary>
        /// <param name="teamSpeakBinaryFolder">location where the native TeamSpeak sdk files can be found.</param>
        /// <param name="resourcesFolder">Path pointing to the directory where the soundbackends folder is located.</param>
        public LibraryParameters(string teamSpeakBinaryFolder, string resourcesFolder)
        {
            string[] nativeBinaryLocations;
            SupportedPlatform platform;
            if (TryGetNativeBinaryName(out nativeBinaryLocations, out platform) == false)
                throw new NotSupportedException("platform is not supported");

            PossibleNativeBinaryLocations = ExtendPossibleLocations(nativeBinaryLocations, teamSpeakBinaryFolder);
            Platform = platform;
            ResourcesFolder = resourcesFolder;
        }

        private string[] ExtendPossibleLocations(string[] nativeBinaryLocations, string teamSpeakBinaryFolder)
        {
            IEnumerable<string> result = nativeBinaryLocations;
            if (teamSpeakBinaryFolder != null)
            {
                result = result.Select(s => s == null ? null : Path.Combine(teamSpeakBinaryFolder, s));
            }
            if (teamSpeakBinaryFolder == null || Path.IsPathRooted(teamSpeakBinaryFolder) == false)
            {
                string root = Path.GetDirectoryName(Path.GetFullPath(typeof(SdkHandle).Assembly.Location));
                result = result.Concat(result.Select(s => s == null ? null : Path.Combine(root, s)));
            }
            return result.ToArray();
        }

        /// <summary>
        /// Creates a new <see cref="LibraryParameters"/>-Object.
        /// </summary>
        /// <param name="nativeBinaryLocation">Location to the TeamSpeak library binary.</param>
        /// <param name="platform">Determines which platform specific code will be executed.</param>
        /// <param name="resourcesFolder">Path pointing to the directory where the soundbackends folder is located.</param>
        public LibraryParameters(string nativeBinaryLocation, SupportedPlatform platform, string resourcesFolder)
        {
            PossibleNativeBinaryLocations = new string[] { nativeBinaryLocation };
            Platform = platform;
            ResourcesFolder = resourcesFolder;
        }
    }
}
