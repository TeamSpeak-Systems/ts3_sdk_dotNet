using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TeamSpeak.Sdk.Server
{
    /// <summary>
    /// A representation of a Virtual Server
    /// </summary>
    public class VirtualServer
    {
        /// <summary>
        /// ID of the virtual server
        /// </summary>
        public ulong ID { get; }

        internal readonly Cache Cache;
        private readonly Channel ZeroChannel;

        /// <summary>
        /// Creates a new <see cref="VirtualServer"/>-Object with a already known id 
        /// </summary>
        /// <param name="id">Id of the connection</param>
        /// <remarks>internal use, <see cref="Library.CreateVirtualServer(uint, string, uint, string, string)"/> to spawn a new <see cref="VirtualServer"/></remarks>
        internal VirtualServer(ulong id)
        {
            ID = id;
            Cache = new Cache(this);
            ZeroChannel = new Channel(this, 0);
        }

        /// <summary>
        /// A collection of all <see cref="Channel">channels</see> on the <see cref="VirtualServer"/>
        /// </summary>
        public ReadonlyChannelCollection AllChannels { get { return new ReadonlyChannelCollection(Library.Api.GetChannelList(this)); } }

        /// <summary>
        /// A collection of all currently visible <see cref="Client">clients</see> on the <see cref="VirtualServer"/>
        /// </summary>
        public ICollection<Client> AllClients { get { return Library.Api.GetClientList(this)?.AsReadOnly(); } }
        /// <summary>
        /// Information about the <see cref="VirtualServer"/> connection.
        /// </summary>
        public ConnectionInfo ConnectionInfo { get; }

        /// <summary>
        /// Get a <see cref="VirtualServerProperty"/> as a <see cref="string"/>
        /// </summary>
        /// <param name="property">the property</param>
        /// <returns>the property as a string</returns>
        public string this[VirtualServerProperty property]
        {
            get { return GetString(property); }
        }

        /// <summary>
        /// Identifies this particular server installation
        /// </summary>
        public string UniqueIdentifier
        {
            get { return GetString(VirtualServerProperty.UniqueIdentifier); }
        }

        /// <summary>
        /// Name of the <see cref="VirtualServer"/>
        /// </summary>
        public string Name
        {
            get { return GetString(VirtualServerProperty.Name); }
            set { SetString(VirtualServerProperty.Name, value); }
        }

        /// <summary>
        /// Message that is displayed to every connecting <see cref="Client"/>
        /// </summary>
        public string WelcomeMessage
        {
            get { return GetString(VirtualServerProperty.Welcomemessage); }
            set { SetString(VirtualServerProperty.Welcomemessage, value); }
        }

        /// <summary>
        /// Operating system used by this server.
        /// </summary>
        public string Platform
        {
            get { return GetString(VirtualServerProperty.Platform); }
        }

        /// <summary>
        /// Application version of this server. 
        /// </summary>
        public string Version
        {
            get { return GetString(VirtualServerProperty.Version); }
        }

        /// <summary>
        /// Defines maximum number of clients which may connect to this server. 
        /// </summary>
        public int MaxClients
        {
            get { return GetInt(VirtualServerProperty.Maxclients); }
            set { SetInt(VirtualServerProperty.Maxclients, value); }
        }

        /// <summary>
        /// Connecting <see cref="Client">Clients</see> must know the password in order to get access to the server.
        /// </summary>
        /// <remarks>Set Password to <see langword="null"/> or <see cref="string.Empty"/> to disable.</remarks>
        public string Password
        {
            get { return GetString(VirtualServerProperty.Password); }
            set { SetString(VirtualServerProperty.Password, Password); }
        }

        /// <summary>
        /// Number of clients currently on this virtual server.
        /// </summary>
        public int ClientsOnline
        {
            get { return GetInt(VirtualServerProperty.ClientsOnline); }
        }

        /// <summary>
        /// Number of channels currently on this virtual server.
        /// </summary>
        public int ChannelsOnline
        {
            get { return GetInt(VirtualServerProperty.ChannelsOnline); }
        }

        /// <summary>
        /// Time when the <see cref="VirtualServer"/> was created.
        /// </summary>
        public DateTimeOffset Created
        {
            get { return Native.FromUnixTime(GetUInt64(VirtualServerProperty.Created)); }
        }

        /// <summary>
        /// Uptime of the <see cref="VirtualServer"/>.
        /// </summary>
        public TimeSpan Uptime
        {
            get { return TimeSpan.FromSeconds(GetInt(VirtualServerProperty.Uptime)); }
        }

        /// <summary>
        /// Defines if voice data encryption is configured per channel, or globally forced on or globally forced off for this virtual server. 
        /// The default behavior is configure per channel, in this case modifying <see cref="Channel.CodecIsUnencrypted"/>
        /// defines voice data encryption of individual channels.
        /// </summary>
        public CodecEncryptionMode CodecEncryptionMode
        {
            get { return (CodecEncryptionMode)GetInt(VirtualServerProperty.CodecEncryptionMode); }
            set { SetInt(VirtualServerProperty.CodecEncryptionMode, (int)value); }
        }

        /// <summary>
        /// Create a new Channel
        /// </summary>
        /// <param name="name">Name of the channel</param>
        /// <param name="parent">The parent channel</param>
        /// <param name="password">Password for password-protected channels.</param>
        /// <param name="topic">Single-line channel topic</param>
        /// <param name="description">Channel description. Can have multiple lines.</param>
        /// <param name="order">the <see cref="Channel"/> after which the new <see cref="Channel"/> is sorted. <see langword="null"/> meaning its going to be the first <see cref="Channel"/> under <paramref name="parent"/>.</param>
        /// <param name="isPermanent">Permanent channels will be restored when the server restarts.</param>
        /// <param name="isSemiPermanent">Semi-permanent channels are not automatically deleted when the last user left but will not be restored when the server restarts.</param>
        /// <param name="codecQuality">Quality of channel codec of this channel. Valid values range from 0 to 10, default is 7. Higher values result in better speech quality but more bandwidth usage</param>
        /// <param name="codecIsUnencrypted">If <see langword="true"/>, this channel is not using encrypted voice data. If <see langword="false"/>, voice data is encrypted for this channel.</param>
        /// <returns>A task that represents the asynchronous creation of the channel.</returns>
        public Channel CreateChannel(string name, Channel parent = null, string password = null, string topic = null, string description = null, Channel order = null,
            bool? isPermanent = null, bool? isSemiPermanent = null,
            int? codecQuality = null, bool? codecIsUnencrypted = null)
        {
            Sdk.Require.NotNull(nameof(name), name);
            if (parent != null)
            {
                Require.SameVirtualServer(nameof(parent), this, parent);
            }

            ZeroChannel.Name = name;
            ZeroChannel.Topic = topic ?? "";
            ZeroChannel.Description = description ?? "";
            ZeroChannel.Password = password ?? "";
            ZeroChannel.Order = order;
            ZeroChannel.IsPermanent = (isPermanent != null) ? isPermanent.Value : false;
            ZeroChannel.IsSemiPermanent = (isSemiPermanent != null) ? isSemiPermanent.Value : false;
            ZeroChannel.CodecQuality = (codecQuality != null) ? codecQuality.Value : 10;
            ZeroChannel.CodecIsUnencrypted = (codecIsUnencrypted != null) ? codecIsUnencrypted.Value : false;

            var channel = Library.Api.FlushChannelCreation(this, parent?.ID ?? 0UL);
            return channel;
        }

        /// <summary>
        /// Voice Data event handler
        /// </summary>
        public event VoiceDataEventHandler VoiceData;

        /// <summary>
        /// Client started talking event handler
        /// </summary>
        public event ClientStartTalkingEventHandler ClientStartTalking;

        /// <summary>
        /// Client stopped talking event handler
        /// </summary>
        public event ClientStopTalkingEventHandler ClientStopTalking;

        /// <summary>
        /// Client connected event handler
        /// </summary>
        public event ClientConnectedEventHandler ClientConnected;

        /// <summary>
        /// Client disconnected event handler
        /// </summary>
        public event ClientDisconnectedEventHandler ClientDisconnected;

        /// <summary>
        /// Client moved event handler
        /// </summary>
        public event ClientMovedEventHandler ClientMoved;

        /// <summary>
        /// Channel created event handler
        /// </summary>
        public event ChannelCreatedEventHandler ChannelCreated;

        /// <summary>
        /// Channel editedd event handler
        /// </summary>
        public event ChannelEditedEventHandler ChannelEdited;

        /// <summary>
        /// Channel delete event handler
        /// </summary>
        public event ChannelDeletedEventHandler ChannelDeleted;

        /// <summary>
        /// Server text message event handler
        /// </summary>
        public event ServerTextMessageEventHandler ServerTextMessage;

        /// <summary>
        /// Channel text message event handler
        /// </summary>
        public event ChannelTextMessageEventHandler ChannelTextMessage;

        /// <summary>
        /// Accounting error event handler
        /// </summary>
        public event AccountingErrorEventHandler AccountingError;

        /// <summary>
        /// FileTransfer event event handler
        /// </summary>
        public event FileTransferEventEventHandler FileTransferEvent;

        /// <summary>
        /// Transform File Path event handler
        /// </summary>
        public event TransformFilePathEventHandler TransformFilePath;

        public PermissionChecks PermissionChecks { get; } = new PermissionChecks();

        public string KeyPair
        {
            get { return Library.Api.GetVirtualServerKeyPair(this); }
        }

        public void Stop()
        {
            Library.Api.StopVirtualServer(this);
        }

        public void Kick(params Client[] clients)
        {
            Kick(null, clients);
        }
        public void Kick(string reason, params Client[] clients)
        {
            Kick(reason, false, clients);
        }
        public void Kick(string reason, bool failOnClientError, params Client[] clients)
        {
            Library.Api.ClientsKickFromServer(clients, reason, failOnClientError);
        }

        public void Move(Channel newChannel, params Client[] clients)
        {
            Library.Api.ClientMove(newChannel, clients);
        }

        public Client GetClient(string uuid)
        {
            List<Client> result = GetClients(uuid);
            return result.Count == 1 ? result[0] : null;
        }

        public List<Client> GetClients(params string[] uuids)
        {
            return Library.Api.GetClientIDSfromUIDS(this, uuids);
        }

        public void FlushVariables()
        {
            Library.Api.FlushVirtualServerVariable(this);
        }

        private int GetInt(VirtualServerProperty flag)
        {
            return Library.Api.GetVirtualServerVariableAsInt(this, flag);
        }
        private ulong GetUInt64(VirtualServerProperty flag)
        {
            return Library.Api.GetVirtualServerVariableAsUInt64(this, flag);
        }
        private string GetString(VirtualServerProperty flag)
        {
            return Library.Api.GetVirtualServerVariableAsString(this, flag);
        }

        private void SetInt(VirtualServerProperty flag, int value)
        {
            bool autoFlush = Library.AutoFlush;
            Library.Api.SetVirtualServerVariableAsInt(this, flag, value);
            if (autoFlush) FlushVariables();
        }
        private void SetUInt64(VirtualServerProperty flag, ulong value)
        {
            bool autoFlush = Library.AutoFlush;
            Library.Api.SetVirtualServerVariableAsUInt64(this, flag, value);
            if (autoFlush) FlushVariables();
        }
        private void SetString(VirtualServerProperty flag, string value)
        {
            bool autoFlush = Library.AutoFlush;
            Library.Api.SetVirtualServerVariableAsString(this, flag, value);
            if (autoFlush) FlushVariables();
        }

        internal protected virtual void OnVoiceData(Client client, byte[] voiceData, uint frequency)
        {
            VoiceData?.Invoke(client, voiceData, frequency);
        }

        internal protected virtual void OnClientStartTalking(Client client)
        {
            ClientStartTalking?.Invoke(client);
        }

        internal protected virtual void OnClientStopTalking(Client client)
        {
            ClientStopTalking?.Invoke(client);

        }

        internal protected virtual void OnClientConnected(Client client, Channel channel, ref bool removeClientError)
        {
            ClientConnected?.Invoke(client, channel, ref removeClientError);
        }

        internal protected virtual void OnClientDisconnected(Client client, Channel channel)
        {
            ClientDisconnected?.Invoke(client, channel);
        }

        internal protected virtual void OnClientMoved(Client client, Channel oldChannel, Channel newChannel)
        {
            ClientMoved?.Invoke(client, oldChannel, newChannel);
        }

        internal protected virtual void OnChannelCreated(Client invoker, Channel channel)
        {
            ChannelCreated?.Invoke(invoker, channel);
        }

        internal protected virtual void OnChannelEdited(Client invoker, Channel channel)
        {
            ChannelEdited?.Invoke(invoker, channel);
        }

        internal protected virtual void OnChannelDeleted(Client invoker, Channel channel)
        {
            ChannelDeleted?.Invoke(invoker, channel);
        }

        internal protected virtual void OnServerTextMessage(Client invoker, string textMessage)
        {
            ServerTextMessage?.Invoke(invoker, textMessage);
        }

        internal protected virtual void OnChannelTextMessage(Client invoker, Channel target, string textMessage)
        {
            ChannelTextMessage?.Invoke(invoker, target, textMessage);
        }

        internal protected virtual void OnAccountingError(Error errorCode)
        {
            AccountingError?.Invoke(errorCode);
        }

        internal protected virtual void OnTransformFilePath(TransformFilePathEventArgs args)
        {
            TransformFilePath?.Invoke(args);
        }
    }

    internal class Cache
    {
        private readonly VirtualServer _virtualServer;

        private ConcurrentDictionary<ulong, Channel> _channelsCache;
        private ConcurrentDictionary<ushort, Client> _clientsCache;

        public Cache(VirtualServer virtualServer)
        {
            _virtualServer = virtualServer;
            _channelsCache = new ConcurrentDictionary<ulong, Channel>();
            _clientsCache = new ConcurrentDictionary<ushort, Client>();
        }

        public Client AddorGetClient(ushort id)
        {
            Client client;
            if (_clientsCache.TryGetValue(id, out client))
                return client;

            client = new Client(_virtualServer, id);
            bool added = _clientsCache.TryAdd(id, client);
            return added ? client : null;
        }

        public Client GetClient(ushort id)
        {
            if (id == 0)
            {
                // id 0 refers to the server itself, which has no Client representation.
                return null;
            }
            Client client;
            if (_clientsCache.TryGetValue(id, out client))
            {
                return client;
            }
            throw null;
        }

        public Channel GetChannel(ulong id)
        {
            Channel channel;
            if (_channelsCache.TryGetValue(id, out channel))
            {
                return channel;
            }
            return null;
        }

        public Channel AddChannel(ulong id)
        {
            var channel = new Channel(_virtualServer, id);
            if (_channelsCache.TryAdd(id, channel))
            {
                return channel;
            }
            return null;
        }

        public Channel RemoveChannel(ulong id)
        {
            Channel channel;
            if (_channelsCache.TryRemove(id, out channel))
            {
                return channel;
            }
            return null;
        }
    }
}