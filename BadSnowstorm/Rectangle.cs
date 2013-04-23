namespace BadSnowstorm
{
    public struct Rectangle
    {
        public readonly int Left;
        public readonly int Top;
        public readonly int Width;
        public readonly int Height;

        public Rectangle(int left, int top, int width, int height)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }

        public int Right
        {
            get { return Left + Width; }
        }

        public int Bottom
        {
            get { return Top + Height; }
        }

        public bool Contains(Point p)
        {
            return Contains(p.Left, p.Top);
        }

        public bool Contains(int pLeft, int pTop)
        {
            return pTop >= Top && pTop <= Bottom && pLeft >= Left && pLeft <= Right;
        }

        public bool Equals(Rectangle other)
        {
            return Left == other.Left && Top == other.Top && Width == other.Width && Height == other.Height;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is Rectangle && Equals((Rectangle)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Left;
                hashCode = (hashCode * 397) ^ Top;
                hashCode = (hashCode * 397) ^ Width;
                hashCode = (hashCode * 397) ^ Height;
                return hashCode;
            }
        }

        public static bool operator ==(Rectangle left, Rectangle right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Rectangle left, Rectangle right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return string.Format("L:{0} T:{1} W:{2} H:{3}", Left, Right, Width, Height);
        }
    }
}