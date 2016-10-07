namespace TeamSpeak.Sdk
{
    /// <summary>
    /// The severity of a log message
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// these messages stop the program
        /// </summary>
        Critical = 0,
        /// <summary>
        /// everything that is really bad, but not so bad we need to shut down
        /// </summary>
        Error,
        /// <summary>
        /// everything that *might* be bad
        /// </summary>
        Warning,
        /// <summary>
        /// output that might help find a problem
        /// </summary>
        Debug,
        /// <summary>
        /// informational output, like "starting database version x.y.z"
        /// </summary>
        Info,
        /// <summary>
        /// developer only output (will not be displayed in release mode)
        /// </summary>
        Devel
    };

}
