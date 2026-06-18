namespace TeamSpeak.Sdk
{
    /// <summary>
    /// Virtual Server Properties
    /// </summary>
    public enum VirtualServerProperty
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
        #region sdk_3_1
#if !TEAMSPEAK_SDK_3_0
        /// <summary>
        /// String.  Read/Write. Comma separated list of available ciphers to encrypt the connection. The server will use the first cipher in the list that is also listed in the \ref CLIENT_ENCRYPTION_CIPHERS of the connecting client. Clients will fail to connect if no match is found. Always available.
        /// </summary>
        /// <remarks>Available in SDK 3.1+.</remarks>
        EncryptionCiphers,
#endif
        #endregion sdk_3_1
#if !TEAMSPEAK_SDK_3_0 && !TEAMSPEAK_SDK_3_1
        /// <summary>
        /// String. Any resolvable address for the specific virtual server.
        /// </summary>
        /// <remarks>Available in SDK 3.5+.</remarks>
        Address,
        /// <summary>
        /// String. Read only. Signature of platform and version.
        /// </summary>
        /// <remarks>Available in SDK 3.5+.</remarks>
        VersionSign,
#endif
#if !TEAMSPEAK_SDK_3_0
        /// <summary>
        /// String.  Read only.  The path to the base directory used to store files transferred using file transfer. Available only on the server. Is set by \ref ts3server_enableFileManager
        /// </summary>
        /// <remarks>Available in SDK 3.1+.</remarks>
        Filebase = 24,
        /// <summary>
        /// UInt64.  Read/Write. Maximum traffic in bytes the server can use for file transfer downloads. Only available on request (\ref ts3client_requestServerVariables).
        /// </summary>
        /// <remarks>Available in SDK 3.1+.</remarks>
        MaxDownloadTotalBandwidth = 29,
        /// <summary>
        /// UInt64.  Read/Write. Maximum traffic in bytes the server can use for file transfer uploads. Only available on request (=> requestServerVariables)
        /// </summary>
        /// <remarks>Available in SDK 3.1+.</remarks>
        MaxUploadTotalBandwidth = 30,
        /// <summary>
        /// Integer. Read/Write. Boolean (1/0) indicating whether to include file transfer activities (uploading or downloading of files) in the server log. Always available.
        /// </summary>
        /// <remarks>Available in SDK 3.1+.</remarks>
        LogFiletransfer = 64
#endif
    };

}
