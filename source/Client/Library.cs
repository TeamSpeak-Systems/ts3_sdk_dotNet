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

namespace TeamSpeak.Sdk.Client
{
    /// <summary>
    /// TeamSpeak Client Library
    /// </summary>
    public static class Library
    {

        private static SdkHandle Handle;
        private static NativeMethods NativeMethods;
        private static NativeEvents Events;
        private static ReaderWriterLock InitializedLock = new ReaderWriterLock();
        private static ConcurrentDictionary<ulong, Connection> ConnectionsCache;
        private static List<CustomDevice> CustomDevicesCache;
        private static bool ProcessExitRegistered = false;

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
        /// true if the TeamSpeak library has been loaded and initialized; otherwise, false
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
        /// Initializes the TeamSpeak clientlib
        /// </summary>
        /// <remarks>
        /// Explicitly loads the TeamSpeak clientlib. Will be automatically invoked by the SDK when required.
        /// </remarks>
        public static void Initialize()
        {
            Initialize(new LibraryParameters());
        }

        /// <summary>
        /// Initializes the TeamSpeak clientlib
        /// </summary>
        /// <param name="usedLogTypes">The library can output log messages (called by <see cref="Library.Log(LogLevel, string, Connection, string)"/>) to a file (located in the logs directory relative to the client executable), to stdout or to user defined callbacks.</param>
        public static void Initialize(LogTypes usedLogTypes)
        {
            Initialize(new LibraryParameters() { UsedLogTypes = usedLogTypes });
        }

