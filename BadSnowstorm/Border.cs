using System;
using System.Collections.Generic;

namespace BadSnowstorm
{
    public struct Border
    {
        private static readonly Lazy<Border> _none = new Lazy<Border>(() => new Border(BorderType.None, BorderType.None, BorderType.None, BorderType.None));
        private static readonly Lazy<Border> _singleLine = new Lazy<Border>(() => new Border(BorderType.SingleLine, BorderType.SingleLine, BorderType.SingleLine, BorderType.SingleLine));
        private static readonly Lazy<Border> _doubleLine = new Lazy<Border>(() => new Border(BorderType.DoubleLine, BorderType.DoubleLine, BorderType.DoubleLine, BorderType.DoubleLine));

        public readonly BorderType Left;
        public readonly BorderType Right;
        public readonly BorderType Top;
        public readonly BorderType Bottom;

        public Border(BorderType left, BorderType right, BorderType top, BorderType bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        public static Border None
        {
            get { return _none.Value; }
        }

        public static Border SingleLine
        {
            get { return _singleLine.Value; }
        }

        public static Border DoubleLine
        {
            get { return _doubleLine.Value; }
        }

        public bool Equals(Border other)
        {
            return Left == other.Left && Right == other.Right && Top == other.Top && Bottom == other.Bottom;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is Border && Equals((Border)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)Left;
                hashCode = (hashCode * 397) ^ (int)Right;
                hashCode = (hashCode * 397) ^ (int)Top;
                hashCode = (hashCode * 397) ^ (int)Bottom;
                return hashCode;
            }
        }

        public static bool operator ==(Border left, Border right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Border left, Border right)
        {
            return !left.Equals(right);
        }

        public IEnumerable<BorderCharacter> GetBorderCharacters(Rectangle location)
        {
            if (Left != BorderType.None)
            {
                for (int i = location.Top; i <= location.Bottom; i++)
                {
                    yield return BorderCharacter.LeftBorder(Left, new Point(location.Left, i));
                }
            }

            if (Right != BorderType.None)
            {
                for (int i = location.Top; i <= location.Bottom; i++)
                {
                    yield return BorderCharacter.RightBorder(Right, new Point(location.Right, i));
                }
            }

            if (Top != BorderType.None)
            {
                for (int i = location.Left; i <= location.Right; i++)
                {
                    yield return BorderCharacter.TopBorder(Top, new Point(i, location.Top));
                }
            }

            if (Bottom != BorderType.None)
            {
                for (int i = location.Left; i <= location.Right; i++)
                {
                    yield return BorderCharacter.BottomBorder(Bottom, new Point(i, location.Bottom));
                }
            }
        }

        public override string ToString()
        {
            if (this == None)
            {
                return "[All:None]";
            }

            if (this == SingleLine)
            {
                return "[All:SingleLine]";
            }

            if (this == DoubleLine)
            {
                return "[All:DoubleLine]";
            }

            return string.Format("[L:{0}, R:{1}, T:{2}, B:{3}]", Left, Right, Top, Bottom);
        }
    }
}