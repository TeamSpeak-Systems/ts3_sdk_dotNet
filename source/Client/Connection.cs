using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TeamSpeak.Sdk.Client
{
    /// <summary>
    /// A connection to a TeamSpeak-Server
    /// </summary>
    public class Connection: IDisposable
    {
        /// <summary>
        /// ID of the client
        /// </summary>
        public ulong ID { get; }

        /// <summary>
        /// Sound Preprocessor Parameters
        /// </summary>
        public Preprocessor Preprocessor { get; }

        // TODO: improve summary
        /// <summary>
        /// the connection as a client object
        /// </summary>
        public Client Self { get; internal set; }

        /// <summary>
        /// A list of channels who have the channel as a parent
        /// </summary>
        public ReadonlyChannelCollection Channels { get { return ChannelTracker.GetChannels(0); } }

        /// <summary>
        /// A list of all channels on the virtual server
        /// </summary>
        public ReadonlyChannelCollection AllChannels { get { return new ReadonlyChannelCollection(Library.Api.GetChannelList(this)); } }

        /// <summary>
        /// A list of all currently visible clients on the virtual server
        /// </summary>
        public ICollection<Client> AllClients { get { return Library.Api.GetClientList(this)?.AsReadOnly(); } }

        /// <summary>
        /// After calling <see cref="O:TeamSpeak.Sdk.Client.Connection.Start"/> the client will be informed of the connection status changes by the event
        /// </summary>
        public event ConnectStatusChangeEventHandler StatusChanged;

        /// <summary>
        /// While connecting to a server, the protocol version is reported.
        /// </summary>
        public event ProtocolVersionEventHandler ProtocolVersionReceived;

        /// <summary>
        /// Informs about the existence of each channel, after connection has been established, all current channels on the server are announced. This happens with delays to avoid a flood of information after connecting.
        /// </summary>
        public event NewChannelEventHandler NewChannel;

        /// <summary>
        /// After on successfully creating a new Channel the event will be called
        /// </summary>
        public event NewChannelCreatedEventHandler NewChannelCreated;

        /// <summary>
        /// After deleting a Channel the event will be called.
        /// </summary>
        public event ChannelDeletedEventHandler ChannelDeleted;

        /// <summary>
        /// Called when a channel is being moved.
        /// </summary>
        public event ChannelMoveEventHandler ChannelMoved;

        /// <summary>
        /// Called when a channel was changed
        /// </summary>
        public event ChannelChangedEventHandler ChannelChanged;

        /// <summary>
        /// Called when a client was changed
        /// </summary>
        public event UpdateClientEventHandler ClientUpdated;

        /// <summary>
        /// Called when a client is actively switching channels.
        /// </summary>
        public event ClientMoveEventHandler ClientMoved;

        /// <summary>
        /// Once a channel has been subscribed or unsubscribed, the event is called for each client in the subscribed channel. The event is not to be confused with <see cref="ClientMoved"/>, which is called for clients actively switching channels.
        /// </summary>
        public event ClientMoveSubscriptionEventHandler ClientMovedSubscription;

        /// <summary>
        /// When a clients is moved because of a timeout
        /// </summary>
        public event ClientMoveTimeoutEventHandler ClientTimeout;

        /// <summary>
        /// When a client is kicked from a channel
        /// </summary>
        public event ClientKickFromChannelEventHandler ClientKickedFromChannel;

        /// <summary>
        /// When a client is kicked from the server
        /// </summary>
        public event ClientKickFromServerEventHandler ClientKickedFromServer;

        /// <summary>
        /// Called when the virtual server was changed
        /// </summary>
        public event ServerUpdatedEventHandler ServerUpdated;

        /// <summary>
        /// Error codes sent by the server to the client
        /// </summary>
        public event ServerErrorEventHandler ServerError;

        /// <summary>
        /// Called when the server has been shutdown
        /// </summary>
        public event ServerStopEventHandler ServerStop;

        /// <summary>
        /// Called when a private text message from a client was received
        /// </summary>
        public event ClientMessageEventHandler ClientMessage;

        /// <summary>
        /// Called when a channel message was received
        /// </summary>
        public event ChannelMessageEventHandler ChannelMessage;

        /// <summary>
        /// Called when a server message was received
        /// </summary>
        public event ServerMessageEventHandler ServerMessage;

        /// <summary>
        /// When a client starts or stops talking
        /// </summary>
        public event TalkStatusChangeEventHandler TalkStatusChanged;

        /// <summary>
        /// Used when whisper is received from a <see cref="Client"/> that has not been added to the whisper allow list.
        /// </summary>
        /// <remarks>Note that whisper voice data is not received until the sending client is added to the receivers whisper allow list using <see cref="AllowWhispersFrom(Client)"/>.</remarks>
        public event IgnoredWhisperEventHandler WhisperIgnored;

        /// <summary>
        /// Called when channel has been subscribed
        /// </summary>
        public event ChannelSubscribeEventHandler ChannelSubscribed;

        /// <summary>
        /// Marks the end of multiple calls to <see cref="ChannelSubscribed"/> 
        /// </summary>
        public event ChannelSubscribeFinishedEventHandler ChannelSubscribesFinished;

        /// <summary>
        /// Called when channel has been unsubscribed
        /// </summary>
        public event ChannelUnsubscribeEventHandler ChannelUnsubscribed;

        /// <summary>
        /// Marks the end of multiple calls to <see cref="ChannelUnsubscribed"/> 
        /// </summary>
        public event ChannelUnsubscribeFinishedEventHandler ChannelUnsubscribesFinished;

        /// <summary>
        /// Called when the <see cref="Channel.Description"/> was edited
        /// </summary>
        public event ChannelDescriptionUpdateEventHandler ChannelDescriptionUpdated;

        /// <summary>
        /// Called when a <see cref="Channel.Password"/> was modified.
        /// </summary>
        /// <remarks>previously entered channel passwords might be remembered, so this callback announces that the stored password are invalid.</remarks>
        public event ChannelPasswordChangedEventHandler ChannelPasswordChanged;

        /// <summary>
        /// Called after <see cref="InitiateGracefulPlaybackShutdown"/> finished for a device
        /// </summary>
        public event PlaybackShutdownCompleteEventHandler PlaybackShutdownCompleted;

        /// <summary>
        /// Called when a incoming voice packet from a remote client was decoded and is about to be played, before it is 3D positioned and mixed with other sound sources.
        /// The event can be used to alter the voice data (for example when you want to do effects on it) or to record the voice data.
        /// </summary>
        /// <remarks>This is used by the TeamSpeak client to record sessions.</remarks>
        public event EditPlaybackVoiceDataEventHandler EditPlaybackVoiceData;

        /// <summary>
        /// Called when a incoming voice packet from a remote client was decoded and 3D positioned and is about to be played, but before it is mixed with other sounds sources.
        /// The event can be used to alter or get the voice data after it has been 3D positioned.
        /// </summary>
        public event EditPostProcessVoiceDataEventHandler EditPostProcessVoiceData;

        /// <summary>
        /// The following event is called when all sounds that are about to be played back for this server connection have been mixed. This is the last chance to alter/get sound.
        /// The event can be used to alter or get the sound data before playback.
        /// </summary>
        public event EditMixedPlaybackVoiceDataEventHandler EditMixedPlaybackVoiceData;

        /// <summary>
        /// Called after sound is recorded from the sound device and is preprocessed. This event can be used to get/alter recorded sound. 
        /// It can also be used to determine if this sound will be send, or discarded.
        /// </summary>
        /// <remarks>This is used by the TeamSpeak client to record sessions.</remarks>
        public event EditCapturedVoiceDataEventHandler EditCapturedVoiceData;

        /// <summary>
        /// Called to calculate volume attenuation for distance in 3D positioning of clients.
        /// </summary>
        public event Custom3dRolloffCalculationClientEventHandler Custom3dRolloffCalculationClient;

        /// <summary>
        /// Called to calculate volume attenuation for distance in 3D positioning of a wave file that was opened with <see cref="PlayWaveFile(string, bool)"/>.
        /// </summary>
        public event Custom3dRolloffCalculationWaveEventHandler Custom3dRolloffCalculationWaveReceived;

        /// <summary>
        /// Used to check if the unique identifier is the correct one
        /// </summary>
        /// <remarks>to be used, if a man in the middle attack is be expected</remarks>
        public event CheckServerUniqueIdentifierEventHandler CheckServerUniqueIdentifier;

        /// <summary>
        /// Called when a file finished being transfered, triggered by <see cref="O:TeamSpeak.Sdk.Client.Channel.SendFile"/>  or <see cref="O:TeamSpeak.Sdk.Client.Channel.RequestFile"/> has finished or aborted with an error.
        /// </summary>
        public event FileTransferStatusEventHandler FileTransferStatusReceived;

        /// <summary>
        /// Called after <see cref="Channel.GetFileInfo(string, string)"/> containing the reply by the server
        /// </summary>
        public event FileInfoEventHandler FileInfoReceived;

        /// <summary>
        /// Unique ID for this virtual server. Stays the same after restarting the server application. Always available when connected.
        /// </summary>
        public string UniqueIdentifier
        {
            get { return GetString(VirtualServerProperty.UniqueIdentifier); }
        }
        /// <summary>
        /// Name of this virtual server. Always available when connected.
        /// </summary>
        public string Name
        {
            get { return GetString(VirtualServerProperty.Name); }
        }
        /// <summary>
        /// Optional welcome message sent to the client on login. This value should be queried by the client after connection has been established, it is not updated afterwards.
        /// </summary>
        public string WelcomeMessage
        {
            get { return GetString(VirtualServerProperty.Welcomemessage); }
        }
        /// <summary>
        /// Operating system used by this server. Always available when connected.
        /// </summary>
        public string Platform
        {
            get { return GetString(VirtualServerProperty.Platform); }
        }
        /// <summary>
        /// Application version of this server. Always available when connected.
        /// </summary>
        public string Version
        {
            get { return GetString(VirtualServerProperty.Version); }
        }
        /// <summary>
        /// Defines maximum number of clients which may connect to this server. Needs to be requested using <see cref="RefreshVariables"/>.
        /// </summary>
        public int MaxClients
        {
            get { return GetInt(VirtualServerProperty.Maxclients); }
        }
        /// <summary>
        /// Number of clients currently on this virtual server. Needs to be requested using <see cref="RefreshVariables"/>.
        /// </summary>
        public int ClientsOnline
        {
            get { return GetInt(VirtualServerProperty.ClientsOnline); }
        }
        /// <summary>
        /// Number of channels currently on this virtual server. Needs to be requested using <see cref="RefreshVariables"/>.
        /// </summary>
        public int ChannelsOnline
        {
            get { return GetInt(VirtualServerProperty.ChannelsOnline); }
        }
        /// <summary>
        /// Time when this virtual server was created. Always available when connected.
        /// </summary>
        public DateTimeOffset Created
        {
            get { return Native.FromUnixTime(GetUInt64(VirtualServerProperty.Created)); }
        }
        /// <summary>
        /// Uptime of this virtual server. Needs to be requested using <see cref="RefreshVariables"/>.
        /// </summary>
        public TimeSpan Uptime
        {
            get { return TimeSpan.FromSeconds(GetInt(VirtualServerProperty.Uptime)); }
        }
        /// <summary>
        /// Defines if voice data encryption is configured per channel, globally forced on or globally forced off for this virtual server. 
        /// The default behavior is configure per channel, in this case modifying <see cref="Channel.CodecIsUnencrypted"/>
        /// defines voice data encryption of individual channels.
        /// </summary>
        public CodecEncryptionMode CodecEncryptionMode
        {
            get { return (CodecEncryptionMode)GetInt(VirtualServerProperty.CodecEncryptionMode); }
        }

        /// <summary>
        /// IP of the server (seen from the client side)
        /// </summary>
        public string ServerIp
        {
            get { return GetString(ConnectionProperty.ServerIp); }
        }

        /// <summary>
        /// Port of the server (seen from the client side)
        /// </summary>
        public ulong ServerPort
        {
            get { return GetUInt64(ConnectionProperty.ServerPort); }
        }

        private bool _IsVoiceRecording = false;
        /// <summary>
        /// When using <see cref="EditCapturedVoiceData"/> to record voice, you should notify the server when recording starts or stops
        /// </summary>
        public bool IsVoiceRecording
        {
            get { return _IsVoiceRecording; }
            set
            {
                if (value) Library.Api.StartVoiceRecording(this);
                else Library.Api.StopVoiceRecording(this);
                _IsVoiceRecording = value;
            }
        }

        private readonly Channel ZeroChannel;
        private readonly Client ZeroClient;
        private readonly AutoResetEvent ZeroChannelGuard = new AutoResetEvent(true);
        internal readonly ConnectionCaches Cache;
        internal readonly ChannelTracker ChannelTracker;

        /// <summary>
        /// Spawns a new <see cref="Connection"/>
        /// </summary>
        public Connection()
            : this(Library.Api.RawSpawnNewServerConnectionHandler(0))
        {
            Library.AddServer(ID, this);
        }

        /// <summary>
        /// Creates a new <see cref="Connection"/>-Object with a already known id 
        /// </summary>
        /// <param name="id">Id of the connection</param>
        /// <remarks>does not spawns a new <see cref="Connection"/>, use <see cref="Connection()"/> or <see cref="Library.SpawnNewConnection(int)"/> to spawn a new <see cref="Connection"/></remarks>
        internal Connection(ulong id)
        {
            ID = id;
            Cache = new ConnectionCaches(this);
            ChannelTracker = new ChannelTracker();
            Preprocessor = new Preprocessor(this);
            ZeroChannel = new Channel(this, 0);
            ZeroClient = new Client(this, 0);
            Self = ZeroClient;
        }

        /// <summary>
        /// Allows Connection to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.
        /// </summary>
        ~Connection()
        {
            Dispose(false);
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the CustomDevice and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposed">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposed)
        {
            if (disposed)
            {
                // No point in knowing if it failed, after all we can't do anything about it
                Library.Api.TryDestroyServerConnectionHandler(this);
                Library.RemoveServer(ID);
            }
        }

        /// <summary>
        /// Closes the connection
        /// </summary>
        public void Close()
        {
            ((IDisposable)this).Dispose();
        }

        /// <summary>
        /// Currently open playback device. Set using <see cref="OpenPlayback(ISoundDevice)"/>
        /// </summary>
        public ISoundDevice PlaybackDevice { get; private set; }

        /// <summary>
        /// Currently open capture device. Set using <see cref="OpenCapture(ISoundDevice)"/>
        /// </summary>
        public ISoundDevice CaptureDevice { get; private set; }

        /// <summary>
        /// Status of the connection to the given server
        /// </summary>
        public ConnectStatus Status { get { return Library.Api.GetConnectionStatus(this); } }

        /// <summary>
        /// The upload speed limit for the specified virtual server in bytes/s.
        /// </summary>
        public ulong SpeedLimitUp
        {
            get { return Library.Api.GetServerConnectionHandlerSpeedLimitUp(this); }
            set { Library.Api.SetServerConnectionHandlerSpeedLimitUp(this, value); }
        }

        /// <summary>
        ///  The download speed limit for the specified virtual server in bytes/s.
        /// </summary>
        public ulong SpeedLimitDown
        {
            get { return Library.Api.GetServerConnectionHandlerSpeedLimitDown(this); }
            set { Library.Api.SetServerConnectionHandlerSpeedLimitDown(this, value); }
        }

        private LocalTestMode _LocalTestMode = LocalTestMode.Off;
        /// <summary>
        /// Instead of sending the sound through the network, it can be routed directly through the playback device, so the user will get immediate audible feedback when for example configuring some sound settings.
        /// </summary>
        public LocalTestMode LocalTestMode
        {
            get { return _LocalTestMode; }
            set
            {
                Library.Api.SetLocalTestMode(this, value);
                _LocalTestMode = value;
            }
        }


        /// <summary>
        /// Modify the voice volume of other speakers. Value is in decibel, so 0 is no modification, 
        /// negative values make the signal quieter and values greater than zero boost the signal louder than it is.
        /// Be careful with high positive values, as you can really cause bad audio quality due to clipping.
        /// The maximum possible Value is 30. Zero and all negative values cannot cause clipping and distortion, 
        /// and are preferred for optimal audio quality. Values greater than zero and less than +6 dB 
        /// might cause moderate clipping and distortion, but should still be within acceptable bounds.
        /// Values greater than +6 dB will cause clipping and distortion that will negatively affect your audio quality.
        /// It is advised to choose lower values. Generally we recommend to not allow values higher than 15 db.
        /// </summary>
        public float VolumeModifier
        {
            get { return GetPlaybackConfigValueAsFloat("volume_modifier"); }
            set { SetPlaybackConfigValue("volume_modifier", value.ToString(CultureInfo.InvariantCulture)); }
        }

        /// <summary>
        /// Adjust the volume of wave files played by <see cref="PlayWaveFile(string, bool)"/>.
        /// The value is a float defining the volume reduction in decibel.
        /// Reasonable values range from �-40.0� (very silent) to �0.0� (loudest).
        /// </summary>
        public float VolumeFactorWave
        {
            get { return GetPlaybackConfigValueAsFloat("volume_modifier"); }
            set { SetPlaybackConfigValue("volume_modifier", value.ToString(CultureInfo.InvariantCulture)); }
        }

        /// <summary>
        /// To initialize a playback device
        /// </summary>
        /// <param name="device">the device to be used. when null, the default playback is used.</param>
        public void OpenPlayback(ISoundDevice device = null)
        {
            if (device != null)
            {
                Library.Api.OpenPlaybackDevice(this, device.Mode, device.ID);
                PlaybackDevice = device;
            }
            else
            {
                Library.Api.OpenPlaybackDevice(this, string.Empty, string.Empty);
                string mode = Library.Api.GetCurrentPlayBackMode(this);
                string id;
                bool isDefault;
                Library.Api.GetCurrentPlaybackDeviceName(this, out id, out isDefault);
                IEnumerable<ISoundDevice> soundDeviceSource = mode != CustomDevice.ModeName
                                                            ? Library.GetPlaybackDevices(mode).Cast<ISoundDevice>()
                                                            : Library.CustomDevices;
                PlaybackDevice = soundDeviceSource.FirstOrDefault(d => d.ID == id) ?? new SoundDevice(mode, id, null); ;
            }
        }

        /// <summary>
        /// To initialize a capture device
        /// </summary>
        /// <param name="device">the device to be used. when null, the default capture is used.</param>
        public void OpenCapture(ISoundDevice device = null)
        {
            if (device != null)
            {
                Library.Api.OpenCaptureDevice(this, device.Mode, device.ID);
                CaptureDevice = device;
            }
            else
            {
                Library.Api.OpenCaptureDevice(this, string.Empty, string.Empty);
                string mode = Library.Api.GetCurrentCaptureMode(this);
                string id;
                bool isDefault;
                Library.Api.GetCurrentCaptureDeviceName(this, out id, out isDefault);
                IEnumerable<ISoundDevice> soundDeviceSource = mode != CustomDevice.ModeName
                                                            ? Library.GetCaptureDevices(mode).Cast<ISoundDevice>()
                                                            : Library.CustomDevices;
                CaptureDevice = soundDeviceSource.FirstOrDefault(d => d.ID == id) ?? new SoundDevice(mode, id, null);

            }
        }

        /// <summary>
        /// Prepares the playback device be closed via <see cref="ClosePlaybackDevice"/>. 
        /// Should be used to prevent interrupting of still playing sounds.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task InitiateGracefulPlaybackShutdown()
        {
            Task task;
            string returnCode = GetNextReturnCode(out task);
            PlaybackShutdownCompleteEventHandler eventHandler = null;
            eventHandler = _ => { lock (eventHandler) SetReturnCodeResult(returnCode, Error.Ok, null); PlaybackShutdownCompleted -= eventHandler; };
            PlaybackShutdownCompleted += eventHandler;
            Error error = Library.Api.TryInitiateGracefulPlaybackShutdown(this);
            if (error != Error.Ok)
            {
                PlaybackShutdownCompleted -= eventHandler;
                if (error != Error.Ok)
                {
                    error = error == Error.OkNoUpdate ? Error.Ok : error;
                    SetReturnCodeResult(returnCode, error, null);
                }
            }
            return task;
        }

        /// <summary>
        /// Close the playback device
        /// </summary>
        public void ClosePlaybackDevice()
        {
            Library.Api.ClosePlaybackDevice(this);
        }

        /// <summary>
        /// Close the capture device
        /// </summary>
        public void CloseCaptureDevice()
        {
            Library.Api.CloseCaptureDevice(this);
        }

        /// <summary>
        /// When connecting to multiple servers with the same client, the capture device can only be active for one server at the same time. 
        /// As soon as the client connects to a new server, the Library will deactivate the capture device for the previously active server.
        /// When a user wants to talk to that previous server again, the client needs to reactivate the capture device.
        /// </summary>
        public void ActivateCaptureDevice()
        {
            Library.Api.ActivateCaptureDevice(this);
        }

        /// <summary>
        /// Set the position, velocity and orientation of the listener in 3D space
        /// </summary>
        /// <param name="position">3D position of the listener. If passing null, the parameter is ignored and the value not updated.</param>
        /// <param name="forward">Forward orientation of the listener. The vector must be of unit length and perpendicular to the up vector. If passing null, the parameter is ignored and the value not updated.</param>
        /// <param name="up">Upward orientation of the listener. The vector must be of unit length and perpendicular to the forward vector. If passing null, the parameter is ignored and the value not updated.</param>
        public void Set3DListenerAttributes(Vector? position, Vector? forward, Vector? up)
        {
            Library.Api.Systemset3DListenerAttributes(this, position, forward, up);
        }

        /// <summary>
        /// Adjusts 3D sound system settings
        /// </summary>
        /// <param name="distanceFactor">Relative distance factor. Default is 1.0 = 1 meter</param>
        /// <param name="rolloffScale">Scaling factor for 3D sound roll-off. Defines how fast sound volume will attenuate. As higher the value, as faster the sound is toned with increasing distance.</param>
        public void Set3DSettings(float distanceFactor, float rolloffScale)
        {
            Library.Api.Systemset3DSettings(this, distanceFactor, rolloffScale);
        }

        #region Start/Stop

        private TaskCompletionSource<Error> StartOrStopTaskCompletionSource = null;
        private volatile bool IsConnecting;
        private void PassStartOrStop()
        {
            TaskCompletionSource<Error> tcs = StartOrStopTaskCompletionSource;
            if (tcs != null && Interlocked.CompareExchange(ref StartOrStopTaskCompletionSource, null, tcs) == tcs)
            {
                Task.Factory.StartNew(o => ((TaskCompletionSource<Error>)o).SetResult(Error.Ok), tcs);
            }
        }
        private void FailStartOrStop(Error error, string extraMessage)
        {
            TaskCompletionSource<Error> tcs = StartOrStopTaskCompletionSource;
            if (tcs != null && Interlocked.CompareExchange(ref StartOrStopTaskCompletionSource, null, tcs) == tcs)
            {
                Task.Factory.StartNew(() => tcs.SetException(Library.CreateException(error, extraMessage)));
            }
        }

        /// <summary>
        /// Connect to a TeamSpeak 3 server
        /// </summary>
        /// <param name="identity">Unique identifier for this server connection. Created with <see cref="Library.CreateIdentity"/></param>
        /// <param name="ip">Hostname or IP of the TeamSpeak 3 server.</param>
        /// <param name="port">UDP port of the TeamSpeak 3 server, by default 9987.</param>
        /// <param name="nickname">On login, the client attempts to take this nickname on the connected server. Note this is not necessarily the actually assigned nickname, as the server can modify the nickname ("gandalf_1" instead the requested "gandalf") or refuse blocked names.</param>
        /// <param name="defaultChannel">A channel on the TeamSpeak 3 server. If the channel exists and the user has sufficient rights and supplies the correct password if required, the channel will be joined on login.</param>
        /// <param name="defaultChannelPassword">Password for the default channel. Pass null or an empty string if no password is required or no default channel is specified.</param>
        /// <param name="serverPassword">Password for the server. Pass null or an empty string if the server does not require a password.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks> If you pass a hostname instead of an IP, the Client Lib will try to resolve it to an IP, but the function may block for an unusually long period of time while resolving is taking place. If you are relying on the function to return quickly, we recommend to resolve the hostname yourself (e.g.asynchronously) and then call with the IP instead of the hostname.</remarks>
        public Task Start(string identity, string ip, uint port, string nickname, string defaultChannel = null, string defaultChannelPassword = null, string serverPassword = null)
        {
            string[] defaultChannelArray = defaultChannel == null ? null : new string[] { defaultChannel };
            return Start(identity, ip, port, nickname, defaultChannelArray, defaultChannelPassword, serverPassword);
        }
        /// <summary>
        /// Connect to a TeamSpeak 3 server
        /// </summary>
        /// <param name="identity">Unique identifier for this server connection. Created with <see cref="Library.CreateIdentity"/></param>
        /// <param name="ip">Hostname or IP of the TeamSpeak 3 server.</param>
        /// <param name="port">UDP port of the TeamSpeak 3 server, by default 9987.</param>
        /// <param name="nickname">On login, the client attempts to take this nickname on the connected server. Note this is not necessarily the actually assigned nickname, as the server can modify the nickname ("gandalf_1" instead the requested "gandalf") or refuse blocked names.</param>
        /// <param name="defaultChannelArray">The path to a channel on the TeamSpeak 3 server. If the channel exists and the user has sufficient rights and supplies the correct password if required, the channel will be joined on login. To define the path to a subchannel of arbitrary level, create an array of channel names detailing the position of the default channel (e.g. { "grandparent", "parent", "mydefault" } ). </param>
        /// <param name="defaultChannelPassword">Password for the default channel. Pass null or an empty string if no password is required or no default channel is specified.</param>
        /// <param name="serverPassword">Password for the server. Pass null or an empty string if the server does not require a password.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks> If you pass a hostname instead of an IP, the Client Lib will try to resolve it to an IP, but the function may block for an unusually long period of time while resolving is taking place. If you are relying on the function to return quickly, we recommend to resolve the hostname yourself (e.g.asynchronously) and then call with the IP instead of the hostname.</remarks>
        public Task Start(string identity, string ip, uint port, string nickname, string[] defaultChannelArray, string defaultChannelPassword = null, string serverPassword = null)
        {
            TaskCompletionSource<Error> taskCompletion = new TaskCompletionSource<Error>();
            if (Interlocked.CompareExchange(ref StartOrStopTaskCompletionSource, taskCompletion, null) != null)
                throw new InvalidOperationException();

            IsConnecting = true;
            ChannelTracker.Reset();
            Error error = Library.Api.TryStartConnection(this, identity, ip, port, nickname, defaultChannelArray, defaultChannelPassword, serverPassword);
            if (error != Error.Ok)
                FailStartOrStop(error, null);
            return taskCompletion.Task;
        }
        /// <summary>
        /// Connect to a TeamSpeak 3 server
        /// </summary>
        /// <param name="identity">Unique identifier for this server connection. Created with <see cref="Library.CreateIdentity"/></param>
        /// <param name="ip">Hostname or IP of the TeamSpeak 3 server.</param>
        /// <param name="port">UDP port of the TeamSpeak 3 server, by default 9987.</param>
        /// <param name="nickname">On login, the client attempts to take this nickname on the connected server. Note this is not necessarily the actually assigned nickname, as the server can modify the nickname ("gandalf_1" instead the requested "gandalf") or refuse blocked names.</param>
        /// <param name="defaultChannelID">The <see cref="Channel.ID"/> to a channel on the TeamSpeak 3 server. If the channel exists and the user has sufficient rights and supplies the correct password if required, the channel will be joined on login.</param>
        /// <param name="defaultChannelPassword">Password for the default channel. Pass null or an empty string if no password is required or no default channel is specified.</param>
        /// <param name="serverPassword">Password for the server. Pass null or an empty string if the server does not require a password.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks> If you pass a hostname instead of an IP, the Client Lib will try to resolve it to an IP, but the function may block for an unusually long period of time while resolving is taking place. If you are relying on the function to return quickly, we recommend to resolve the hostname yourself (e.g.asynchronously) and then call with the IP instead of the hostname.</remarks>
        public Task Start(string identity, string ip, uint port, string nickname, ulong defaultChannelID, string defaultChannelPassword = null, string serverPassword = null)
        {
            TaskCompletionSource<Error> taskCompletion = new TaskCompletionSource<Error>();
            if (Interlocked.CompareExchange(ref StartOrStopTaskCompletionSource, taskCompletion, null) != null)
                throw new InvalidOperationException();

            IsConnecting = true;
            ChannelTracker.Reset();
            Error error = Library.Api.TryStartConnection(this, identity, ip, port, nickname, defaultChannelID, defaultChannelPassword, serverPassword);
            if (error != Error.Ok)
                FailStartOrStop(error, null);
            return taskCompletion.Task;
        }

        /// <summary>
        /// Disconnect from a TeamSpeak 3 server
        /// </summary>
        /// <param name="quitMessage">A message like for example "leaving".</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task Stop(string quitMessage = null)
        {
            TaskCompletionSource<Error> taskCompletion = new TaskCompletionSource<Error>();
            if (Status != ConnectStatus.Disconnected)
            {
                if (Interlocked.CompareExchange(ref StartOrStopTaskCompletionSource, taskCompletion, null) != null)
                    throw new InvalidOperationException();

                IsConnecting = false;
                Error error = Library.Api.TryStopConnection(this, quitMessage);
                if (error != Error.Ok)
                    FailStartOrStop(error, null);
            }
            else
            {
                Error error = Library.Api.TryStopConnection(this, quitMessage);
                if (error == Error.Ok) taskCompletion.SetResult(0);
                else taskCompletion.SetException(Library.CreateException(error));
            }
            return taskCompletion.Task;
        }

        #endregion Start/Stop

        /// <summary>
        /// Subscribe to all channels on the server
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task SubscribeAll()
        {
            Task task;
            Library.Api.RequestChannelSubscribeAll(this, GetNextReturnCode(out task));
            return task;
        }

        /// <summary>
        /// Unsubscribe from all channels on the server
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task UnsubscribeAll()
        {
            Task task;
            Library.Api.RequestChannelUnsubscribeAll(this, GetNextReturnCode(out task));
            return task;
        }

        /// <summary>
        /// Refreshing the server information.
        /// </summary>
        /// <remarks><see cref="ServerUpdated"/> is called when the information is available</remarks>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task RefreshVariables()
        {
            Task task;
            string returnCode = GetNextReturnCode(out task);
            ServerUpdatedEventHandler serverUpdated = null;
            serverUpdated = (server, editor) =>
            {
                if (server == this && editor == null)
                {
                    SetReturnCodeResult(returnCode, Error.Ok, null);
                    ServerUpdated -= serverUpdated;
                }
            };
            ServerUpdated += serverUpdated;
            Library.Api.RequestServerVariables(this);
            return task;
        }

        /// <summary>
        /// Subscribes to one or more channels
        /// </summary>
        /// <param name="channels">Array of channels to subscribe to.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task Subscribe(params Channel[] channels)
        {
            Require.NotNull(nameof(channels), channels);
            Require.EntriesNotNull(nameof(channels), channels);
            Require.SameConnection(nameof(channels), this, channels);
            Task task;
            Library.Api.RequestChannelSubscribe(channels, GetNextReturnCode(out task));
            return task;
        }
        /// <summary>
        /// Unsubscribes from one or more channels
        /// </summary>
        /// <param name="channels">Array of channels to unsubscribe from.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task Unsubscribe(params Channel[] channels)
        {
            Require.NotNull(nameof(channels), channels);
            Require.EntriesNotNull(nameof(channels), channels);
            Require.SameConnection(nameof(channels), this, channels);
            Task task;
            Library.Api.RequestChannelUnsubscribe(channels, GetNextReturnCode(out task));
            return task;
        }

        /// <summary>
        /// Mutes one or more clients
        /// </summary>
        /// <param name="clients">Array of clients to be muted</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// Clients can be locally muted. This information is handled client-side only and not visible to other clients. It mainly serves as a sort of individual "ban" or "ignore" feature, where users can decide not to listen to certain clients anymore.
        /// When a client becomes muted, he will no longer be heard by the muter. Also the TeamSpeak 3 server will stop sending voice packets.
        /// The mute state is not visible to the muted client nor to other clients. It is only available to the muting client by checking <see cref="Client.Muted"/>
        /// </remarks>
        public Task Mute(params Client[] clients)
        {
            Require.NotNull(nameof(clients), clients);
            Require.EntriesNotNull(nameof(clients), clients);
            Require.SameConnection(nameof(clients), this, clients);
            Task task;
            Library.Api.RequestMuteClients(clients, GetNextReturnCode(out task));
            return task;
        }

        /// <summary>
        /// Unmute one or more clients
        /// </summary>
        /// <param name="clients">Array of clients to be muted</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task Unmute(params Client[] clients)
        {
            Require.NotNull(nameof(clients), clients);
            Require.EntriesNotNull(nameof(clients), clients);
            Require.SameConnection(nameof(clients), this, clients);
            Task task;
            Library.Api.RequestUnmuteClients(clients, GetNextReturnCode(out task));
            return task;
        }

        /// <summary>
        /// Create a new Channel
        /// </summary>
        /// <param name="name">Name of the channel</param>
        /// <param name="parent">The parent channel</param>
        /// <param name="topic">Single-line channel topic</param>
        /// <param name="description">Channel description. Can have multiple lines.</param>
        /// <param name="password">Password for password-protected channels.</param>
        /// <param name="order">the <see cref="Channel"/> after which the new <see cref="Channel"/> is sorted. <see langword="null"/> meaning its going to be the first <see cref="Channel"/> under <paramref name="parent"/>.</param>
        /// <param name="isPermanent">Permanent channels will be restored when the server restarts.</param>
        /// <param name="isSemiPermanent">Semi-permanent channels are not automatically deleted when the last user left but will not be restored when the server restarts.</param>
        /// <param name="codec">Codec used for this channel</param>
        /// <param name="codecQuality">Quality of channel codec of this channel. Valid values range from 0 to 10, default is 7. Higher values result in better speech quality but more bandwidth usage</param>
        /// <param name="codecIsUnencrypted">If true, this channel is not using encrypted voice data. If false, voice data is encrypted for this channel.</param>
        /// <param name="codecLatencyFactor">Latency of this channel.</param>
        /// <returns>A task that represents the asynchronous creation of the channel.</returns>
        public Task<Channel> CreateChannel(string name, Channel parent, string topic = null, string description = null, string password = null, Channel order = null,
            bool? isPermanent = null, bool? isSemiPermanent = null,
            CodecType? codec = null, int? codecQuality = null, bool? codecIsUnencrypted = null, int? codecLatencyFactor = null)
        {
            Require.NotNull(nameof(name), name);
            Require.SameConnection(nameof(parent), this, parent);

            TaskCompletionSource<Channel> taskCompletionSource = new TaskCompletionSource<Channel>();

            NewChannelCreatedEventHandler newChannelCreated = null;
            newChannelCreated = (channel, invoker) =>
            {
                if (invoker == Self)
                {
                    NewChannelCreated -= newChannelCreated;
                    ZeroChannelGuard.Set();
                    taskCompletionSource.SetResult(channel);
                }
            };

            Action<Task> onChannelCreationFailed = t =>
            {
                NewChannelCreated -= newChannelCreated;
                ZeroChannelGuard.Set();
                taskCompletionSource.SetException(t.Exception);
            };

            WaitOrTimerCallback mainBody = (state, timeout) =>
            {
                ZeroChannel.Name = name;
                if (topic != null) ZeroChannel.Topic = topic;
                if (description != null) ZeroChannel.Description = description;
                if (password != null) ZeroChannel.Password = password;
                ZeroChannel.Order = order;
                if (isPermanent != null) ZeroChannel.IsPermanent = isPermanent.Value;
                if (isSemiPermanent != null) ZeroChannel.IsSemiPermanent = isSemiPermanent.Value;
                if (codecQuality != null) ZeroChannel.CodecQuality = codecQuality.Value;
                if (codecIsUnencrypted != null) ZeroChannel.CodecIsUnencrypted = codecIsUnencrypted.Value;
                if (codecLatencyFactor != null) ZeroChannel.CodecLatencyFactor = codecLatencyFactor.Value;

                NewChannelCreated += newChannelCreated;
                Task flushTask;
                string returnCode = GetNextReturnCode(out flushTask);
                Library.Api.TryFlushChannelCreation(this, parent, returnCode);
                flushTask.ContinueWith(onChannelCreationFailed, TaskContinuationOptions.OnlyOnFaulted);
            };
            if (ZeroChannelGuard.WaitOne(0))
                mainBody(null, false);
            else
                ThreadPool.RegisterWaitForSingleObject(ZeroChannelGuard, mainBody, null, Timeout.Infinite, true);
            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Send a private text message to a client
        /// </summary>
        /// <param name="client">The target client.</param>
        /// <param name="message">The text message.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task SendTextMessage(Client client, string message)
        {
            Require.NotNull(nameof(client), client);
            Require.SameConnection(nameof(client), this, client);
            Require.NotNull(nameof(message), message);
            return client.SendTextMessage(message);
        }

        /// <summary>
        /// Send a text message to a channel
        /// </summary>
        /// <param name="channel">The target channel.</param>
        /// <param name="message">The text message.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task SendTextMessage(Channel channel, string message)
        {
            Require.NotNull(nameof(channel), channel);
            Require.SameConnection(nameof(channel), this, channel);
            Require.NotNull(nameof(message), message);
            return channel.SendTextMessage(message);
        }

        /// <summary>
        /// Send a text message to the server
        /// </summary>
        /// <param name="message">The text message.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task SendTextMessage(string message)
        {
            Require.NotNull(nameof(message), message);
            Task task;
            Library.Api.RequestSendServerTextMsg(this, message, GetNextReturnCode(out task));
            return task;
        }

        /// <summary>
        /// Move a client to a channel.
        /// </summary>
        /// <param name="client">The client to move.</param>
        /// <param name="channel">The channel to join.</param>
        /// <param name="password">Optional password. Pass empty string or null if unused.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task Move(Client client, Channel channel, string password = null)
        {
            Require.NotNull(nameof(client), client);
            Require.SameConnection(nameof(client), this, client);
            return client.MoveTo(channel, password);
        }

        /// <summary>
        /// Move a channel to a new parent channel
        /// </summary>
        /// <param name="channel">The channel to be moved.</param>
        /// <param name="newParent">The parent channel where the moved channel is to be inserted as child. Use null to insert as top-level channel.</param>
        /// <param name="newChannelOrder">the <see cref="Channel"/> after which <paramref name="channel"/> is sorted. <see langword="null"/> meaning its going to be the first <see cref="Channel"/> under <paramref name="newParent"/>.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task Move(Channel channel, Channel newParent, Channel newChannelOrder)
        {
            Require.NotNull(nameof(channel), channel);
            Require.SameConnection(nameof(channel), this, channel);
            return channel.MoveTo(newParent, newChannelOrder);
        }

        /// <summary>
        /// Play a local wave file
        /// </summary>
        /// <param name="path">Local path of the sound file in WAV format to be played.</param>
        /// <param name="loop">If true, the sound will be looping until the <see cref="WaveHandle"/> is paused or closed.</param>
        /// <returns>a waveHandle that allows controlling the playback.</returns>
        public WaveHandle PlayWaveFile(string path, bool loop = false)
        {
            Require.NotNull(nameof(path), path);
            return Library.Api.PlayWaveFileHandle(this, path, loop);
        }

        /// <summary>
        /// Add a client to the whisper allow list.
        /// </summary>
        /// <param name="client">The client to be added to the whisper allow list.</param>
        public void AllowWhispersFrom(Client client)
        {
            Require.NotNull(nameof(client), client);
            Require.SameConnection(nameof(client), this, client);
            Library.Api.AllowWhispersFrom(client);
        }

        /// <summary>
        /// Remove a client from the whisper allow list.
        /// </summary>
        /// <param name="client">The client to be removed from the whisper allow list.</param>
        public void RemoveFromAllowedWhispersFrom(Client client)
        {
            Require.NotNull(nameof(client), client);
            Require.SameConnection(nameof(client), this, client);
            Library.Api.RemoveFromAllowedWhispersFrom(client);
        }

        /// <summary>
        /// Returns every client with a matching UniqueId
        /// </summary>
        /// <param name="uniqueIdentifier">UniqueId to look for</param>
        /// <returns>A task that represents the asynchronous search operation.</returns>
        public Task<ICollection<Client>> FindClient(string uniqueIdentifier)
        {
            Require.NotNullOrEmpty(nameof(uniqueIdentifier), uniqueIdentifier);
            bool success = Cache.ClientIDsBuilder.Register(uniqueIdentifier);
            if (success == false)
                throw new InvalidOperationException("Can't search for the same UniqueIdentifier more then once at the same time.");
            Task task;
            Library.Api.TryRequestClientIDs(this, uniqueIdentifier, GetNextReturnCode(out task));
            return CollectList(task, Cache.ClientIDsBuilder, uniqueIdentifier);
        }

        private void ServerConnection_ClientIDsReceived(Client client)
        {
            throw new NotImplementedException();
        }

        private int GetInt(VirtualServerProperty flag)
        {
            return Library.Api.GetServerVariableAsInt(this, flag);
        }
        private ulong GetUInt64(VirtualServerProperty flag)
        {
            return Library.Api.GetServerVariableAsUInt64(this, flag);
        }
        private string GetString(VirtualServerProperty flag)
        {
            return Library.Api.GetServerVariableAsString(this, flag);
        }
        private string GetString(ConnectionProperty flag)
        {
            return Library.Api.GetConnectionVariableAsString(Self, flag);
        }
        private ulong GetUInt64(ConnectionProperty flag)
        {
            return Library.Api.GetConnectionVariableAsUInt64(Self, flag);
        }

        private float GetPlaybackConfigValueAsFloat(string ident)
        {
            return Library.Api.GetPlaybackConfigValueAsFloat(this, ident);
        }
        private void SetPlaybackConfigValue(string ident, string value)
        {
            Library.Api.SetPlaybackConfigValue(this, ident, value);
        }

        internal Task<ICollection<T>> CollectList<T>(Task task, ReceivedListBuilder<T> builder, string key)
        {
            return task.ContinueWith(antecendent =>
            {
                if (antecendent.IsFaulted)
                {
                    TeamSpeakException exception = antecendent.Exception.InnerException as TeamSpeakException;
                    if (exception == null)
                    {
                        System.Diagnostics.Debug.Assert(false);
                        throw antecendent.Exception;
                    }
                    switch (exception.ErrorCode)
                    {
                        case Error.DatabaseEmptyResult: return (ICollection<T>)new List<T>(0).AsReadOnly();
                        default: throw exception;
                    }
                }
                else return builder.Collect(key).AsReadOnly();
            });
        }

        #region Events

        internal void OnStatusChanged(ConnectStatus newStatus, Error error)
        {
            switch (newStatus)
            {
                case ConnectStatus.Connected:
                    Client self;
                    error = Library.Api.TryGetClientID(this, out self);
                    Debug.Assert(Error.Ok == error);
                    Self = self;
                    break;
                case ConnectStatus.ConnectionEstablished:
                    if (IsConnecting)
                    {
                        if (error == Error.Ok)
                            PassStartOrStop();
                        else
                            FailStartOrStop(error, null);
                    }
                    break;
                case ConnectStatus.Disconnected:
                    FailAllReturnCodes();
                    if (error != Error.Ok)
                        FailStartOrStop(error, null);
                    else if (IsConnecting == false)
                        PassStartOrStop();
                    else
                        FailStartOrStop(Error.Undefined, null);
                    break;
            }
            EventHelper.Run(StatusChanged, _ => _(this, newStatus, error));
        }

        internal void OnProtocolVersionReceived(int protocolVersion)
        {
            EventHelper.Run(ProtocolVersionReceived, _ => _(this, protocolVersion));
        }

        internal void OnNewChannel(Channel channel)
        {
            EventHelper.Run(NewChannel, _=>_(channel));
        }

        internal void OnNewChannelCreated(Channel channel, Client invoker)
        {
            EventHelper.Run(NewChannelCreated, _ => _(channel, invoker));
        }

        internal void OnChannelDeleted(Channel channel, Client invoker)
        {
            EventHelper.Run(ChannelDeleted,  _=>_(channel, invoker));
        }

        internal void OnChannelMoved(Channel channel, Client invoker)
        {
            EventHelper.Run(ChannelMoved, _ => _ (channel, invoker));
        }

        internal void OnChannelChanged(Channel channel, Client invoker)
        {
            channel?.RefreshProperties(wait: false);
            EventHelper.Run(ChannelChanged, _ => _(channel, invoker));
        }

        internal void OnClientUpdated(Client client, Client invoker)
        {
            client?.RefreshProperties(false);
            EventHelper.Run(ClientUpdated , _ => _(client, invoker));
        }

        internal void OnClientMoved(Client client, Channel oldChannel, Channel newChannel, Visibility visibility, Client invoker, string message)
        {
            EventHelper.Run(ClientMoved, _=>_(client, oldChannel, newChannel, visibility, invoker, message));
        }

        internal void OnSubscriptionClientMoved(Client client, Channel oldChannel, Channel newChannel, Visibility visibility)
        {
            EventHelper.Run(ClientMovedSubscription, _ => _(client, oldChannel, newChannel, visibility));
        }

        internal void OnClientTimeout(Client client, Channel oldChannel, Channel newChannel, Visibility visibility, string message)
        {
            EventHelper.Run(ClientTimeout, _=>_(client, oldChannel, newChannel, visibility, message));
        }

        internal void OnClientKickedFromChannel(Client client, Channel oldChannel, Channel newChannel, Visibility visibility, Client invoker, string message)
        {
            EventHelper.Run(ClientKickedFromChannel, _=>_(client, oldChannel, newChannel, visibility, invoker, message));
        }

        internal void OnClientKickedFromServer(Client client, Channel oldChannel, Channel newChannel, Visibility visibility, Client invoker, string message)
        {
            EventHelper.Run(ClientKickedFromServer, _ => _(client, oldChannel, newChannel, visibility, invoker, message));
        }

        internal void OnServerUpdated(Client editor)
        {
            EventHelper.Run(ServerUpdated, _ => _(this, editor));
        }

        internal void OnServerError(Error error, string returnCode, string extraMessage)
        {
            if (returnCode != null)
            {
                if (returnCode.StartsWith(ReturnCodePrefix))
                    SetReturnCodeResult(returnCode, error, extraMessage);
            }
            else if (IsConnecting)
            {
                FailStartOrStop(error, extraMessage);
            }
            EventHelper.Run(ServerError, _ => _(this, error, returnCode, extraMessage));
        }

        internal void OnServerStop(string message)
        {
            EventHelper.Run(ServerStop, _ => _(this, message));
        }

        internal void OnClientMessage(Client from, Client to, string message)
        {
            EventHelper.Run(ClientMessage, _ => _(from, to, message));
        }

        internal void OnChannelMessage(Client from, string message)
        {
            EventHelper.Run(ChannelMessage, _ => _(from, message));
        }

        internal void OnServerMessage(Client from, string message)
        {
            EventHelper.Run(ServerMessage, _ => _(from, message));
        }

        internal void OnTalkStatusChanged(Client client, TalkStatus status, bool isReceivedWhisper)
        {
            EventHelper.Run(TalkStatusChanged, _ => _(client, status, isReceivedWhisper));
        }

        internal void OnWhisperIgnored(Client client)
        {
            EventHelper.Run(WhisperIgnored, _ => _(client));
        }

        internal void OnChannelSubscribed(Channel channel)
        {
            EventHelper.Run(ChannelSubscribed, _ => _(channel));
        }

        internal void OnChannelSubscribesFinished()
        {
            EventHelper.Run(ChannelSubscribesFinished, _ => _(this));
        }

        internal void OnChannelUnsubscribed(Channel channel)
        {
            EventHelper.Run(ChannelUnsubscribed, _ => _(channel));
        }

        internal void OnChannelUnsubscribesFinished()
        {
            EventHelper.Run(ChannelUnsubscribesFinished, _ => _(this));
        }

        internal void OnChannelDescriptionUpdated(Channel channel)
        {
            EventHelper.Run(ChannelDescriptionUpdated, _ => _(channel));
        }

        internal void OnChannelPasswordChanged(Channel channel)
        {
            EventHelper.Run(ChannelPasswordChanged, _ => _(channel));
        }

        internal void OnPlaybackShutdownCompleted()
        {
            EventHelper.Run(PlaybackShutdownCompleted, _ => _(this));
        }

        internal void OnEditPlaybackVoiceData(Client client, IntPtr samples, int sampleCount, int channels)
        {
            EditPlaybackVoiceDataEventHandler editPlaybackVoiceData = EditPlaybackVoiceData;
            if (editPlaybackVoiceData != null)
            {
                short[] managed_samples = new short[sampleCount];
                Marshal.Copy(samples, managed_samples, 0, sampleCount);
                editPlaybackVoiceData(client, managed_samples, channels);
                Marshal.Copy(managed_samples, 0, samples, sampleCount);
            }
        }

        internal void OnEditPostProcessVoiceData(Client client, IntPtr samples, int sampleCount, int channels, IntPtr channelSpeakerArray, ref Speakers channelFillMask)
        {
            EditPostProcessVoiceDataEventHandler editPostProcessVoiceData = EditPostProcessVoiceData;
            if (editPostProcessVoiceData != null)
            {
                short[] managed_samples = new short[sampleCount];
                Marshal.Copy(samples, managed_samples, 0, sampleCount);
                Speakers[] channelSpeakers = new Speakers[channels];
                Marshal.Copy(channelSpeakerArray, (int[])(object)channelSpeakers, 0, channels);
                editPostProcessVoiceData(client, managed_samples, channels, channelSpeakers, ref channelFillMask);
                if (channelFillMask != (Speakers)0)
                    Marshal.Copy(managed_samples, 0, samples, sampleCount);
            }
        }

        internal void OnEditMixedPlaybackVoiceData(IntPtr samples, int sampleCount, int channels, IntPtr channelSpeakerArray, ref Speakers channelFillMask)
        {
            EditMixedPlaybackVoiceDataEventHandler editMixedPlaybackVoiceData = EditMixedPlaybackVoiceData;
            if (editMixedPlaybackVoiceData != null)
            {
                short[] managed_samples = new short[sampleCount * channels];
                Marshal.Copy(samples, managed_samples, 0, managed_samples.Length);
                Speakers[] channelSpeakers = new Speakers[channels];
                Marshal.Copy(channelSpeakerArray, (int[])(object)channelSpeakers, 0, channels);
                editMixedPlaybackVoiceData(this, managed_samples, channels, channelSpeakers, ref channelFillMask);
                if (channelFillMask != (Speakers)0)
                    Marshal.Copy(managed_samples, 0, samples, managed_samples.Length);
            }
        }

        internal void OnEditCapturedVoiceData(IntPtr samples, int sampleCount, int channels, ref bool edited, ref bool cancel)
        {
            EditCapturedVoiceDataEventHandler editCapturedVoiceData = EditCapturedVoiceData;
            if (editCapturedVoiceData != null)
            {
                short[] managed_samples = new short[sampleCount];
                Marshal.Copy(samples, managed_samples, 0, sampleCount);
                editCapturedVoiceData(this, managed_samples, channels, ref edited, ref cancel);
                if (edited && cancel == false)
                    Marshal.Copy(managed_samples, 0, samples, sampleCount);
            }
        }

        internal void OnCustom3dRolloffCalculationClient(Client client, float distance, ref float volume)
        {
            Custom3dRolloffCalculationClient?.Invoke(client, distance, ref volume);
        }

        internal void OnCustom3dRolloffCalculationWave(WaveHandle wave, float distance, ref float volume)
        {
            Custom3dRolloffCalculationWaveReceived?.Invoke(wave, distance, ref volume);
        }

        internal void OnCheckUniqueIdentifier(string uniqueIdentifier, out bool cancelConnect)
        {
            cancelConnect = false;
            CheckServerUniqueIdentifier?.Invoke(this, uniqueIdentifier, ref cancelConnect);
        }

        internal void OnFileTransferStatusReceived(FileTransfer transfer, Error status)
        {
            EventHelper.Run(FileTransferStatusReceived, _ => _(transfer, status));
        }

        internal void OnFileListReceived(Channel channel, string path, string name, ulong size, DateTimeOffset dateTime, FileListType type, ulong incompleteSize, string returnCode)
        {
            Cache.FileListBuilder.Add(returnCode, new FileInfo(channel, path, name, size, dateTime, type, incompleteSize));
        }

        internal void OnFileInfoReceived(Channel channel, string name, ulong size, DateTimeOffset dateTime)
        {
            EventHelper.Run(FileInfoReceived, _ => _(channel, name, size, dateTime));
        }

        internal void OnClientIDReceived(Client client)
        {
            Cache.ClientIDsBuilder.Add(client.UniqueIdentifier, client);
        }

        #endregion Events

        #region ReturnCode

        private ConcurrentDictionary<string, TaskCompletionSource<Error>> ReturnCodeTasks = new ConcurrentDictionary<string, TaskCompletionSource<Error>>();
        private int ReturnCodeValue = 0;
        private const string ReturnCodePrefix = "_";

        internal string GetNextReturnCode(out Task task)
        {
            int value = Interlocked.Increment(ref ReturnCodeValue);
            string returnCode = ReturnCodePrefix + value;
            TaskCompletionSource<Error>  taskCompletionSource = new TaskCompletionSource<Error>();
            if (ReturnCodeTasks.TryAdd(returnCode, taskCompletionSource) == false)
                Debug.Assert(false);
            task = taskCompletionSource.Task;
            return returnCode;
        }

        internal void SetReturnCodeResult(string returnCode, Error error, string extraMessage)
        {
            TaskCompletionSource<Error> taskCompletionSource;
            if (ReturnCodeTasks.TryRemove(returnCode, out taskCompletionSource) == false)
                return;
            Task.Factory.StartNew(() =>
            {
                switch (error)
                {
                    case Error.Ok:
                    case Error.OkNoUpdate:
                        taskCompletionSource.SetResult(error);
                        break;
                    default:
                        taskCompletionSource.SetException(Library.CreateException(error, extraMessage));
                        break;
                }
            });
        }

        private void FailAllReturnCodes()
        {
            foreach (TaskCompletionSource<Error> taskCompletionSource in ReturnCodeTasks.Values)
                taskCompletionSource.SetException(Library.CreateException(Error.ConnectionLost, null));
            ReturnCodeTasks.Clear();
        }

        #endregion RecturnCode
    }
}
