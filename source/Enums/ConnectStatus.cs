namespace Teamspeak.Sdk
{
    /// <summary>
    /// Status of the <see cref="Client.Connection"/>
    /// </summary>
    public enum ConnectStatus
    {
        /// <summary>
        /// There is no activity to the server, this is the default value
        /// </summary>
        Disconnected = 0,
        /// <summary>
        /// We are trying to connect, we haven't got a clientID yet, we haven't been accepted by the server
        /// </summary>
        Connecting,
        /// <summary>
        /// The server has accepted us, we can talk and hear and we got a clientID, but we don't have the channels and clients yet, we can get server infos (welcome message etc.)
        /// </summary>
        Connected,
        /// <summary>
        /// we are CONNECTED and we are visible
        /// </summary>
        ConnectionEstablishing,
        /// <summary>
        /// we are CONNECTED and we have the client and channels available
        /// </summary>
        ConnectionEstablished,
    };

}
