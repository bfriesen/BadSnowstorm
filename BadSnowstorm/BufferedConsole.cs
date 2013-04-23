using System;
using System.Collections.Generic;

namespace BadSnowstorm
{
    internal class BufferedConsole : IConsole
    {
        private readonly IConsole _console;
        private readonly Buffer _buffer;

        private int _index;
        private bool _isBuffering;
        private ConsoleColor _backgroundColor;
        private ConsoleColor _foregroundColor;

        public BufferedConsole(IConsole console)
        {
            _console = console;
            _buffer = new Buffer(_console.WindowWidth, _console.WindowHeight);
        }

        public IDisposable BeginBuffer()
        {
            _isBuffering = true;
            _index = _console.WindowLeft + (_console.WindowWidth * _console.WindowTop);
            ForegroundColor = _console.ForegroundColor;
            BackgroundColor = _console.BackgroundColor;
            return new Renderer(this);
        }

        private void EndRender()
        {
            var currentCursorLeft = CursorLeft;
            var currentCursorTop = CursorTop;

            foreach (var chunk in _buffer.GetChunks())
            {
                _console.SetCursorPosition(chunk.Left, chunk.Top);

                if (Enum.IsDefined(typeof(ConsoleColor), chunk.Foreground))
                {
                    _console.ForegroundColor = chunk.Foreground;
                }

                if (Enum.IsDefined(typeof(ConsoleColor), chunk.Background))
                {
                    _console.BackgroundColor = chunk.Background;
                }

                _console.Write(new string(chunk.Characters.ToArray()));
            }

            _buffer.Flip();
            _isBuffering = false;

            CursorLeft = currentCursorLeft;
            CursorTop = currentCursorTop;
        }

        public ConsoleColor BackgroundColor
        {
            get
            {
                if (_isBuffering)
                {
                    return _backgroundColor;
                }

                return _console.BackgroundColor;
            }
            set
            {
                if (_isBuffering)
                {
                    _backgroundColor = value;
                }
                else
                {
                    _console.BackgroundColor = value;
                }
            }
        }

        public ConsoleColor ForegroundColor
        {
            get
            {
                if (_isBuffering)
                {
                    return _foregroundColor;
                }

                return _console.ForegroundColor;
            }
            set
            {
                if (_isBuffering)
                {
                    _foregroundColor = value;
                }
                else
                {
                    _console.ForegroundColor = value;
                }
            }
        }

        public int CursorLeft
        {
            get
            {
                return _index % _console.WindowWidth;
            }
            set
            {
                _console.CursorLeft = value;
                _index = value + (_console.WindowWidth * CursorTop);
            }
        }

        public int CursorTop
        {
            get
            {
                return _index / _console.WindowWidth;
            }
            set
            {
                _console.CursorTop = value;
                _index = CursorLeft + (_console.WindowWidth * value);
            }
        }

        public bool CursorVisible
        {
            get { return _console.CursorVisible; }
            set { _console.CursorVisible = value; }
        }

        public int WindowHeight
        {
            get { return _console.WindowHeight; }
            set { _console.WindowHeight = value; }
        }

        public int WindowLeft
        {
            get { return _console.WindowLeft; }
            set { _console.WindowLeft = value; }
        }

        public int WindowTop
        {
            get { return _console.WindowTop; }
            set { _console.WindowTop = value; }
        }

        public int WindowWidth
        {
            get { return _console.WindowWidth; }
            set { _console.WindowWidth = value; }
        }

        public Rectangle GetBounds()
        {
            return _console.GetBounds();
        }

        public void Beep(int frequency, int duration)
        {
            _console.Beep(frequency, duration);
        }

        public void Clear()
        {
            _console.Clear();
            _buffer.Clear();
            ForegroundColor = _console.ForegroundColor;
            BackgroundColor = _console.BackgroundColor;
        }

        public void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop)
        {
            // TODO: Implement
        }

        public char ReadKey()
        {
            return _console.ReadKey();
        }

        public void SetCursorPosition(int left, int top)
        {
            _console.SetCursorPosition(left, top);
            _index = left + (_console.WindowWidth * top);
        }

