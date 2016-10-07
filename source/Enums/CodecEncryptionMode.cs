namespace Teamspeak.Sdk
{
    /// <summary>
    /// Encryption mode used by the Teamspeak-Server
    /// </summary>
    public enum CodecEncryptionMode
    {
        /// <summary>
        /// Encryption is configured per <see cref="Client.Channel"/> using <see cref="Client.Channel.CodecIsUnencrypted"/>
        /// </summary>
        PerChannel = 0,
        /// <summary>
        /// Encryption is forced off server-wide.
        /// </summary>
        ForcedOff,
        /// <summary>
        /// Encryption is forced on server-wide.
        /// </summary>
        ForcedOn,
    }
}
