using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TeamSpeak.Sdk.Server
{
    /// <summary>
    /// Information about the <see cref="Sdk.Server.VirtualServer"/>
    /// </summary>
    public class ConnectionInfo
    {
        /// <summary>
        /// How many bytes per second are currently being sent by file transfers
        /// </summary>
        public ulong FiletransferBandwidthSent
        {
            get { return GetUInt64(ConnectionProperty.FiletransferBandwidthSent); }
        }

        /// <summary>
        /// How many bytes per second are currently being received by file transfers
        /// </summary>
        public ulong FiletransferBandwidthReceived
        {
            get { return GetUInt64(ConnectionProperty.FiletransferBandwidthReceived); }
        }
        /// <summary>
        /// Average latency for a round trip through and back this connection.
        /// </summary>
        public TimeSpan Ping
        {
            get { return TimeSpan.FromMilliseconds(GetUInt64(ConnectionProperty.Ping)); }
        }

        /// <summary>
        /// Standard deviation of the above average latency
        /// </summary>
        public double PingDeviation
        {
            get { return GetDouble(ConnectionProperty.PingDeviation); }
        }

        /// <summary>
        /// How many Speech packets were sent through this connection
        /// </summary>
        public ulong PacketsSentSpeech
        {
            get { return GetUInt64(ConnectionProperty.PacketsSentSpeech); }
        }
        /// <summary>
        /// How many Keepalive packets were sent through this connection
        /// </summary>
        public ulong PacketsSentKeepalive
        {
            get { return GetUInt64(ConnectionProperty.PacketsSentKeepalive); }
        }
        /// <summary>
        /// How many Control packets were sent through this connection
        /// </summary>
        public ulong PacketsSentControl
        {
            get { return GetUInt64(ConnectionProperty.PacketsSentControl); }
        }
        /// <summary>
        /// How many packets were sent totally (this is <see cref="PacketsSentSpeech"/> + <see cref="PacketsSentKeepalive"/> + <see cref="PacketsSentControl"/>)
        /// </summary>
        public ulong PacketsSentTotal
        {
            get { return GetUInt64(ConnectionProperty.PacketsSentTotal); }
        }
        /// <summary>
        /// How many bytes worth of Speech packets were sent through this connection
        /// </summary>
        public ulong BytesSentSpeech
        {
            get { return GetUInt64(ConnectionProperty.BytesSentSpeech); }
        }
        /// <summary>
        /// How many bytes worth of Keepalive packets were sent through this connection
        /// </summary>
        public ulong BytesSentKeepalive
        {
            get { return GetUInt64(ConnectionProperty.BytesSentKeepalive); }
        }
        /// <summary>
        /// How many bytes worth of Sent packets were sent through this connection
        /// </summary>
        public ulong BytesSentControl
        {
            get { return GetUInt64(ConnectionProperty.BytesSentControl); }
        }
        /// <summary>
        /// How many bytes were sent through this connection
        /// </summary>
        public ulong BytesSentTotal
        {
            get { return GetUInt64(ConnectionProperty.BytesSentTotal); }
        }
        /// <summary>
        /// How many Speech packets were received through this connection
        /// </summary>
        public ulong PacketsReceivedSpeech
        {
            get { return GetUInt64(ConnectionProperty.PacketsReceivedSpeech); }
        }
        /// <summary>
        /// How many Keepalive packets were received through this connection
        /// </summary>
        public ulong PacketsReceivedKeepalive
        {
            get { return GetUInt64(ConnectionProperty.PacketsReceivedKeepalive); }
        }
        /// <summary>
        /// How many Control packets were received through this connection
        /// </summary>
        public ulong PacketsReceivedControl
        {
            get { return GetUInt64(ConnectionProperty.PacketsReceivedControl); }
        }
        /// <summary>
        /// How many packets were received through this connection
        /// </summary>
        public ulong PacketsReceivedTotal
        {
            get { return GetUInt64(ConnectionProperty.PacketsReceivedTotal); }
        }
        /// <summary>
        /// How many bytes worth of Speech packets were received through this connection
        /// </summary>
        public ulong BytesReceivedSpeech
        {
            get { return GetUInt64(ConnectionProperty.BytesReceivedSpeech); }
        }
        /// <summary>
        /// How many bytes worth of Keepalive packets were received through this connection
        /// </summary>
        public ulong BytesReceivedKeepalive
        {
            get { return GetUInt64(ConnectionProperty.BytesReceivedKeepalive); }
        }
        /// <summary>
        /// How many bytes worth of Control packets were received through this connection
        /// </summary>
        public ulong BytesReceivedControl
        {
            get { return GetUInt64(ConnectionProperty.BytesReceivedControl); }
        }
        /// <summary>
        /// How many bytes were received through this connection
        /// </summary>
        public ulong BytesReceivedTotal
        {
            get { return GetUInt64(ConnectionProperty.BytesReceivedTotal); }
        }
        
        /// <summary>
        /// The probability with which speech packet round trip failed because a packet was lost
        /// </summary>
        public double PacketlossSpeech
        {
            get { return GetDouble(ConnectionProperty.PacketlossSpeech); }
        }
        
        /// <summary>
        /// The probability with which keepalive packet round trip failed because a packet was lost
        /// </summary>
        public double PacketlossKeepalive
        {
            get { return GetDouble(ConnectionProperty.PacketlossKeepalive); }
        }
        
        /// <summary>
        /// The probability with which control packet round trip failed because a packet was lost
        /// </summary>
        public double PacketlossControl
        {
            get { return GetDouble(ConnectionProperty.PacketlossControl); }
        }
        
        /// <summary>
        /// The probability with which a packet round trip failed because a packet was lost
        /// </summary>
        public double PacketlossTotal
        {
            get { return GetDouble(ConnectionProperty.PacketlossTotal); }
        }

        /// <summary>
        /// How many bytes of speech packets we sent during the last second
        /// </summary>
        public double BandwidthSentLastSecondSpeech
        {
            get { return GetDouble(ConnectionProperty.BandwidthSentLastSecondSpeech); }
        }
        
        /// <summary>
        /// How many bytes of keepalive packets we sent during the last second
        /// </summary>
        public double BandwidthSentLastSecondKeepalive
        {
            get { return GetDouble(ConnectionProperty.BandwidthSentLastSecondKeepalive); }
        }

        /// <summary>
        /// How many bytes of control packets we sent during the last second
        /// </summary>
        public double BandwidthSentLastSecondControl
        {
            get { return GetDouble(ConnectionProperty.BandwidthSentLastSecondControl); }
        }

        /// <summary>
        /// How many bytes of packets we sent during the last second
        /// </summary>
        public double BandwidthSentLastSecondTotal
        {
            get { return GetDouble(ConnectionProperty.BandwidthSentLastSecondTotal); }
        }

        /// <summary>
        /// How many bytes/s of speech packets we sent in average during the last minute
        /// </summary>
        public double BandwidthSentLastMinuteSpeech
        {
            get { return GetDouble(ConnectionProperty.BandwidthSentLastMinuteSpeech); }
        }

        /// <summary>
        /// How many bytes/s of keepalive packets we sent in average during the last minute
        /// </summary>
        public double BandwidthSentLastMinuteKeepalive
        {
            get { return GetDouble(ConnectionProperty.BandwidthSentLastMinuteKeepalive); }
        }

        /// <summary>
        /// How many bytes/s of control packets we sent in average during the last minute
        /// </summary>
        public double BandwidthSentLastMinuteControl
        {
            get { return GetDouble(ConnectionProperty.BandwidthSentLastMinuteControl); }
        }

        /// <summary>
        /// How many bytes/s of packets we received in average during the last minute
        /// </summary>
        public double BandwidthSentLastMinuteTotal
        {
            get { return GetDouble(ConnectionProperty.BandwidthSentLastMinuteTotal); }
        }

        /// <summary>
        /// How many bytes/s of Speech packets we received in average during the last second
        /// </summary>
        public double BandwidthReceivedLastSecondSpeech
        {
            get { return GetDouble(ConnectionProperty.BandwidthReceivedLastSecondSpeech); }
        }

        /// <summary>
        /// How many bytes/s of Keepalive packets we received in average during the last second
        /// </summary>
        public double BandwidthReceivedLastSecondKeepalive
        {
            get { return GetDouble(ConnectionProperty.BandwidthReceivedLastSecondKeepalive); }
        }

        /// <summary>
        /// How many bytes/s of Control packets we received in average during the last second
        /// </summary>
        public double BandwidthReceivedLastSecondControl
        {
            get { return GetDouble(ConnectionProperty.BandwidthReceivedLastSecondControl); }
        }

        /// <summary>
        /// How many bytes/s of packets we received in average during the last second
        /// </summary>
        public double BandwidthReceivedLastSecondTotal
        {
            get { return GetDouble(ConnectionProperty.BandwidthReceivedLastSecondTotal); }
        }

        /// <summary>
        /// How many bytes/s of Speed packets we received in average during the last minute
        /// </summary>
        public double BandwidthReceivedLastMinuteSpeech
        {
            get { return GetDouble(ConnectionProperty.BandwidthReceivedLastMinuteSpeech); }
        }

        /// <summary>
        /// How many bytes/s of Keepalive packets we received in average during the last minute
        /// </summary>
        public double BandwidthReceivedLastMinuteKeepalive
        {
            get { return GetDouble(ConnectionProperty.BandwidthReceivedLastMinuteKeepalive); }
        }

        /// <summary>
        /// How many bytes/s of Control packets we received in average during the last minute
        /// </summary>
        public double BandwidthReceivedLastMinuteControl
        {
            get { return GetDouble(ConnectionProperty.BandwidthReceivedLastMinuteControl); }
        }

        /// <summary>
        /// How many bytes/s of packets we received in average during the last minute
        /// </summary>
        public double BandwidthReceivedLastMinuteTotal
        {
            get { return GetDouble(ConnectionProperty.BandwidthReceivedLastMinuteTotal); }
        }

        /// <summary>
        /// The <see cref="VirtualServer"/>
        /// </summary>
        public VirtualServer VirtualServer { get; }

        /// <summary>
        /// Creates a new <see cref="ConnectionInfo"/>-Object
        /// </summary>
        /// <param name="virtualServer">The <see cref="VirtualServer"/> for which the <see cref="ConnectionInfo"/> is requested</param>
        public ConnectionInfo(VirtualServer virtualServer)
        {
            Sdk.Require.NotNull(nameof(virtualServer), virtualServer);
            VirtualServer = virtualServer;
        }

        private ulong GetUInt64(ConnectionProperty flag)
        {
            return Library.Api.GetVirtualServerConnectionVariableAsUInt64(VirtualServer, flag);
        }
        private double GetDouble(ConnectionProperty flag)
        {
            return Library.Api.GetVirtualServerConnectionVariableAsDouble(VirtualServer, flag);
        }
    }
}
