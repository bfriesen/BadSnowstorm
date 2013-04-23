using System;
using System.Collections.Generic;
using System.Linq;

namespace BadSnowstorm
{
    public class ContentArea
    {
        public const char BlackForeground = '\u03B1';
        public const char BlueForeground = '\u03B2';
        public const char CyanForeground = '\u03B3';
        public const char DarkBlueForeground = '\u03B4';
        public const char DarkCyanForeground = '\u03B5';
        public const char DarkGrayForeground = '\u03B6';
        public const char DarkGreenForeground = '\u03B7';
        public const char DarkMagentaForeground = '\u03B8';
        public const char DarkRedForeground = '\u03B9';
        public const char DarkYellowForeground = '\u03BA';
        public const char GrayForeground = '\u03BB';
        public const char GreenForeground = '\u03BC';
        public const char MagentaForeground = '\u03BD';
        public const char RedForeground = '\u03BE';
        public const char WhiteForeground = '\u03BF';
        public const char YellowForeground = '\u03C0';

        public const char BlackBackground = '\u0391';
        public const char BlueBackground = '\u0392';
        public const char CyanBackground = '\u0393';
        public const char DarkBlueBackground = '\u0394';
        public const char DarkCyanBackground = '\u0395';
        public const char DarkGrayBackground = '\u0396';
        public const char DarkGreenBackground = '\u0397';
        public const char DarkMagentaBackground = '\u0398';
        public const char DarkRedBackground = '\u0399';
        public const char DarkYellowBackground = '\u039A';
        public const char GrayBackground = '\u039B';
        public const char GreenBackground = '\u039C';
        public const char MagentaBackground = '\u039D';
        public const char RedBackground = '\u039E';
        public const char WhiteBackground = '\u039F';
        public const char YellowBackground = '\u03A0';

        private static readonly string ForegroundColors = string.Concat(
            BlackForeground, BlueForeground, CyanForeground, DarkBlueForeground, DarkCyanForeground, DarkGrayForeground,
            DarkGreenForeground, DarkMagentaForeground, DarkRedForeground, DarkYellowForeground, GrayForeground, GreenForeground,
            MagentaForeground, RedForeground, WhiteForeground, YellowForeground);

        private static readonly string BackgroundColors = string.Concat(
            BlackBackground, BlueBackground, CyanBackground, DarkBlueBackground, DarkCyanBackground, DarkGrayBackground,
            DarkGreenBackground, DarkMagentaBackground, DarkRedBackground, DarkYellowBackground, GrayBackground, GreenBackground,
            MagentaBackground, RedBackground, WhiteBackground, YellowBackground);

        private static readonly string AllColors = ForegroundColors + BackgroundColors;

        private readonly string _name;

        internal ContentArea()
        {
            Children = new List<ContentArea>();            
        }

        public ContentArea(string name)
            : this()
        {
            _name = name.ThrowIfArgumentNull("name");
        }

        public virtual string Name
        {
            get { return _name; }
        }

        public Rectangle Bounds { get; set; }
        public Padding Padding { get; set; }
        public Border Border { get; set; }
        public string Content { get; set; }
        public ContentAlignment ContentAlignment { get; set; }
        public ContentType ContentType { get; set; }
        public List<ContentArea> Children { get; private set; }
        public Func<Point, BorderInfo, bool> BorderRenderOverride { get; set; }
        public Point LastCursorLocation { get; private set; }

        public void RenderContent(IConsole console)
        {
            if (string.IsNullOrEmpty(Content))
            {
                return;
            }

            var paddedBounds = GetPaddedBounds();

            var lines = Content.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();

            if (ContentType == ContentType.Text)
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    var line = lines[i];

                    if (line.Length > paddedBounds.Width)
                    {
                        var truncated = line.Substring(0, paddedBounds.Width);
                        var lastSpaceOnLine = truncated.LastIndexOf(' ');

                        string left, right;

                        if (lastSpaceOnLine != -1)
                        {
                            left = truncated.Substring(0, lastSpaceOnLine);
                            right = line.Substring(lastSpaceOnLine).TrimStart();
                        }
                        else
                        {
                            left = line.Substring(0, paddedBounds.Width);
                            right = line.Substring(paddedBounds.Width).TrimStart();
                        }

                        lines[i] = left;
                        lines.Insert(i + 1, right);
                    }
                }

                while (lines.Count > paddedBounds.Height)
                {
                    lines.RemoveAt(lines.Count - 1);
                }
            }
            else
            {
                if (lines.Count > paddedBounds.Height)
                {
                    if (ContentAlignment == ContentAlignment.TopLeft)
                    {
                        while (lines.Count > paddedBounds.Height)
                        {
                            lines.RemoveAt(lines.Count - 1);
                        }
                    }
                    else
                    {
                        var removeFromBeginning = false;
                        while (lines.Count > paddedBounds.Height)
                        {
                            if (removeFromBeginning)
                            {
                                lines.RemoveAt(0);
                            }
                            else
                            {
                                lines.RemoveAt(lines.Count - 1);
                            }

                            removeFromBeginning = !removeFromBeginning;
                        }
                    }
                }

                for (int i = 0; i < lines.Count; i++)
                {
                    var line = lines[i];

                    if (line.Length > paddedBounds.Width)
                    {
                        if (ContentAlignment == ContentAlignment.TopLeft)
                        {
                            lines[i] = line.Substring(0, paddedBounds.Width);
                        }
                        else
                        {
                            var removeFromBeginning = false;
                            while (line.Length > paddedBounds.Width)
                            {
                                line = removeFromBeginning ? line.Substring(1) : line.Substring(0, line.Length - 1);
                                removeFromBeginning = !removeFromBeginning;
                            }

                            lines[i] = line;
                        }
                    }
                }
            }
            
