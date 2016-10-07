namespace TeamSpeak.Sdk
{
    internal enum ClientProperty
    {
        /// <summary>
        /// automatically up-to-date for any  "in view", can be used to identify this particular  installation
        /// </summary>
        UniqueIdentifier = 0,
        /// <summary>
        /// automatically up-to-date for any  "in view"
        /// </summary>
        Nickname,
        /// <summary>
        /// for other s than ourself, this needs to be requested (=> requestVariables)
        /// </summary>
        Version,
        /// <summary>
        /// for other s than ourself, this needs to be requested (=> requestVariables)
        /// </summary>
        Platform,
        /// <summary>
        /// automatically up-to-date for any  that can be heard (in room / whisper)
        /// </summary>
        FlagTalking,
        /// <summary>
        /// automatically up-to-date for any  "in view", this s microphone mute status
        /// </summary>
        InputMuted,
        /// <summary>
        /// automatically up-to-date for any  "in view", this s headphones/speakers/mic combined mute status
        /// </summary>
        OutputMuted,
        /// <summary>
        /// automatically up-to-date for any  "in view", this s headphones/speakers only mute status
        /// </summary>
        OutputonlyMuted,
        /// <summary>
        /// automatically up-to-date for any  "in view", this s microphone hardware status (is the capture device opened?)
        /// </summary>
        InputHardware,
        /// <summary>
        /// automatically up-to-date for any  "in view", this s headphone/speakers hardware status (is the playback device opened?)
        /// </summary>
        OutputHardware,
        /// <summary>
        /// only usable for ourself, not propagated to the network
        /// </summary>
        InputDeactivated,
        /// <summary>
        /// internal use
        /// </summary>
        IdleTime,
        /// <summary>
        /// only usable for ourself, the default channel we used to connect on our last connection attempt
        /// </summary>
        DefaultChannel,
        /// <summary>
        /// internal use
        /// </summary>
        DefaultChannelPassword,
        /// <summary>
        /// internal use
        /// </summary>
        ServerPassword,
        /// <summary>
        /// automatically up-to-date for any  "in view", not used by TeamSpeak, free storage for sdk users
        /// </summary>
        MetaData,
        /// <summary>
        /// only make sense on the  side locally, "1" if this  is currently muted by us, "0" if he is not
        /// </summary>
        IsMuted,
        /// <summary>
        /// automatically up-to-date for any  "in view"
        /// </summary>
        IsRecording,
        /// <summary>
        /// internal use
        /// </summary>
        VolumeModificator,
        /// <summary>
        /// sign
        /// </summary>
        VersionSign,
        /// <summary>
        /// SDK use, not used by TeamSpeak. Hash is provided by an outside source. A channel will use the security salt + other  data to calculate a hash, which must be the same as the one provided here.
        /// </summary>
        SecurityHash,
    };

}