        /// <summary>
        /// Creates a new <see cref="Library"/>-Instance
        /// </summary>
        /// <param name="parameters">Information used to create the instance</param>
        /// <exception cref="InvalidOperationException">a <see cref="Library"/> is already created</exception>
        /// <exception cref="NullReferenceException"><paramref name="parameters"/> is null</exception>
        public static void Initialize(LibraryParameters parameters)
        {
            Require.NotNull(nameof(parameters), parameters);

            InitializedLock.AcquireWriterLock(Timeout.Infinite);
            try
            {
                if (IsInitialized) throw new InvalidOperationException("Library is already initialized");
                else if (Handle != null) Handle.WaitForUnloadingFinished();
                if (ProcessExitRegistered == false)
                {
                    AppDomain.CurrentDomain.ProcessExit += ProcessExit;
                    ProcessExitRegistered = true;
                }
                Platform = parameters.Platform;
                Handle = SdkHandle.Load(Platform, parameters.NativeBinary);
                NativeBinary = Handle.Location;
                FunctionRarePointers = parameters.FunctionRarePointers;
                UsedLogTypes = parameters.UsedLogTypes;
                LogFileFolder = parameters.LogFileFolder;
                ResourcesFolder = parameters.ResourcesFolder ?? System.IO.Path.GetDirectoryName(NativeBinary);
                ClientPasswordEncrypt = parameters.ClientPasswordEncrypt;
                CustomPacketEncrypt = parameters.CustomPacketEncrypt;
                CustomPacketDecrypt = parameters.CustomPacketDecrypt;
                Events = new NativeEvents(
                    useClientPasswordEncrypt: ClientPasswordEncrypt != null,
                    useCustomPacketEncrypt: CustomPacketEncrypt != null,
                    useCustomPacketDecrypt: CustomPacketDecrypt != null);
                ConnectionsCache = new ConcurrentDictionary<ulong, Connection>();
                CustomDevicesCache = new List<CustomDevice>();

				if (ResourcesFolder == null)
					ResourcesFolder = string.Empty;
				else if (ResourcesFolder.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString(), StringComparison.InvariantCulture) == false)
                    ResourcesFolder += System.IO.Path.DirectorySeparatorChar;

                NativeMethods nativeMethods = new NativeMethods(Handle);
                nativeMethods.InitClientLib(Events.ClientUIFunctions, FunctionRarePointers, UsedLogTypes, LogFileFolder, ResourcesFolder);
                NativeMethods = nativeMethods;
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
        /// Unused by SDK
        /// </summary>
        public static IntPtr FunctionRarePointers { get; private set; }
        /// <summary>
        /// The library can output log messages (called by <see cref="Log(LogLevel, string, Connection, string)"/>) to a file (located in the logs directory relative to the client executable), to stdout or to user defined callbacks.
        /// </summary>
        public static LogTypes UsedLogTypes { get; private set; }
        /// <summary>
        /// Defines the location where the logs are written to. Pass null for the default behavior, which is to use a folder called logs in the current working directory.
        /// </summary>
        public static string LogFileFolder { get; private set; }
        /// <summary>
        /// Path pointing to the directory where the soundbackends folder is located.
        /// </summary>
        public static string ResourcesFolder { get; private set; }

        /// <summary>
        /// Called when the list of sound devices returned by <see cref="GetCaptureDevices(string)"/> and <see cref="GetPlaybackDevices(string)"/> was changed
        /// </summary>
        public static event SoundDeviceListChangedEventHandler SoundDeviceListChanged;
        /// <summary>
        /// If user-defined logging was enabled when initializing the Client Lib by setting <see cref="LogTypes.Userlogging"/> in <see cref="LibraryParameters.UsedLogTypes"/>, log messages will be sent to the callback, which allows user customizable logging and handling
        /// </summary>
        public static event UserLoggingMessageEventHandler UserLogMessage;

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


        /// <summary>
        /// List of all currently existing server connections
        /// </summary>
        public static ICollection<Connection> Connections { get { return Api.GetServerConnectionHandlerList()?.AsReadOnly(); } }

        /// <summary>
        /// Query all available playback modes
        /// </summary>
        /// <returns>a readonly collection of playback modes.</returns>
        /// <seealso cref="GetCaptureModes"/>
        /// <seealso cref="GetPlaybackDevices(string)"/>
        public static ICollection<string> GetPlaybackModes() { return Api.GetPlaybackModeList()?.AsReadOnly(); }

        /// <summary>
        /// Query all available capture modes
        /// </summary>
        /// <returns>a readonly collection of capture modes.</returns>
        /// <seealso cref="GetPlaybackModes"/>
        /// <seealso cref="GetCaptureDevices(string)"/>
        public static ICollection<string> GetCaptureModes() { return Api.GetCaptureModeList()?.AsReadOnly(); }

        /// <summary>
        /// Get playback devices available for the given mode, as well as the current operating systems default. 
        /// </summary>
        /// <param name="mode">The playback mode to use. For different modes there might be different devices. Valid modes are returned by <see cref="GetPlaybackModes"/>.</param>
        /// <returns>a readonly collection of sound devices.</returns>
        /// <remarks>The returned devices values can be used to initialize the devices using <see cref="Connection.OpenPlayback(ISoundDevice)"/>.</remarks>
        /// <remarks>A list of available modes is returned by <see cref="GetPlaybackModes"/></remarks>
        /// <seealso cref="GetPlaybackModes"/>
        /// <seealso cref="GetCaptureDevices(string)"/>
        public static ICollection<SoundDevice> GetPlaybackDevices(string mode)
        {
            Require.NotNullOrEmpty(nameof(mode), mode);
            return Api.GetPlaybackDeviceList(mode)?.AsReadOnly();
        }

        /// <summary>
        /// Get capture devices available for the given mode, as well as the current operating systems default. 
        /// </summary>
        /// <param name="mode">The capture mode to use. For different modes there might be different devices. Valid modes are returned by <see cref="GetCaptureModes"/>.</param>
        /// <returns>a readonly collection of sound devices</returns>
        /// <remarks>The returned devices values can be used to initialize the devices using <see cref="Connection.OpenCapture(ISoundDevice)"/>.</remarks>
        /// <remarks>A list of available modes is returned by <see cref="GetCaptureModes"/></remarks>
        /// <seealso cref="GetCaptureModes"/>
        /// <seealso cref="GetPlaybackDevices(string)"/>
        public static ICollection<SoundDevice> GetCaptureDevices(string mode)
        {
            Require.NotNullOrEmpty(nameof(mode), mode);
            return Api.GetCaptureDeviceList(mode)?.AsReadOnly();
        }

        /// <summary>
        /// List of all currently existing custom devices
        /// </summary>
        public static ICollection<CustomDevice> CustomDevices { get { return CustomDevicesCache.AsReadOnly(); } }

        internal static Exception CreateException(Error error, string extraMessage = null)
        {
            string message;
            if (Api.TryGetErrorMessage(error, out message) != Error.Ok)
                message = error.ToString();
            TeamSpeakException result = new TeamSpeakException(error, message);
            if (extraMessage != null)
                result.Data.Add("extraMessage", extraMessage);
            return result;
        }

        internal static void AddServer(ulong id, Connection connection)
        {
            Connection old = ConnectionsCache.AddOrUpdate(id, connection, (_1, _2) => connection);
            Debug.Assert(object.ReferenceEquals(connection, old));
        }
        internal static Connection GetServer(ulong id)
        {
            return ConnectionsCache.GetOrAdd(id, _ => new Connection(id));
        }
        internal static void RemoveServer(ulong id)
        {
            Connection connection;
            ConnectionsCache.TryRemove(id, out connection);
        }

        /// <summary>
        /// To get the upload speed limit for all virtual servers in bytes/s
        /// </summary>
        public static ulong SpeedLimitUp
        {
            get { return Api.GetInstanceSpeedLimitUp(); }
            set { Api.SetInstanceSpeedLimitUp(value); }
        }

        /// <summary>
        /// To get the download speed limit for all virtual servers in bytes/s
        /// </summary>
        public static ulong SpeedLimitDown
        {
            get { return Api.GetInstanceSpeedLimitDown(); }
            set { Api.SetInstanceSpeedLimitDown(value); }
        }

        /// <summary>
        /// The complete Client Lib version string can be queried with
        /// </summary>
        public static string Version
        {
            get { return Api.GetClientLibVersion(); }
        }

        /// <summary>
        /// Version number, a part of the complete version string
        /// </summary>
        public static LibraryVersion VersionNumber
        {
            get { return Api.GetClientLibVersionNumber(); }
        }

        /// <summary>
        /// To connect to a server, a client application is required to request an identity from the Library. This string should be requested only once and then locally stored in the applications configuration. The next time the application connects to a server, the identity should be read from the configuration and reused again.
        /// </summary>
        /// <returns>a identity string</returns>
        public static string CreateIdentity()
        {
            return Api.CreateIdentity();
        }

        /// <summary>
        /// Generate the unique identifier of a identity
        /// </summary>
        /// <param name="identity">a identity string, that was created by <see cref="CreateIdentity"/></param>
        /// <returns>the unique identifier of the identity</returns>
        public static string IdentityToUniqueIdentifier(string identity)
        {
            Require.NotNullOrEmpty(nameof(identity), identity);
            return Api.IdentityStringToUniqueIdentifier(identity);
        }

        /// <summary>
        /// Before connecting to a TeamSpeak 3 server, a new <see cref="Connection"/> needs to be spawned. With a <see cref="Connection"/> a connection can be established and dropped multiple times, so for simply reconnecting to the same or another server no new <see cref="Connection"/> needs to be spawned but existing ones can be reused. However, for using multiple connections simultaneously a new <see cref="Connection"/> has to be spawned for each connection.
        /// </summary>
        /// <param name="port">Port the client should bind on. Default is 0 to let the operating system chose any free port.</param>
        /// <returns>a new <see cref="Connection"/>.</returns>
        public static Connection SpawnNewConnection(int port = 0)
        {
            return Api.SpawnNewServerConnectionHandler(port);
        }

        /// <summary>
        /// Instead of opening existing sound devices that TeamSpeak has detected, you can also use our custom capture and playback mechanism to allow you to override the way in which TeamSpeak does capture and playback. When you have opened a custom capture and playback device you must regularly supply new "captured" sound data via <see cref="CustomDevice.ProcessData(short[], int)"/> and retrieve data that should be "played back" via <see cref="CustomDevice.AcquireData(short[], int)"/>. Where exactly this captured sound data comes from and where the playback data goes to is up to you, which allows a lot of cool things to be done with this mechanism.
        /// A custom device can be opened like any standard device with <see cref="Connection.OpenCapture(ISoundDevice)"/> and <see cref="Connection.OpenPlayback(ISoundDevice)"/>.
        /// </summary>
        /// <param name="name">Displayed name of the custom device. Freely choose a name which identifies your device.</param>
        /// <param name="captureRate">Frequency of the capture device.</param>
        /// <param name="captureChannels">Number of channels of the capture device. This value depends on if the used codec is a mono or stereo codec.</param>
        /// <param name="playbackRate">Frequency of the playback device.</param>
        /// <param name="playbackChannels">Number of channels of the playback device.</param>
        /// <remarks> Implementing own custom devices is for special use cases and entirely optional.</remarks>
        /// <returns>a new <see cref="CustomDevice"/></returns>
        public static CustomDevice CreateCustomDevice(string name, SamplingRate captureRate = SamplingRate.Hz48000, int captureChannels = 1, SamplingRate playbackRate = SamplingRate.Hz48000, int playbackChannels = 1)
        {
            return new CustomDevice(name, captureRate, captureChannels, playbackRate, playbackChannels);
        }

        /// <summary>
        /// Basic logging function.
        /// </summary>
        /// <param name="severity">The level of the message.</param>
        /// <param name="message">Text written to log.</param>
        /// <param name="connection">Identify the current server connection when using multiple connections. Pass null if unused.</param>
        /// <param name="channel">Custom text to categorize the message channel (i.e. "Client", "Sound").</param>
        /// <remarks>
        /// Log messages can be printed to stdout, logged to a file logs/ts3client_[date]__[time].log and sent to user-defined callbacks. The log output behavior is defined when initializing the client library using <see cref="LibraryParameters"/>.
        /// Unless user-defined logging is used, program execution will halt on a log message with severity <see cref="LogLevel.Critical"/>
        /// </remarks>
        public static void Log(LogLevel severity, string message, Connection connection, string channel)
        {
            Require.NotNullOrEmpty(nameof(message), message);
            Api.LogMessage(message, severity, channel ?? string.Empty, connection?.ID ?? 0);
        }

        private static LogLevel _LogLevel = LogLevel.Devel;

        /// <summary>
        /// The minimum severity of log messages that are passed to <see cref="UserLogMessage"/>
        /// </summary>
        public static LogLevel LogLevel
        {
            get { return _LogLevel; }
            set
            {
                Api.SetLogVerbosity(value);
                _LogLevel = value;
            }
        }

        /// <summary>
        /// Gets a descriptive text message for a error-code
        /// </summary>
        /// <param name="error">the error-code</param>
        /// <returns>a descriptive text message</returns>
        public static string GetErrorMessage(Error error)
        {
            string result;
            error = Api.TryGetErrorMessage(error, out result);
            if (error != Error.Ok) throw CreateException(error);
            return result;
        }

        private static void ProcessExit(object sender, EventArgs e)
        {
            Destroy();
        }

        #region Events

        internal static void OnSoundDeviceListChanged(string modeID, bool playOrCap)
        {
            EventHelper.Run(SoundDeviceListChanged, _ => _(modeID, playOrCap));
        }

        internal static void OnUserLoggingMessage(string message, LogLevel level, string channel, Connection connection, string time, string completeString)
        {
            EventHelper.Run(UserLogMessage, _ => _(message, level, channel, connection, time, completeString));
        }

        internal static string OnClientPasswordEncrypt(Connection connection, string plaintext, int maxEncryptedTextByteSize)
        {
            return ClientPasswordEncrypt?.Invoke(connection, plaintext, maxEncryptedTextByteSize) ?? null;
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