            Func<string, int> xModifier;
            Func<string, int> yModifier;

            if (ContentAlignment == ContentAlignment.Centered)
            {
                Func<int, int, int> adjust = (containerBoundry, contentSize) => containerBoundry % 2 == 0 && contentSize % 2 == 1 ? 1 : 0;

                xModifier = line =>
                {
                    var lineLength = line.Count(c => !AllColors.Contains(c));
                    return Math.Max(paddedBounds.Left, paddedBounds.Left + (paddedBounds.Width / 2) - (lineLength / 2) - adjust(paddedBounds.Width, lineLength));
                };
                yModifier = line => Math.Max(paddedBounds.Top, paddedBounds.Top + (paddedBounds.Height / 2) - (lines.Count / 2) - adjust(paddedBounds.Height, lines.Count));
            }
            else
            {
                xModifier = line => paddedBounds.Left;
                yModifier = line => paddedBounds.Top;
            }

            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                var y = i + yModifier(line);
                var x = xModifier(line);

                if (line.Length > 0)
                {
                    foreach (char c in line)
                    {
                        if (ForegroundColors.Contains(c))
                        {
                            console.ForegroundColor = GetColor(c);
                        }
                        else if (BackgroundColors.Contains(c))
                        {
                            console.BackgroundColor = GetColor(c);
                        }
                        else
                        {
                            if (paddedBounds.Contains(x, y))
                            {
                                console.SetCursorPosition(x, y);
                                console.Write(c);
                            }

                            x++;
                        }
                    }
                }
                else
                {
                    console.SetCursorPosition(x, y);
                }
            }

            LastCursorLocation = new Point(console.CursorLeft, console.CursorTop);
        }

        private Rectangle GetPaddedBounds()
        {
            var left = Padding.Left + (Border.Left != BorderType.None ? 1 : 0);
            var right = Padding.Right + (Border.Right != BorderType.None ? 1 : 0) - 1;
            var top = Padding.Top + (Border.Top != BorderType.None ? 1 : 0);
            var bottom = Padding.Bottom + (Border.Bottom != BorderType.None ? 1 : 0) - 1;

            var paddedBounds = new Rectangle(
                Bounds.Left + left,
                Bounds.Top + top,
                Bounds.Width - right - left,
                Bounds.Height - bottom - top);

            return paddedBounds;
        }

        public void ClearContent(IConsole console)
        {
            var nonBorderBounds = GetNonBorderBounds();

            for (int y = nonBorderBounds.Top; y <= nonBorderBounds.Bottom; y++)
            {
                console.SetCursorPosition(nonBorderBounds.Left, y);
                console.Write(new string(' ', nonBorderBounds.Width));
            }
        }

        private Rectangle GetNonBorderBounds()
        {
            var left = Border.Left != BorderType.None ? 1 : 0;
            var right = (Border.Right != BorderType.None ? 1 : 0) - 1;
            var top = Border.Top != BorderType.None ? 1 : 0;
            var bottom = Border.Bottom != BorderType.None ? 1 : 0;

            var paddedBounds = new Rectangle(
                Bounds.Left + left,
                Bounds.Top + top,
                Bounds.Width - right - left,
                Bounds.Height - bottom - top);

            return paddedBounds;
        }

        protected IEnumerable<ContentArea> GetDescendants()
        {
            foreach (var child in Children.Where(x => x != null))
            {
                yield return child;

                foreach (var descendant in child.GetDescendants())
                {
                    yield return descendant;
                }
            }
        }

        protected IEnumerable<ContentArea> GetSelfAndDescendants()
        {
            yield return this;

            foreach (var descendant in GetDescendants())
            {
                yield return descendant;
            }
        }

        protected ConsoleColor GetColor(char greekChar)
        {
            switch (greekChar)
            {
                case BlackForeground:
                case BlackBackground:
                    return ConsoleColor.Black;
                case BlueForeground:
                case BlueBackground:
                    return ConsoleColor.Blue;
                case CyanForeground:
                case CyanBackground:
                    return ConsoleColor.Cyan;
                case DarkBlueForeground:
                case DarkBlueBackground:
                    return ConsoleColor.DarkBlue;
                case DarkCyanForeground:
                case DarkCyanBackground:
                    return ConsoleColor.DarkCyan;
                case DarkGrayForeground:
                case DarkGrayBackground:
                    return ConsoleColor.DarkGray;
                case DarkGreenForeground:
                case DarkGreenBackground:
                    return ConsoleColor.DarkGreen;
                case DarkMagentaForeground:
                case DarkMagentaBackground:
                    return ConsoleColor.DarkMagenta;
                case DarkRedForeground:
                case DarkRedBackground:
                    return ConsoleColor.DarkRed;
                case DarkYellowForeground:
                case DarkYellowBackground:
                    return ConsoleColor.DarkYellow;
                case GrayForeground:
                case GrayBackground:
                    return ConsoleColor.Gray;
                case GreenForeground:
                case GreenBackground:
                    return ConsoleColor.Green;
                case MagentaForeground:
                case MagentaBackground:
                    return ConsoleColor.Magenta;
                case RedForeground:
                case RedBackground:
                    return ConsoleColor.Red;
                case WhiteForeground:
                case WhiteBackground:
                    return ConsoleColor.White;
                case YellowForeground:
                case YellowBackground:
                    return ConsoleColor.Yellow;
                default:
                    return ConsoleColor.White;
            }
        }
    }
}