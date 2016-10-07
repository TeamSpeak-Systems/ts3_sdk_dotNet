namespace TeamSpeak.Sdk
{
    internal enum ChannelProperty
    {
        /// <summary>
        /// Available for all channels that are "in view", always up-to-date
        /// </summary>
        Name = 0,
        /// <summary>
        /// Available for all channels that are "in view", always up-to-date
        /// </summary>
        Topic,
        /// <summary>
        /// Must be requested (=> requestChannelDescription)
        /// </summary>
        Description,
        /// <summary>
        /// not available client side
        /// </summary>
        Password,
        /// <summary>
        /// Available for all channels that are "in view", always up-to-date
        /// </summary>
        Codec,
        /// <summary>
        /// Available for all channels that are "in view", always up-to-date
        /// </summary>
        CodecQuality,
        /// <summary>
        /// Available for all channels that are "in view", always up-to-date
        /// </summary>
        Maxclients,
        /// <summary>
        /// Available for all channels that are "in view", always up-to-date
        /// </summary>
        Maxfamilyclients,
        /// <summary>
        /// Available for all channels that are "in view", always up-to-date
        /// </summary>
        Order,
        /// <summary>
        /// Available for all channels that are "in view", always up-to-date
        /// </summary>
        FlagPermanent,
        /// <summary>
        /// Available for all channels that are "in view", always up-to-date
        /// </summary>
        FlagSemiPermanent,
        /// <summary>
        /// Available for all channels that are "in view", always up-to-date
        /// </summary>
        FlagDefault,
        /// <summary>
        /// Available for all channels that are "in view", always up-to-date
        /// </summary>
        FlagPassword,
        /// <summary>
        /// Available for all channels that are "in view", always up-to-date
        /// </summary>
        CodecLatencyFactor,
        /// <summary>
        /// Available for all channels that are "in view", always up-to-date
        /// </summary>
        CodecIsUnencrypted,
        /// <summary>
        /// Not available client side, not used in TeamSpeak, only SDK. Sets the options+salt for security hash.
        /// </summary>
        SecuritySalt,
        /// <summary>
        /// How many seconds to wait before deleting this channel
        /// </summary>
        DeleteDelay,
    }
}
