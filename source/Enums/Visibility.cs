namespace Teamspeak.Sdk
{
    /// <summary>
    /// Visibility of a client
    /// </summary>
    public enum Visibility
    {
        /// <summary>
        /// Client came into view
        /// </summary>
        Enter = 0,
        /// <summary>
        /// Client remained in view
        /// </summary>
        Retain,
        /// <summary>
        /// Client left view
        /// </summary>
        Leave,
    }
}