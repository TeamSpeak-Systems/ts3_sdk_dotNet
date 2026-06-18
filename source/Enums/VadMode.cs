namespace TeamSpeak.Sdk
{
    /// <summary>
    /// Voice Activitiy Detection Mode
    /// </summary>
    public enum VoiceActivityDetectionMode
    {
        /// <summary>
        /// based on neural networking
        /// </summary>
        Likelihood = 0,
        /// <summary>
        /// everything that is really bad, but not so bad we need to shut down
        /// </summary>
        Power,
        /// <summary>
        /// Combination of Likelihood and power. Both are required to pass.
        /// </summary>
        LikelihoodAndPower
    };

}
