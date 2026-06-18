namespace TeamSpeak.Sdk
{
    /// <summary>
    /// Voice Activitiy Detection Mode
    /// </summary>
    public enum DenoiseLevel
    {
        /// <summary>
        /// low amount of denoising
        /// </summary>
        Low = 0,
        /// <summary>
        /// moderate amount of denoising, default
        /// </summary>
        Moderate,
        /// <summary>
        /// high amount of denoising
        /// </summary>
        High,
        /// <summary>
        /// very high amount of denoising
        /// </summary>
        VeryHigh
    };
}