        public void Write(string value)
        {
            if (!_isBuffering)
            {
                _console.Write(value);
            }
            
            foreach (var c in value)
            {
                _buffer.Back[_index++] = new BufferedChar(c, ForegroundColor, BackgroundColor);
            }
        }

        public void Write(char value)
        {
            if (!_isBuffering)
            {
                _console.Write(value);
            }
            
            _buffer.Back[_index++] = new BufferedChar(value, ForegroundColor, BackgroundColor);
        }

        public void SendChar(char value)
        {
            _console.SendChar(value);
        }

        private class Renderer : IDisposable
        {
            private readonly BufferedConsole _console;

            public Renderer(BufferedConsole console)
            {
                _console = console;
            }

            public void Dispose()
            {
                _console.EndRender();
            }
        }

        private class BufferedChar
        {
            public static readonly BufferedChar NotSet = new BufferedChar();

            private readonly char _value = '\0';

            public readonly ConsoleColor Foreground = (ConsoleColor)(-1);
            public readonly ConsoleColor Background = (ConsoleColor)(-1);

            private BufferedChar()
            {
            }

            public BufferedChar(char value, ConsoleColor foreground, ConsoleColor background)
            {
                _value = value;
                Foreground = foreground;
                Background = background;
            }

            public bool IsSameAs(BufferedChar other)
            {
                return _value == other._value && Foreground == other.Foreground && Background == other.Background;
            }

            public char GetSafeValue()
            {
                return _value == '\0' ? ' ' : _value;
            }
        }

        private class Buffer
        {
            private readonly int _width;

            private readonly BufferedChar[] _front;
            private readonly BufferedChar[] _back;

            private bool _isReversed;

            public Buffer(int width, int height)
            {
                _width = width;
                _front = new BufferedChar[width * height];
                _back = new BufferedChar[width * height];

                for (int i = 0; i < _front.Length; i++)
                {
                    _front[i] = BufferedChar.NotSet;
                    _back[i] = BufferedChar.NotSet;
                }
            }

            public void Clear()
            {
                var front = Front;
                var back = Back;
                for (int i = 0; i < front.Length; i++)
                {
                    front[i] = BufferedChar.NotSet;
                    back[i] = BufferedChar.NotSet;
                }
            }

            public void Flip()
            {
                _isReversed = !_isReversed;
                var buffer = Back;
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = BufferedChar.NotSet;
                }
            }

            private BufferedChar[] Front
            {
                get { return _isReversed ? _back : _front; }
            }

            public BufferedChar[] Back
            {
                get { return _isReversed ? _front : _back; }
            }

            public IEnumerable<BufferChunk> GetChunks()
            {
                var front = Front;
                var back = Back;

                var chunks = new List<BufferChunk>();
                BufferChunk currentChunk = null;

                for (int i = 0; i < front.Length; i++)
                {
                    if (front[i].IsSameAs(back[i]))
                    {
                        if (currentChunk != null)
                        {
                            chunks.Add(currentChunk);
                            currentChunk = null;
                        }

                        continue;
                    }

                    if (currentChunk == null)
                    {
                        currentChunk = new BufferChunk
                        {
                            Top = i / _width,
                            Left = i % _width,
                            Foreground = back[i].Foreground,
                            Background = back[i].Background
                        };
                        currentChunk.Characters.Add(back[i].GetSafeValue());
                        continue;
                    }

                    if (currentChunk.Foreground != back[i].Foreground || currentChunk.Background != back[i].Background)
                    {
                        chunks.Add(currentChunk);
                        currentChunk = new BufferChunk
                        {
                            Top = i / _width,
                            Left = i % _width,
                            Foreground = back[i].Foreground,
                            Background = back[i].Background
                        };
                        currentChunk.Characters.Add(back[i].GetSafeValue());
                    }
                    else
                    {
                        currentChunk.Characters.Add(back[i].GetSafeValue());
                    }
                }

                if (currentChunk != null)
                {
                    chunks.Add(currentChunk);
                }

                return chunks;
            }
        }

        private class BufferChunk
        {
            public BufferChunk()
            {
                Characters = new List<char>();
            }

            public int Left { get; set; }
            public int Top { get; set; }
            public ConsoleColor Foreground { get; set; }
            public ConsoleColor Background { get; set; }
            public List<char> Characters { get; private set; }
        }
    }
}