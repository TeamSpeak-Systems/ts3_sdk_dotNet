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
    }
}