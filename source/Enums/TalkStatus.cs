namespace TeamSpeak.Sdk
{
    /// <summary>
    /// State of the <see cref="Client"/> voice transmission
    /// </summary>
    public enum TalkStatus : ushort
    {
        /// <summary>
        /// No voice data is being transmitted.
        /// </summary>
        NotTalking = 0,
        /// <summary>
        /// Voice data is being transmitted.
        /// </summary>
        Talking = 1,
        /// <summary>
        /// Voice data would be transmitted, but <see cref="Client.Client.IsInputDeactivated"/> is true.
        /// </summary>
        TalkingWhileDisabled = 2,
    }
}
