namespace TeamSpeak.Sdk
{
    /// <summary>
    /// The mode of a <see cref="Client.FileTransfer"/>
    /// </summary>
    public enum TransferMode
    {
        /// <summary>
        /// A file is being downloaded
        /// </summary>
        Download = 0,
        /// <summary>
        /// A file is being uploaded
        /// </summary>
        Upload = 1,
    };
}
