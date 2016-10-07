namespace TeamSpeak.Sdk
{
    /// <summary>
    /// State of the <see cref="Client.FileTransfer"/>
    /// </summary>
    public enum FileTransferState : ushort
    {
        /// <summary>
        /// Transfer is being initialized.
        /// </summary>
        Initializing = 0,
        /// <summary>
        /// File is being transfered.
        /// </summary>
        Active,
        /// <summary>
        /// Transfer finished.
        /// </summary>
        Finished,
    }
}
