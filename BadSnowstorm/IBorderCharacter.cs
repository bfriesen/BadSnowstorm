using System;

namespace BadSnowstorm
{
    public interface IBorderCharacter
    {
        char GetValue();
        Point Location { get; }
    }
}
