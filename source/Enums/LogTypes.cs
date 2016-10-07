using System;
namespace Teamspeak.Sdk
{
    /// <summary>
    /// The different logging mechanisms.
    /// </summary>
    [Flags]
    public enum LogTypes
    {
        /// <summary>
        /// No logging-mechanism
        /// </summary>
        None = 0x0000,
        /// <summary>
        /// Log into file
        /// </summary>
        File = 0x0001,
        /// <summary>
        /// Log to console
        /// </summary>
        Console = 0x0002,
        /// <summary>
        /// Enable user-defined-logging
        /// </summary>
        Userlogging = 0x0004,
    };
}
