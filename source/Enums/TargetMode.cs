namespace TeamSpeak.Sdk
{
    /// <summary>
    /// Determines the kind of entity a text message will be sent to
    /// </summary>
    internal enum TargetMode : ushort
    {
        /// <summary>
        /// Send a text message to a client
        /// </summary>
        Client = 1,
        /// <summary>
        /// Send a text message to a channel chat
        /// </summary>
        Channel,
        /// <summary>
        /// Send a text message to the server chat
        /// </summary>
        Server
    }
}
