namespace BadSnowstorm
{
    public struct Padding
    {
        public readonly int Left;
        public readonly int Right;
        public readonly int Top;
        public readonly int Bottom;

        public Padding(int left, int right, int top, int bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        public override string ToString()
        {
            return string.Format("L:{0} R:{1} T:{2} B:{3}", Left, Right, Top, Bottom);
        }
    }
}