namespace BadSnowstorm
{
    public struct Point
    {
        public readonly int Left;
        public readonly int Top;

        public Point(int left, int top)
        {
            Left = left;
            Top = top;
        }

        public bool Equals(Point other)
        {
            return Left == other.Left && Top == other.Top;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is Point && Equals((Point)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Left * 397) ^ Top;
            }
        }

        public static bool operator ==(Point left, Point right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Point left, Point right)
        {
            return !left.Equals(right);
        }
    }
}