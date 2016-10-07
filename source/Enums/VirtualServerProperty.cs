namespace TeamSpeak.Sdk
{
    internal enum VirtualServerProperty
    {
        /// <summary>
        /// available when connected, can be used to identify this particular server installation
        /// </summary>
        UniqueIdentifier = 0,
        /// <summary>
        /// available and always up-to-date when connected
        /// </summary>
        Name,
        /// <summary>
        /// available when connected,  (=> requestServerVariables)
        /// </summary>
        Welcomemessage,
        /// <summary>
        /// available when connected
        /// </summary>
        Platform,
        /// <summary>
        /// available when connected
        /// </summary>
        Version,
        /// <summary>
        /// only available on request (=> requestServerVariables), stores the maximum number of clients that may currently join the server
        /// </summary>
        Maxclients,
        /// <summary>
        /// not available to clients, the server password
        /// </summary>
        Password,
        /// <summary>
        /// only available on request (=> requestServerVariables),
        /// </summary>
        ClientsOnline,
        /// <summary>
        /// only available on request (=> requestServerVariables),
        /// </summary>
        ChannelsOnline,
        /// <summary>
        /// available when connected, stores the time when the server was created
        /// </summary>
        Created,
        /// <summary>
        /// only available on request (=> requestServerVariables), the time since the server was started
        /// </summary>
        Uptime,
        /// <summary>
        /// available and always up-to-date when connected
        /// </summary>
        CodecEncryptionMode,
    };

}
