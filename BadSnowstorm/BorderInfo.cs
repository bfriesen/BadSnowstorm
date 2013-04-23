using System;

namespace BadSnowstorm
{
    [Flags]
    public enum BorderInfo
    {
        SingleLeft =    0x01,
        SingleRight =   0x02,
        SingleTop =     0x04,
        SingleBottom =  0x08,
        DoubleLeft =    0x10,
        DoubleRight =   0x20,
        DoubleTop =     0x40,
        DoubleBottom =  0x80,
    }
}