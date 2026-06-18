using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TeamSpeak.Sdk.Server
{
    /// <summary>
    /// Represents a <see cref="Channel"/> on a <see cref="Sdk.Server.VirtualServer"/>
    /// </summary>
    public class Channel
    {
        /// <summary>
        /// ID of the <see cref="Channel"/>
        /// </summary>
        public ulong ID { get; }
        /// <summary>
        /// The <see cref="Sdk.Server.VirtualServer"/> the <see cref="Channel"/> is located on.
        /// </summary>
        public VirtualServer VirtualServer { get; }

        // cached name and topic, so that they can survive the channels death
        private string CachedName;
        private string CachedTopic;

        /// <summary>
        /// Creates a new <see cref="Channel"/>-Object.
        /// </summary>
        /// <remarks>Does not create a new <see cref="Channel"/> on the <see cref="Sdk.Server.VirtualServer"/>. To create a new <see cref="Channel"/> use <see cref="VirtualServer.CreateChannel(string, Channel, string, string, string, Channel, bool?, bool?, CodecType?, int?, bool?, int?)"/></remarks>
        /// <param name="virtualServer">the <see cref="Sdk.Server.VirtualServer"/> the <see cref="Channel"/> is located on.</param>
        /// <param name="id">ID of the channel</param>
        public Channel(VirtualServer virtualServer, ulong id)
            : this(virtualServer, id, waitForProperties: false)
        {
        }

        internal Channel(VirtualServer virtualServer, ulong id, bool waitForProperties)
        {
            Sdk.Require.NotNull(nameof(virtualServer), virtualServer);
            ID = id;
            VirtualServer = virtualServer;
            RefreshProperties(waitForProperties);
        }

        /// <summary>
        /// The parent <see cref="Channel"/>
        /// </summary>
        public Channel Parent { get { return Library.Api.GetParentChannelOfChannel(this); } }
        /// <summary>
        /// List of all <see cref="Client">Clients</see> in the <see cref="Channel"/>.
        /// </summary>
        public ICollection<Client> Clients { get { return Library.Api.GetChannelClientList(this)?.AsReadOnly(); } }
        /// <summary>
        /// List of all subchannels in the <see cref="Channel"/>.
        /// </summary>
        public ReadonlyChannelCollection Channels { get { throw new NotImplementedException(); } }
        /// <summary>
        /// Get a <see cref="ChannelProperty"/> as a <see cref="string"/>
        /// </summary>
        /// <param name="property">the property</param>
        /// <returns>the property as a string</returns>
        public string this[ChannelProperty property]
        {
            get { return GetString(property); }
        }
        /// <summary>
        /// Name of the <see cref="Channel"/>.
        /// </summary>
        public string Name
        {
            get { return CachedName; }
            set
            {
                Sdk.Require.NotNull(nameof(value), value);
                SetString(ChannelProperty.Name, value);
            }
        }
        /// <summary>
        /// Single-line channel topic
        /// </summary>
        public string Topic
        {
            get { return CachedTopic; }
            set
            {
                Sdk.Require.NotNull(nameof(value), value);
                SetString(ChannelProperty.Topic, value);
            }
        }
        /// <summary>
        /// Optional channel description.
        /// </summary>
        /// <remarks>Can have multiple lines.</remarks>
        public string Description
        {
            get { return GetString(ChannelProperty.Description); }
            set
            {
                Sdk.Require.NotNull(nameof(value), value);
                SetString(ChannelProperty.Description, value);
            }
        }
        /// <summary>
        /// Optional password for password-protected channels.
        /// </summary>
        public string Password
        {
            get { return GetString(ChannelProperty.Password); }
            set
            {
                Sdk.Require.NotNull(nameof(value), value);
                SetString(ChannelProperty.Password, value);
            }
        }
        /// <summary>
        /// Codec used for this channel
        /// </summary>
        public CodecType Codec
        {
            get { return (CodecType)GetInt(ChannelProperty.Codec); }
            set { SetInt(ChannelProperty.Codec, (int)value); }
        }
        /// <summary>
        /// Quality of channel codec of this channel.
        /// </summary>
        /// <remarks>Valid values range from 0 to 10, default is 7. Higher values result in better speech quality but more bandwidth usage</remarks>
        public int CodecQuality
        {
            get { return GetInt(ChannelProperty.CodecQuality); }
            set { SetInt(ChannelProperty.CodecQuality, value); }
        }
        /// <summary>
        /// Number of maximum clients who can join this channel
        /// </summary>
        public int MaxClients
        {
            get { return GetInt(ChannelProperty.Maxclients); }
            set { SetInt(ChannelProperty.Maxclients, value); }
        }
        /// <summary>
        /// Number of maximum clients who can join this channel and all subchannels
        /// </summary>
        public int MaxFamilyClients
        {
            get { return GetInt(ChannelProperty.Maxfamilyclients); }
            set { SetInt(ChannelProperty.Maxfamilyclients, value); }
        }
        /// <summary>
        /// <see cref="Order"/> is the <see cref="Channel"/> after which this channel is sorted. <see langword="null"/> meaning its going to be the first <see cref="Channel"/> under <see cref="Parent"/>
        /// </summary>
        public Channel Order
        {
            get
            {
                int id = GetInt(ChannelProperty.Order);
                if (id < 0) throw new NotSupportedException("Reading the channel order is not supported.");
                return VirtualServer.Cache.GetChannel((ulong)id);
            }
            set
            {
                Require.SameVirtualServer(nameof(value), VirtualServer, value);
                ulong id = value?.ID ?? 0;
                if (id > int.MaxValue) throw new NotSupportedException("Setting the channel order is not supported.");
                SetInt(ChannelProperty.Order, (int)id);
            }
        }
        /// <summary>
        /// Permanent channels will be restored when the server restarts.
        /// </summary>
        public bool IsPermanent
        {
            get { return GetInt(ChannelProperty.FlagPermanent) != 0; }
            set { SetInt(ChannelProperty.FlagPermanent, value ? 1 : 0); }
        }
        /// <summary>
        /// Semi-permanent channels are not automatically deleted when the last user left but will not be restored when the server restarts.
        /// </summary>
        public bool IsSemiPermanent
        {
            get { return GetInt(ChannelProperty.FlagSemiPermanent) != 0; }
            set { SetInt(ChannelProperty.FlagSemiPermanent, value ? 1 : 0); }
        }
        /// <summary>
        /// Channel is the default channel.
        /// </summary>
        /// <remarks>
        /// There can only be one default channel per server.
        /// New users who did not configure a channel to join on login via <see cref="Sdk.Client.Connection.Start(string, string, uint, string, string, string, string)"/> 
        /// will automatically join the default channel.
        /// </remarks>
        public bool IsDefault
        {
            get { return GetInt(ChannelProperty.FlagDefault) != 0; }
            set { SetInt(ChannelProperty.FlagDefault, value ? 1 : 0); }
        }
        /// <summary>
        /// If set, channel is password protected. The password itself is stored in ChannelPassword
        /// </summary>
        public bool IsPasswordProtected
        {
            get { return GetInt(ChannelProperty.FlagPassword) != 0; }
        }
        /// <summary>
        /// Latency of this channel.
        /// </summary>
        /// <remarks>
        /// Allows to increase the packet size resulting in less bandwidth usage at the cost of higher latency. A value of 1 (default) is the best setting for lowest latency and best quality. If bandwidth or network quality are restricted, increasing the latency factor can help stabilize the connection. Higher latency values are only possible for low-quality codec and codec quality settings.
        /// </remarks>
        public int CodecLatencyFactor
        {
            get { return GetInt(ChannelProperty.CodecLatencyFactor); }
            set { SetInt(ChannelProperty.CodecLatencyFactor, value); }
        }
        /// <summary>
        /// If <see langword="true"/>, this channel is not using encrypted voice data. If <see langword="false"/>, voice data is encrypted for this channel.
        /// </summary>
        /// <remarks>
        /// Note that channel voice data encryption can be globally disabled or enabled for the virtual server. Changing this flag makes only sense if global voice data encryption is set to be configured per channel.
        /// </remarks>
        public bool CodecIsUnencrypted
        {
            get { return GetInt(ChannelProperty.CodecIsUnencrypted) != 0; }
            set { SetInt(ChannelProperty.CodecIsUnencrypted, value ? 1 : 0); }
        }
        /// <summary>
        /// This parameter defines how many seconds the server waits until a temporary channel is deleted when empty.
        /// When a temporary channel is created, a timer is started. If a user joins the channel before the countdown is finished, the channel is not deleted. After the last person has left the channel, the countdown starts again. DeleteDelay defines the length of this countdown in seconds.
        /// </summary>
        public TimeSpan DeleteDelay
        {
            get { return TimeSpan.FromSeconds(GetInt(ChannelProperty.DeleteDelay)); }
            set
            {
                double seconds = value.TotalSeconds;
                try
                {
                    SetInt(ChannelProperty.DeleteDelay, Convert.ToInt32(seconds));
                }
                catch (OverflowException)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "TotalSeconds must fit into a Int32");
                }
            }
        }

        public void Delete(bool force = false)
        {
            Library.Api.ChannelDelete(this, force);
        }

        public void MoveTo(Channel newParent, Channel newChannelOrder = null)
        {
            Require.SameVirtualServer(nameof(newParent), VirtualServer, newParent);
            Require.SameVirtualServer(nameof(newChannelOrder), VirtualServer, newChannelOrder);
            Library.Api.ChannelMove(this, newParent, newChannelOrder);
        }

        public void FlushVariables()
        {
            Library.Api.FlushChannelVariable(this);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            string result = Name;
            if (string.IsNullOrEmpty(result) == false)
                return result;
            else return base.ToString();
        }

        private int GetInt(ChannelProperty flag)
        {
            return Library.Api.GetChannelVariableAsInt(this, flag);
        }

        private string GetString(ChannelProperty flag)
        {
            return Library.Api.GetChannelVariableAsString(this, flag);
        }

        private void SetInt(ChannelProperty flag, int value)
        {
            bool autoFlush = Library.AutoFlush;
            Library.Api.SetChannelVariableAsInt(this, flag, value);
            if (autoFlush) FlushVariables();
        }

        private void SetString(ChannelProperty flag, string value)
        {
            bool autoFlush = Library.AutoFlush;
            Library.Api.SetChannelVariableAsString(this, flag, value);
            if (autoFlush) FlushVariables();
        }

        internal void RefreshProperties(bool wait)
        {
            if (ID == 0) return;
            if (Library.Api.TryGetChannelVariableAsString(this, ChannelProperty.Name, out CachedName) != Error.Ok && wait)
            {
                System.Diagnostics.Stopwatch timer = System.Diagnostics.Stopwatch.StartNew();
                Error error;
                do Thread.Yield();
                while ((error = Library.Api.TryGetChannelVariableAsString(this, ChannelProperty.Name, out CachedName)) != Error.Ok && timer.ElapsedMilliseconds < 25);
                System.Diagnostics.Debug.Assert(error == Error.Ok);
            }
            Library.Api.TryGetChannelVariableAsString(this, ChannelProperty.Topic, out CachedTopic);
        }
    }
}