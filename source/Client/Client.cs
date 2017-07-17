using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TeamSpeak.Sdk.Client
{
    /// <summary>
    /// Represents a Client on a TeamSpeak-Server
    /// </summary>
    public class Client: IEquatable<Client>
    {
        /// <summary>
        /// ID of the client
        /// </summary>
        public ushort ID { get; internal set; }
        /// <summary>
        /// Server Connection
        /// </summary>
        public Connection Connection { get; }
        /// <summary>
        /// The channel the client is currently joined
        /// </summary>
        public Channel Channel { get { return Library.Api.GetChannelOfClient(this); } }
        /// <summary>
        /// Server ConnectionInfo
        /// </summary>
        public ConnectionInfo ConnectionInfo { get; }

        // cached UniqueIdentifier, Nickname, Metadata, so that they can survive the clients death
        private string CachedUniqueIdentifier;
        private string CachedNickname;
        private string CachedMetaData;

        /// <summary>
        /// Creates a new Client-Object.
        /// </summary>
        /// <remarks>does not spawn a new client on the server. To spawn a new client use another <see cref="Sdk.Client.Connection"/></remarks>
        /// <param name="connection">the server connection</param>
        /// <param name="id">ID of the client</param>
        public Client(Connection connection, ushort id)
            : this(connection, id, waitForProperties: false)
        {
        }
        
        internal Client(Connection connection, ushort id, bool waitForProperties)
        {
            Require.NotNull(nameof(connection), connection);
            ID = id;
            Connection = connection;
            ConnectionInfo = new ConnectionInfo(this);
            CachedMetaData = string.Empty;
            RefreshProperties(waitForProperties);
        }

        /// <summary>
        /// Unique ID for this client. Stays the same after restarting the application, so you can use this to identify individual users
        /// </summary>
        public string UniqueIdentifier
        {
            get { return GetString(ClientProperty.UniqueIdentifier, CachedUniqueIdentifier); }
        }
        /// <summary>
        /// Nickname used by the client. This value is always automatically updated for visible clients
        /// </summary>
        public string Nickname
        {
            get { return GetString(ClientProperty.Nickname, CachedNickname); }
            set
            {
                Require.NotNull(nameof(value), value);
                SetString(ClientProperty.Nickname, value, ref CachedNickname);
            }
        }
        /// <summary>
        /// Application version used by this client. Needs to be requested with <see cref="RequestClientVariables"/> unless called on own client
        /// </summary>
        public string Version
        {
            get { return GetString(ClientProperty.Version); }
        }
        /// <summary>
        /// Operating system used by this client. Needs to be requested with <see cref="RequestClientVariables"/> unless called on own client.
        /// </summary>
        public string Platform
        {
            get { return GetString(ClientProperty.Platform); }
        }
        /// <summary>
        /// Set when the client is currently sending voice data to the server. Always available for visible clients.
        /// </summary>
        public bool IsTalking
        {
            get { return GetBool(ClientProperty.FlagTalking); }
        }
        /// <summary>
        /// Indicates the mute status of the clients capture device. Always available for visible clients
        /// </summary>
        public bool InputMuted
        {
            get { return GetBool(ClientProperty.InputMuted); }
            set { SetBool(ClientProperty.InputMuted, value); }
        }
        /// <summary>
        /// Indicates the combined mute status of the clients playback and capture devices. Always available for visible clients
        /// </summary>
        public bool OutputMuted
        {
            get { return GetBool(ClientProperty.OutputMuted); }
            set { SetBool(ClientProperty.OutputMuted, value); }
        }
        /// <summary>
        /// Indicates the mute status of the clients playback device. Always available for visible clients
        /// </summary>
        public bool OutputOnlyMuted
        {
            get { return GetBool(ClientProperty.OutputonlyMuted); }
        }
        /// <summary>
        /// Set if the clients capture device is not available. Always available for visible clients
        /// </summary>
        public bool InputHardwareEnabled
        {
            get { return GetBool(ClientProperty.InputHardware); }
        }
        /// <summary>
        /// True if the clients playback device is not available. Always available for visible clients
        /// </summary>
        public bool OutputHardware
        {
            get { return GetBool(ClientProperty.OutputHardware); }
        }

        /// <summary>
        /// Determines if the sound input is deactivated , only usable for <see cref="Connection.Self"/>, not propagated to the network.
        /// </summary>
        public bool IsInputDeactivated
        {
            get { return GetBool(ClientProperty.InputDeactivated); }
            set { SetBool(ClientProperty.InputDeactivated, value); }
        }

        /// <summary>
        /// Time the client has been idle. Needs to be requested with <see cref="RequestClientVariables"/>
        /// </summary>
        public TimeSpan IdleTime
        {
            get { return TimeSpan.FromSeconds(GetInt(ClientProperty.IdleTime)); }
        }
        /// <summary>
        /// Default channel name used in the last <see cref="O:TeamSpeak.Sdk.Client.Connection.Start"/> call. Only available for own client
        /// </summary>
        public string DefaultChannel
        {
            get { return GetString(ClientProperty.DefaultChannel); }
        }
        /// <summary>
        /// Not used by TeamSpeak 3, offers free storage for SDK users. Always available for visible clients
        /// </summary>
        public string MetaData
        {
            get { return GetString(ClientProperty.MetaData, CachedMetaData); }
            set
            {
                Require.NotNull(nameof(value), value);
                SetString(ClientProperty.MetaData, value, ref CachedMetaData);
            }
        }
        /// <summary>
        /// Indicates a client has been locally muted. Client-side only
        /// </summary>
        public bool Muted
        {
            get { return GetBool(ClientProperty.IsMuted); }
            set
            {
                if (value) Connection.Mute(this);
                else Connection.Unmute(this);
            }
        }
        /// <summary>
        /// Indicates a client is currently recording all voice data in his channel
        /// </summary>
        public bool IsRecording
        {
            get { return GetBool(ClientProperty.IsRecording); }
        }
        /// <summary>
        /// The client volume modifier
        /// </summary>
        public float VolumeModificator
        {
            get { return GetUInt64(ClientProperty.VolumeModificator); }
            set { Library.Api.SetClientVolumeModifier(Connection, ID, value); }
        }

        /// <summary>
        /// Switch the client to a certain channel.
        /// </summary>
        /// <param name="channel">the channel to join.</param>
        /// <param name="password">Optional password. Pass empty string or null if unused.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task MoveTo(Channel channel, string password = null)
        {
            Require.NotNull(nameof(channel), channel);
            Require.SameConnection(nameof(channel), Connection, channel);
            Task task;
            Library.Api.RequestClientMove(this, channel, password, Connection.GetNextReturnCode(out task));
            return task;
        }

        /// <summary>
        /// Request the latest data for the client from the server.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task RequestClientVariables()
        {
            TaskCompletionSource<Error> tcs = new TaskCompletionSource<Error>();
            Task task;
            Library.Api.RequestClientVariables(this, Connection.GetNextReturnCode(out task));
            task.ContinueWith(t =>
            {
                RefreshProperties(false);
                if (t.IsFaulted)
                    tcs.SetException(t.Exception);
                else tcs.SetResult(Error.Ok);
            });
            return tcs.Task;
        }

        /// <summary>
        /// Kick the client from the channel
        /// </summary>
        /// <param name="kickReason">A short message explaining why the client is kicked from the channel.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task KickFromChannel(string kickReason = null)
        {
            Task task;
            Library.Api.RequestClientKickFromChannel(this, kickReason, Connection.GetNextReturnCode(out task));
            return task;
        }

        /// <summary>
        /// Kick the client from the server
        /// </summary>
        /// <param name="kickReason">A short message explaining why the client is kicked from the server.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task KickFromServer(string kickReason = null)
        {
            Task task;
            Library.Api.RequestClientKickFromServer(this, kickReason, Connection.GetNextReturnCode(out task));
            return task;
        }

        /// <summary>
        /// Send a private text message to the client.
        /// </summary>
        /// <param name="message">The text message</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task SendTextMessage(string message)
        {
            Require.NotNull(nameof(message), message);
            Task task;
            Library.Api.RequestSendPrivateTextMsg(this, message, Connection.GetNextReturnCode(out task));
            return task;
        }

        /// <summary>
        /// Adjust a position and velocity in 3D space
        /// </summary>
        /// <param name="position">position of the given client in 3D space</param>
        public void Set3DAttributes(Vector position)
        {
            Library.Api.Channelset3DAttributes(this, position);
        }

        /// <summary>
        /// Compares two <see cref="Client"/> for equality. 
        /// </summary>
        /// <param name="a">The first <see cref="Client"/> structure to compare.</param>
        /// <param name="b">The second <see cref="Client"/> structure to compare.</param>
        /// <returns>true if the ID and Mode of a and b are equal; otherwise, false.</returns>
        public static bool operator ==(Client a, Client b)
        {
            if (object.ReferenceEquals(a, b)) return true;
            if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null)) return false;
            return a.Connection == b.Connection && a.ID == b.ID;
        }

        /// <summary>
        /// Compares two <see cref="Client"/> for inequality. 
        /// </summary>
        /// <param name="a">The first <see cref="Client"/> structure to compare.</param>
        /// <param name="b">The second <see cref="Client"/> structure to compare.</param>
        /// <returns>true if the ID and Mode of a and b are different; otherwise, false.</returns>
        public static bool operator !=(Client a, Client b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            string result = Nickname;
            if (string.IsNullOrEmpty(result) == false)
                return result;
            else return base.ToString();
        }

        /// <summary>
        /// Indicates whether this instance and a another instance  are equal.
        /// </summary>
        /// <param name="other">Another instance to compare to.</param>
        /// <returns>true if this and  the other instance represent the same value; otherwise, false.</returns>
        public bool Equals(Client other)
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
            return obj is Client && this == (Client)obj;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return Connection.ID.GetHashCode() * 7 + ID.GetHashCode();
        }

        private int GetInt(ClientProperty flag)
        {
            return Library.Api.GetClientVariableAsInt(this, flag);
        }
        private  bool GetBool(ClientProperty flag)
        {
            return GetInt(flag) != 0;
        }
        private ulong GetUInt64(ClientProperty flag)
        {
            return Library.Api.GetClientVariableAsUInt64(this, flag);
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
            if (this != Connection.Self) throw new NotSupportedException();
            Library.Api.SetClientSelfVariableAsInt(Connection, flag, value);
            FlushClientSelfUpdates();
        }
        private void SetBool(ClientProperty flag, bool value)
        {
            SetInt(flag, value ? 1 : 0);
        }
        private void SetUInt64(ClientProperty flag)
        {
            if (this != Connection.Self) throw new NotSupportedException();
            Library.Api.GetClientVariableAsUInt64(this, flag);
            FlushClientSelfUpdates();
        }
        private void SetString(ClientProperty flag, string value, ref string cache)
        {
            if (this != Connection.Self) throw new NotSupportedException();
            Library.Api.SetClientSelfVariableAsString(Connection, flag, value);
            FlushClientSelfUpdates();
            cache = value;
        }

        private void FlushClientSelfUpdates()
        {
            // anything but threadsafe!
            if (Connection.Status != ConnectStatus.Disconnected)
            {
                Task task;
                Library.Api.FlushClientSelfUpdates(Connection, Connection.GetNextReturnCode(out task));
                task.Wait();
            }
        }

        private void FillCache(ClientProperty flag, bool wait, ref string cache)
        {
            string value;
            if (Library.Api.TryGetClientVariableAsString(this, flag, out value) == Error.Ok)
            {
                cache = value;
                return;
            }
            if (wait)
            {
                System.Diagnostics.Stopwatch timer = System.Diagnostics.Stopwatch.StartNew();
                do
                {
                    Thread.Yield();
                    if (Library.Api.TryGetClientVariableAsString(this, flag, out cache) == Error.Ok)
                    {
                        cache = value;
                        return;
                    }
                }
                while (timer.ElapsedMilliseconds < 25);
            }
        }

        internal void RefreshProperties(bool wait)
        {
            if (ID == 0) return;
            FillCache(ClientProperty.Nickname, wait, ref CachedNickname);
            FillCache(ClientProperty.UniqueIdentifier, wait, ref CachedUniqueIdentifier);
            FillCache(ClientProperty.MetaData, wait, ref CachedMetaData);
        }

        internal void Hint(string nickname, string uniqueIdentifier)
        {
            if (ID == 0) return;
            CachedNickname = nickname ?? CachedNickname;
            CachedUniqueIdentifier = uniqueIdentifier ?? CachedUniqueIdentifier;
        }
    }
}