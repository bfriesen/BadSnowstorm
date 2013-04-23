using System;

namespace BadSnowstorm
{
    public class Tempo
    {
        private readonly double multiplier;

        private Tempo(int bpm)
        {
            multiplier = 60.0 / bpm * 1000 / (int)Duration.Quarter;
        }

        public static Tempo FromBpm(int bpm)
        {
            return new Tempo(bpm);
        }

        public int GetDurationMilliseconds(Duration duration)
        {
            return Convert.ToInt32(multiplier * (int)duration);
        }
    }
}