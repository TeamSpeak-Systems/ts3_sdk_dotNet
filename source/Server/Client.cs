using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TeamSpeak.Sdk.Server
{
    /// <summary>
    /// Represents a <see cref="Client"/> on a <see cref="Sdk.Server.VirtualServer"/>
    /// </summary>
    public class Client
    {
        /// <summary>
        /// ID of the <see cref="Client"/>
        /// </summary>
        public ushort ID { get; }
        /// <summary>
        /// The <see cref="Sdk.Server.VirtualServer"/> the <see cref="Client"/> is connected to.
        /// </summary>
        public VirtualServer VirtualServer { get; }

        private string CachedUniqueIdentifier;
        private string CachedNickname;

        /// <summary>
        /// Creates a new <see cref="Client"/>-Object.
        /// </summary>
        /// <remarks>does not spawn a new client on the <see cref="Sdk.Server.VirtualServer"/>. To spawn a new client connect to the <see cref="Sdk.Server.VirtualServer"/> using a <see cref="Sdk.Client.Connection"/>.</remarks>
        /// <param name="virtualServer">the <see cref="Sdk.Server.VirtualServer"/> the <see cref="Client"/> is on.</param>
        /// <param name="id">ID of the <see cref="Client"/></param>

        public Client(VirtualServer virtualServer, ushort id)
            : this(virtualServer, id, waitForProperties: false)
        {
        }

        internal Client(VirtualServer virtualServer, ushort id, bool waitForProperties)
        {
            Sdk.Require.NotNull(nameof(virtualServer), virtualServer);
            ID = id;
            VirtualServer = virtualServer;
            RefreshProperties(waitForProperties);
        }
        /// <summary>
        /// The channel the client is currently joined.
        /// </summary>
        public Channel Channel
        {
            get { return Library.Api.GetChannelOfClient(this); }
        }
        /// <summary>
        /// Get a <see cref="ClientProperty"/> as a <see cref="string"/>
        /// </summary>
        /// <param name="property">the property</param>
        /// <returns>the property as a string</returns>
        public string this[ClientProperty property]
        {
            get { return GetString(property); }
        }
        /// <summary>
        /// Unique ID for this client. Stays the same after restarting the application, so you can use this to identify individual users.
        /// </summary>
        public string UniqueIdentifier
        {
            get { return GetString(ClientProperty.UniqueIdentifier, CachedUniqueIdentifier); }
        }
        /// <summary>
        /// Nickname used by the client.
        /// </summary>
        /// <remarks>Use <see cref="FlushClientVariables"/> to flush changes.</remarks>
        public string Nickname
        {
            get { return GetString(ClientProperty.Nickname, CachedNickname); }
            set
            {
                Sdk.Require.NotNull(nameof(value), value);
                SetString(ClientProperty.Nickname, value);
            }
        }
        /// <summary>
        /// Application version used by this client.
        /// </summary>
        public string Version
        {
            get { return GetString(ClientProperty.Version); }
        }
        /// <summary>
        /// Operating system used by this client.
        /// </summary>
        public string Platform
        {
            get { return GetString(ClientProperty.Platform); }
        }
        /// <summary>
        /// Set when the client is currently sending voice data to the server.
        /// </summary>
        public bool IsTalking
        {
            get { return GetBool(ClientProperty.FlagTalking); }
        }
        /// <summary>
        /// Indicates the mute status of the clients capture device.
        /// </summary>
        /// <remarks>Use <see cref="FlushClientVariables"/> to flush changes.</remarks>
        public bool InputMuted
        {
            get { return GetBool(ClientProperty.InputMuted); }
            set { SetBool(ClientProperty.InputMuted, value); }
        }
        /// <summary>
        /// Indicates the combined mute status of the clients playback and capture devices.
        /// </summary>
        /// <remarks>Use <see cref="FlushClientVariables"/> to flush changes.</remarks>
        public bool OutputMuted
        {
            get { return GetBool(ClientProperty.OutputMuted); }
            set { SetBool(ClientProperty.OutputMuted, value); }
        }
        /// <summary>
        /// Indicates the mute status of the clients playback device.
        /// </summary>
        public bool OutputOnlyMuted
        {
            get { return GetBool(ClientProperty.OutputonlyMuted); }
        }
        /// <summary>
        /// Set if the clients capture device is not available.
        /// </summary>
        public bool InputHardwareEnabled
        {
            get { return GetBool(ClientProperty.InputHardware); }
        }
        /// <summary>
        /// True if the clients playback device is not available.
        /// </summary>
        public bool OutputHardware
        {
            get { return GetBool(ClientProperty.OutputHardware); }
        }

        /// <summary>
        /// Time the client has been idle.
        /// </summary>
        public TimeSpan IdleTime
        {
            get { return TimeSpan.FromSeconds(GetInt(ClientProperty.IdleTime)); }
        }
        /// <summary>
        /// Default channel name used in the last <see cref="O:TeamSpeak.Sdk.Client.Connection.Start"/> call.
        /// </summary>
        public string DefaultChannel
        {
            get { return GetString(ClientProperty.DefaultChannel); }
        }
        /// <summary>
        /// Not used by TeamSpeak 3, offers free storage for SDK users.
        /// </summary>
        /// <remarks>Use <see cref="FlushClientVariables"/> to flush changes.</remarks>
        public string MetaData
        {
            get { return GetString(ClientProperty.MetaData); }
            set
            {
                Sdk.Require.NotNull(nameof(value), value);
                SetString(ClientProperty.MetaData, value);
            }
        }
        /// <summary>
        /// Indicates a client is currently recording all voice data in his channel
        /// </summary>
        public bool IsRecording
        {
            get { return GetBool(ClientProperty.IsRecording); }
        }

        public void FlushClientVariables()
        {
            Library.Api.FlushClientVariable(this);
        }

        /// <summary>
        /// With a whisper list set a client can talk to the specified clients and channels bypassing the standard rule that voice is only transmitted to the current channel. Whisper lists can be defined for individual clients.
        /// </summary>
        /// <remarks>
        /// To control which client is allowed to whisper to own client, the Library implements an internal whisper whitelist mechanism. When a client receives a whisper while the whispering client has not yet been added to the whisper allow list, the receiving client gets the <see cref="Connection.WhisperIgnored"/>-Event. Note that whisper voice data is not received until the sending client is added to the receivers whisper allow list.
        /// </remarks>
        /// <param name="targetChannelArray">array of channels to whisper to, set to null to disable</param>
        /// <param name="targetClientArray">array of clients to whisper to, set to null to disable</param>
        public void SetWhisperList(Channel[] targetChannelArray, Client[] targetClientArray)
        {
            Sdk.Require.EntriesNotNull(nameof(targetChannelArray), targetChannelArray);
            Require.SameVirtualServer(nameof(targetChannelArray), VirtualServer, targetChannelArray);
            Sdk.Require.EntriesNotNull(nameof(targetClientArray), targetClientArray);
            Require.SameVirtualServer(nameof(targetClientArray), VirtualServer, targetClientArray);
            Library.Api.SetClientWhisperList(this, targetChannelArray, targetClientArray);
        }

        private int GetInt(ClientProperty flag)
        {
            return Library.Api.GetClientVariableAsInt(this, flag);
        }
        private bool GetBool(ClientProperty flag)
        {
            return GetInt(flag) != 0;
        }
        private string GetString(ClientProperty flag)
        {
            return Library.Api.GetClientVariableAsString(this, flag);
        }
        private string GetString(ClientProperty flag, string cache)
        {
            string result;
            Error error = Library.Api.TryGetClientVariableAsString(this, flag, out result);
            if (error == Error.Ok)
                return result;
            else return cache;
        }
        private void SetInt(ClientProperty flag, int value)
        {
            Library.Api.SetClientVariableAsInt(this, flag, value);
        }
        private void SetBool(ClientProperty flag, bool value)
        {
            SetInt(flag, value ? 1 : 0);
        }
        private void SetString(ClientProperty flag, string value)
        {
            Library.Api.SetClientVariableAsString(this, flag, value);
        }

        internal void RefreshProperties(bool wait)
        {
            if (ID == 0) return;
            if (Library.Api.TryGetClientVariableAsString(this, ClientProperty.Nickname, out CachedNickname) != Error.Ok && wait)
            {
                System.Diagnostics.Stopwatch timer = System.Diagnostics.Stopwatch.StartNew();
                do Thread.Yield();
                while (Library.Api.TryGetClientVariableAsString(this, ClientProperty.Nickname, out CachedNickname) != Error.Ok && timer.ElapsedMilliseconds < 25);
                System.Diagnostics.Debug.Assert(CachedNickname != null);
            }
            Library.Api.TryGetClientVariableAsString(this, ClientProperty.UniqueIdentifier, out CachedUniqueIdentifier);
        }

        internal void Hint(string nickname, string uniqueIdentifier)
        {
            if (ID == 0) return;
            CachedNickname = nickname ?? CachedNickname;
            CachedUniqueIdentifier = uniqueIdentifier ?? CachedUniqueIdentifier;
        }
    }
}