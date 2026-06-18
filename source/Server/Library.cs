using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TeamSpeak.Sdk.Server
{
    /// <summary>
    /// TeamSpeak Server Library
    /// </summary>
    public static class Library
    {
        private static SdkHandle Handle;
        private static NativeMethods NativeMethods;
        private static NativeEvents Events;
        private static readonly ReaderWriterLock InitializedLock = new ReaderWriterLock();
        private static ConcurrentDictionary<ulong, VirtualServer> _virtualServersCache;
        private static volatile bool ProcessExitRegistered = false;
        private static volatile bool _AutoFlush = false;
        private static volatile LogLevel _LogLevel = LogLevel.Devel;
        private static readonly HashSet<ClientCommand> _DisabledCommands = new HashSet<ClientCommand>();

        internal static NativeMethods Api
        {
            get
            {
                InitializedLock.AcquireReaderLock(Timeout.Infinite);
                try
                {
                    if (IsInitialized == false)
                    {
                        LockCookie cookie = InitializedLock.UpgradeToWriterLock(Timeout.Infinite);
                        try
                        {
                            if (IsInitialized == false)
                            {
                                Initialize();
                            }
                        }
                        finally
                        {
                            InitializedLock.DowngradeFromWriterLock(ref cookie);
                        }
                    }
                    return NativeMethods;
                }
                finally
                {
                    InitializedLock.ReleaseReaderLock();
                }
            }
        }

        /// <summary>
        /// <see langword="true"/> if the TeamSpeak <see cref="Library"/> has been loaded and initialized; otherwise, <see langword="false"/>
        /// </summary>
        public static bool IsInitialized
        {
            get
            {
                SdkHandle handle = Handle;
                return handle != null && handle.IsClosed == false;
            }
        }

        /// <summary>
        /// Initializes the TeamSpeak <see cref="Library"/>
        /// </summary>
        /// <remarks>
        /// Explicitly loads the TeamSpeak clientlib. Will be automatically invoked by the SDK when required.
        /// </remarks>
        public static void Initialize()
        {
            Initialize(new LibraryParameters());
        }

        /// <summary>
        /// Initializes the TeamSpeak <see cref="Library"/>
        /// </summary>
        /// <param name="usedLogTypes">The library can output log messages to a file (located in the logs directory relative to the client executable), to stdout or to user defined callbacks.</param>
        public static void Initialize(LogTypes usedLogTypes)
        {
            Initialize(new LibraryParameters() { UsedLogTypes = usedLogTypes });
        }


        /// <summary>
        /// Initializes the TeamSpeak <see cref="Library"/>
        /// </summary>
        /// <param name="parameters">Information used to create the instance</param>
        /// <exception cref="InvalidOperationException">a <see cref="Library"/> is already created</exception>
        /// <exception cref="NullReferenceException"><paramref name="parameters"/> is <see langword="null"/></exception>
        public static void Initialize(LibraryParameters parameters)
        {
            Sdk.Require.NotNull(nameof(parameters), parameters);

            InitializedLock.AcquireWriterLock(Timeout.Infinite);
            try
            {
                if (Handle != null) throw new InvalidOperationException("Library is already initialized");
                if (ProcessExitRegistered == false)
                {
                    AppDomain.CurrentDomain.ProcessExit += ProcessExit;
                    ProcessExitRegistered = true;
                }
                Platform = parameters.Platform;
                Handle = SdkHandle.LoadServerLibrary(Platform, parameters.PossibleNativeBinaryLocations);
                NativeBinary = Handle.Location;
                UsedLogTypes = parameters.UsedLogTypes;
                LogFileFolder = parameters.LogFileFolder;
                ClientPasswordEncrypt = parameters.ClientPasswordEncrypt;
                CustomPacketEncrypt = parameters.CustomPacketEncrypt;
                CustomPacketDecrypt = parameters.CustomPacketDecrypt;
                FileManager = new FileManager(parameters.FileManager);
                Events = new NativeEvents(
                    useClientPasswordEncrypt: ClientPasswordEncrypt != null,
                    useCustomPacketEncrypt: CustomPacketEncrypt != null,
                    useCustomPacketDecrypt: CustomPacketDecrypt != null);
                _virtualServersCache = new ConcurrentDictionary<ulong, VirtualServer>();

                NativeMethods nativeMethods = new NativeMethods(Handle);
                nativeMethods.InitServerLib(Events.ServerLibFunctions, UsedLogTypes, LogFileFolder);
                NativeMethods = nativeMethods;

                lock (_DisabledCommands)
                {
                    _DisabledCommands.Clear();
                    foreach (ClientCommand command in parameters.DisabledCommands)
                        DisableCommand(command);
                }

                if (FileManager.Enabled)
                    Api.EnableFileManager(FileManager.FileBase, FileManager.IPs.ToArray(), FileManager.Port, FileManager.DownloadBandwitdh, FileManager.UploadBandwidth);
            }
            finally
            {
                InitializedLock.ReleaseWriterLock();
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="Library"/>
        /// </summary>
        public static void Destroy()
        {
            Handle?.Dispose();
        }

        private static void ProcessExit(object sender, EventArgs e)
        {
            Destroy();
        }

        /// <summary>
        /// Location to the TeamSpeak library binary.
        /// </summary>
        public static string NativeBinary { get; private set; }
        /// <summary>
        /// Platform the library is running on
        /// </summary>
        /// <remarks>
        /// Used to determine how the native library will loaded and unloaded.
        /// </remarks>
        public static SupportedPlatform Platform { get; private set; }

        /// <summary>
        /// If user-defined logging was enabled when initializing the Client Lib by setting <see cref="LogTypes.Userlogging"/> in <see cref="LibraryParameters.UsedLogTypes"/>, log messages will be sent to the callback, which allows user customizable logging and handling
        /// </summary>
        public static event UserLoggingMessageEventHandler UserLogMessage;

        /// <summary>
        /// The library can output log messages to a file (located in the logs directory relative to the client executable), to stdout or to user defined callbacks.
        /// </summary>
        public static LogTypes UsedLogTypes { get; private set; }
        /// <summary>
        /// Defines the location where the logs are written to. Pass <see langword="null"/> for the default behavior, which is to use a folder called logs in the current working directory.
        /// </summary>
        public static string LogFileFolder { get; private set; }
        public static LogLevel LogLevel
        {
            get { return _LogLevel; }
            set
            {
                Api.SetLogVerbosity(value);
                _LogLevel = value;
            }
        }
        public static ICollection<VirtualServer> VirtualServers { get { return Api.GetVirtualServerList()?.AsReadOnly(); } }

        public static string Version
        {
            get { return Api.GetServerLibVersion(); }
        }

        public static ulong VersionNumber
        {
            get { return Api.GetServerLibVersionNumber(); }
        }

        public static FileManager FileManager { get; private set; }

        /// <summary>
        /// If <see langword="true"/>: every change to a property of a <see cref="Sdk.Server.Channel"/>,
        /// <see cref="Sdk.Server.Client"/> or <see cref="VirtualServer"/> are automatically flushed. Otherwise 
        /// when <see langword="false"/>: changes to properties must be flushed using 
        /// <see cref="Sdk.Server.Channel.FlushVariables"/>, <see cref="Sdk.Server.Client.FlushVariables"/> and <see cref="Sdk.Server.VirtualServer.FlushVariables"/> respectivly.
        /// </summary>
        public static bool AutoFlush
        {
            get { return _AutoFlush; }
            set { _AutoFlush = value; }
        }

        public static HashSet<ClientCommand> DisabledCommands
        {
            get { lock (_DisabledCommands) return new HashSet<ClientCommand>(_DisabledCommands); }
        }

        public static void DisableCommand(ClientCommand command)
        {
            lock (_DisabledCommands)
            {
                Api.DisableClientCommand(command);
                _DisabledCommands.Add(command);
            }
        }

        public static VirtualServer CreateVirtualServer(uint maxClients, string keyPair = null, uint port = 0, string ip = null, string name = null)
        {
            var virtualServer = Api.CreateVirtualServer(port == 0 ? 9987 : port, ip ?? "0.0.0.0", name, keyPair, maxClients);
            AddVirtualServer(virtualServer.ID, virtualServer);
            return virtualServer;
        }

        /// <summary>
        /// Gets a descriptive text message for a error-code
        /// </summary>
        /// <param name="error">the error-code</param>
        /// <returns>a descriptive text message</returns>
        public static string GetErrorMessage(Error error)
        {
            string result;
            error = Api.TryGetGlobalErrorMessage(error, out result);
            if (error != Error.Ok) throw CreateException(error);
            return result;
        }

        internal static Exception CreateException(Error error, string extraMessage = null)
        {
            string message;
            if (Api.TryGetGlobalErrorMessage(error, out message) != Error.Ok)
                message = error.ToString();
            TeamSpeakException result = new TeamSpeakException(error, message);
            if (extraMessage != null)
                result.Data.Add("extraMessage", extraMessage);
            return result;
        }

        internal static void AddVirtualServer(ulong id, VirtualServer virtualServer)
        {
            _virtualServersCache.AddOrUpdate(id, virtualServer, (_1, _2) => virtualServer);
        }

        internal static VirtualServer GetVirtualServer(ulong id)
        {
            VirtualServer virtualServer;
            if (_virtualServersCache.TryGetValue(id, out virtualServer))
            {
                return virtualServer;
            }
            return null;
        }

        internal static VirtualServer GetOrAddVirtualServer(ulong id)
        {
            return _virtualServersCache.GetOrAdd(id, _ => new VirtualServer(id));
        }

        internal static void RemoveVirtualServer(ulong id)
        {
            VirtualServer virtualServer;
            _virtualServersCache.TryRemove(id, out virtualServer);
        }

        internal static void OnUserLoggingMessage(string message, LogLevel level, string channel, VirtualServer virtualServer, string time, string completeString)
        {
            EventHelper.Run(UserLogMessage, _ => _(message, level, channel, virtualServer, time, completeString));
        }

        /// <summary>
        /// Used to hash the password in the same way it is hashed in the outside data store. Or just return the password to send the clear text to the server.
        /// </summary>
        public static ClientPasswordEncryptHandler ClientPasswordEncrypt { get; private set; }

        /// <summary>
        /// Encrypts outgoing data
        /// </summary>
        public static CustomPacketEncryptHandler CustomPacketEncrypt { get; private set; }

        /// <summary>
        /// Decrypts incoming data
        /// </summary>
        public static CustomPacketDecryptHandler CustomPacketDecrypt { get; private set; }

        #region Events
        internal static void OnClientPasswordEncrypt(string plaintext, byte[] encryptedText)
        {
            ClientPasswordEncrypt?.Invoke(plaintext, encryptedText);
        }

        internal static void OnCustomPacketEncrypt(ref IntPtr dataToSend, ref uint sizeOfData)
        {
            CustomPacketEncrypt?.Invoke(ref dataToSend, ref sizeOfData);
        }

        internal static void OnCustomPacketDecrypt(ref IntPtr dataReceived, ref uint dataReceivedSize)
        {
            CustomPacketDecrypt?.Invoke(ref dataReceived, ref dataReceivedSize);
        }
        #endregion Events
    }
}