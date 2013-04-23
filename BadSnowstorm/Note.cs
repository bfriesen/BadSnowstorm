namespace BadSnowstorm
{
    public struct Note
    {
        private readonly Tone _toneVal;
        private readonly Duration _durVal;

        public Note(Tone frequency, Duration time)
        {
            _toneVal = frequency;
            _durVal = time;
        }

        public Tone NoteTone { get { return _toneVal; } }
        public Duration NoteDuration { get { return _durVal; } }
    }
}