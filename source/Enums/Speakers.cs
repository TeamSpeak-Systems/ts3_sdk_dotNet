using System;
namespace Teamspeak.Sdk
{
    /// <summary>
    /// The speaker a channel is mapped to.
    /// </summary>
    [Flags]
    public enum Speakers: uint
    {
#pragma warning disable 1591
        FrontLeft = 0x1,
        FrontRight = 0x2,
        FrontCenter = 0x4,
        LowFrequency = 0x8,
        BackLeft = 0x10,
        BackRight = 0x20,
        FrontLeftOfCenter = 0x40,
        FrontRightOfCenter = 0x80,
        BackCenter = 0x100,
        SideLeft = 0x200,
        SideRight = 0x400,
        TopCenter = 0x800,
        TopFrontLeft = 0x1000,
        TopFrontCenter = 0x2000,
        TopFrontRight = 0x4000,
        TopBackLeft = 0x8000,
        TopBackCenter = 0x10000,
        TopBackRight = 0x20000,
        HeadphonesLeft = 0x10000000,
        HeadphonesRight = 0x20000000,
        Mono = 0x40000000,
#pragma warning restore
    }
}
