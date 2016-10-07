namespace TeamSpeak.Sdk
{
    internal enum ConnectionProperty
    {
        /// <summary>
        /// average latency for a round trip through and back this connection
        /// </summary>
        Ping = 0,
        /// <summary>
        /// standard deviation of the above average latency
        /// </summary>
        PingDeviation,
        /// <summary>
        /// how long the  exists already
        /// </summary>
        ConnectedTime,
        /// <summary>
        /// how long since the last action of this client
        /// </summary>
        IdleTime,
        /// <summary>
        /// IP of this client (as seen from the server side)
        /// </summary>
        ClientIp,
        /// <summary>
        /// Port of this client (as seen from the server side)
        /// </summary>
        ClientPort,
        /// <summary>
        /// IP of the server (seen from the client side) - only available on yourself, not for remote clients, not available server side
        /// </summary>
        ServerIp,
        /// <summary>
        /// Port of the server (seen from the client side) - only available on yourself, not for remote clients, not available server side
        /// </summary>
        ServerPort,
        /// <summary>
        /// how many Speech packets were sent through this connection
        /// </summary>
        PacketsSentSpeech,
        PacketsSentKeepalive,
        PacketsSentControl,
        /// <summary>
        /// how many packets were sent totally (this is PACKETSSentSpeech + packetsSentKeepalive + packetsSentControl)
        /// </summary>
        PacketsSentTotal,
        BytesSentSpeech,
        BytesSentKeepalive,
        BytesSentControl,
        BytesSentTotal,
        PacketsReceivedSpeech,
        PacketsReceivedKeepalive,
        PacketsReceivedControl,
        PacketsReceivedTotal,
        BytesReceivedSpeech,
        BytesReceivedKeepalive,
        BytesReceivedControl,
        BytesReceivedTotal,
        PacketlossSpeech,
        PacketlossKeepalive,
        PacketlossControl,
        /// <summary>
        /// the probability with which a packet round trip failed because a packet was lost
        /// </summary>
        PacketlossTotal,
        /// <summary>
        /// the probability with which a speech packet failed from the server to the client
        /// </summary>
        Server2clientPacketlossSpeech,
        Server2clientPacketlossKeepalive,
        Server2clientPacketlossControl,
        Server2clientPacketlossTotal,
        Client2serverPacketlossSpeech,
        Client2serverPacketlossKeepalive,
        Client2serverPacketlossControl,
        Client2serverPacketlossTotal,
        /// <summary>
        /// how many bytes of speech packets we sent during the last second
        /// </summary>
        BandwidthSentLastSecondSpeech,
        BandwidthSentLastSecondKeepalive,
        BandwidthSentLastSecondControl,
        BandwidthSentLastSecondTotal,
        /// <summary>
        /// how many bytes/s of speech packets we sent in average during the last minute
        /// </summary>
        BandwidthSentLastMinuteSpeech,
        BandwidthSentLastMinuteKeepalive,
        BandwidthSentLastMinuteControl,
        BandwidthSentLastMinuteTotal,
        BandwidthReceivedLastSecondSpeech,
        BandwidthReceivedLastSecondKeepalive,
        BandwidthReceivedLastSecondControl,
        BandwidthReceivedLastSecondTotal,
        BandwidthReceivedLastMinuteSpeech,
        BandwidthReceivedLastMinuteKeepalive,
        BandwidthReceivedLastMinuteControl,
        BandwidthReceivedLastMinuteTotal,
        /// <summary>
        /// how many bytes per second are currently being sent by file transfers
        /// </summary>
        FiletransferBandwidthSent = 62,
        /// <summary>
        /// how many bytes per second are currently being received by file transfers
        /// </summary>
        FiletransferBandwidthReceived = 63,
        /// <summary>
        /// how many bytes we received in total through file transfers
        /// </summary>
        FiletransferBytesReceivedTotal = 64,
        /// <summary>
        /// how many bytes we sent in total through file transfers
        /// </summary>
        FiletransferBytesSentTotal = 65,
    };
}
