using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamSpeak.Sdk
{
    /// <summary>
    /// Platforms the SDK can run on
    /// </summary>
    public enum SupportedPlatform
    {
#pragma warning disable 1591
        Android,
        iOS,
        MacOSX,
        Linux,
        Windows,
    }
#pragma warning restore
}
