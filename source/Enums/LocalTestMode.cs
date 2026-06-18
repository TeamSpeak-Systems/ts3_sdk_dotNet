namespace TeamSpeak.Sdk
{
    /// <summary>
    /// Modes to test the local capture device.
    /// </summary>
    public enum LocalTestMode
    {
        /// <summary>
        /// LocalTestMode is disabled.
        /// </summary>
        Off = 0,
        /// <summary>
        /// Only the local capture device is routed to playback.
        /// </summary>
        LocalVoice = 1,
        /// <summary>
        /// Both the local capture device and remote voices are routed to playblack.
        /// </summary>
        LocalAndRemoteVoice = 2,
        #region sdk_3_1
#if !TEAMSPEAK_SDK_3_0
        /// <summary>
        /// Don't produce playback, just talk status changes.
        /// </summary>
        /// <remarks>Available in SDK 3.1+.</remarks>
        TalkStatusChangesOnly = 3
#endif
        #endregion sdk_3_1
    }
}