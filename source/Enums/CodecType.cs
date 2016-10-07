namespace TeamSpeak.Sdk
{
    /// <summary>
    /// Codec used to transmit voice data
    /// </summary>
    public enum CodecType
    {
        /// <summary>
        /// mono,   16bit,  8kHz, bitrate dependent on the quality setting
        /// </summary>
        SpeexNarrowband = 0,
        /// <summary>
        /// mono,   16bit, 16kHz, bitrate dependent on the quality setting
        /// </summary>
        SpeexWideband,
        /// <summary>
        /// mono,   16bit, 32kHz, bitrate dependent on the quality setting
        /// </summary>
        SpeexUltrawideband,
        /// <summary>
        /// mono,   16bit, 48kHz, bitrate dependent on the quality setting
        /// </summary>
        CeltMono,
        /// <summary>
        /// mono,   16bit, 48khz, bitrate dependent on the quality setting, optimized for voice
        /// </summary>
        OpusVoice,
        /// <summary>
        /// stereo, 16bit, 48khz, bitrate dependent on the quality setting, optimized for music
        /// </summary>
        OpusMusic,
    }
}
