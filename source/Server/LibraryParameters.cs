using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TeamSpeak.Sdk.Server
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
        /// <returns><see langword="true"/> if a matching binary exists</returns>
        public static bool TryGetNativeBinaryName(out string[] names, out SupportedPlatform platform)
        {
            return PlatformSpecific.TryGetNativeBinaryNameServer(out names, out platform);
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
        /// The library can output log messages (called by <see cref="Library.Log(LogLevel, string, Connection, string)"/>) to a file (located in the logs directory relative to the client executable), to stdout or to user defined callbacks.
        /// </summary>
        public LogTypes UsedLogTypes { get; set; } = LogTypes.None;

        /// <summary>
        /// Defines the location where the logs are written to. Pass <see langword="null"/> for the default behavior, which is to use a folder called logs in the current working directory.
        /// </summary>
        public string LogFileFolder { get; set; } = null;

        public HashSet<ClientCommand> DisabledCommands { get; } = new HashSet<ClientCommand>();

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
        /// If <see langword="true"/>: every change to a Property of a <see cref="Sdk.Server.Channel"/>
        /// or a <see cref="Sdk.Server.Client"/> are automatically flushed. Otherwise 
        /// when <see langword="false"/>: Changes to Properties must be flushed using 
        /// <see cref="Sdk.Server.Channel.FlushVariables"/> and <see cref="Sdk.Server.Client.FlushVariables"/>.
        /// </summary>
        public bool AutoFlush { get; set; } = true;

        public FileManagerParameters FileManager { get; set; } = new FileManagerParameters();

        /// <summary>
        /// Creates a new <see cref="LibraryParameters"/>-Object.
        /// </summary>
        public LibraryParameters()
            : this(null)
        {
        }

        /// <summary>
        /// Creates a new <see cref="LibraryParameters"/>-Object.
        /// </summary>
        /// <param name="teamSpeakBinaryFolder">location where the native TeamSpeak sdk files can be found.</param>
        public LibraryParameters(string teamSpeakBinaryFolder)
        {
            string[] nativeBinaryLocations;
            SupportedPlatform platform;
            if (TryGetNativeBinaryName(out nativeBinaryLocations, out platform) == false)
                throw new NotSupportedException("platform is not supported");
            
            PossibleNativeBinaryLocations = ExtendPossibleLocations(nativeBinaryLocations, teamSpeakBinaryFolder);
            Platform = platform;
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
        public LibraryParameters(string nativeBinaryLocation, SupportedPlatform platform)
        {
            PossibleNativeBinaryLocations = new string[] { nativeBinaryLocation };
            Platform = platform;
        }
    }
}