using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace BadSnowstorm
{
    public class DefaultConsole : IConsole
    {
        public ConsoleColor BackgroundColor
        {
            get { return Console.BackgroundColor; }
            set { Console.BackgroundColor = value; }
        }

        public int CursorLeft
        {
            get { return Console.CursorLeft; }
            set { Console.CursorLeft = value; }
        }

        public int CursorTop
        {
            get { return Console.CursorTop; }
            set { Console.CursorTop = value; }
        }

        public bool CursorVisible
        {
            get { return Console.CursorVisible; }
            set { Console.CursorVisible = value; }
        }

        public ConsoleColor ForegroundColor
        {
            get { return Console.ForegroundColor; }
            set { Console.ForegroundColor = value; }
        }

        public int WindowHeight
        {
            get { return Console.WindowHeight; }
            set { Console.WindowHeight = value; }
        }

        public int WindowLeft
        {
            get { return Console.WindowLeft; }
            set { Console.WindowLeft = value; }
        }

        public int WindowTop
        {
            get { return Console.WindowTop; }
            set { Console.WindowTop = value; }
        }

        public int WindowWidth
        {
            get { return Console.WindowWidth; }
            set { Console.WindowWidth = value; }
        }

        public Rectangle GetBounds()
        {
            return new Rectangle(Console.WindowLeft, Console.WindowTop, Console.WindowWidth, Console.WindowHeight);
        }

        public void Beep(int frequency, int duration)
        {
            Console.Beep(frequency, duration);
        }

        public void Clear()
        {
            Console.Clear();
        }

        public void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop)
        {
            Console.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop);
        }

        public char ReadKey()
        {
            return Console.ReadKey(true).KeyChar;
        }

        public void SetCursorPosition(int left, int top)
        {
            Console.SetCursorPosition(left, top);
        }

        public void Write(string value)
        {
            var totalWindowCharacterCount = WindowHeight * WindowWidth;
            var currentCursorOneDimensionIndex = CursorLeft + (WindowWidth * CursorTop);

            if (currentCursorOneDimensionIndex + value.Length >= totalWindowCharacterCount - 1)
            {
                // The value spills to (and maybe past) the bottom-right of the screen - we need to do some trickery to make it render correctly.

                var count = totalWindowCharacterCount - currentCursorOneDimensionIndex - 1;

                var originalLeft = Console.CursorLeft;
                var originalTop = Console.CursorTop;

                Console.SetCursorPosition(Console.WindowWidth - 2, Console.WindowHeight - 1);
                Console.Write(value[count]);
                Console.MoveBufferArea(Console.WindowWidth - 2, Console.WindowHeight - 1, 1, 1, Console.WindowWidth - 1, Console.WindowHeight - 1);

                Console.CursorLeft = originalLeft;
                Console.CursorTop = originalTop;
                
                var output = new string(value.Take(count).ToArray());
                Console.Write(output);
            }
            else
            {
                // Everything should fit - just let Console.Write handle it.
                Console.Write(value);
            }
        }

        public void Write(char value)
        {
            if (Console.CursorLeft == Console.WindowWidth - 1 && Console.CursorTop == Console.WindowHeight - 1)
            {
                Console.SetCursorPosition(Console.WindowWidth - 2, Console.WindowHeight - 1);
                Console.Write(value);
                Console.MoveBufferArea(Console.WindowWidth - 2, Console.WindowHeight - 1, 1, 1, Console.WindowWidth - 1, Console.WindowHeight - 1);
                Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
            }
            else
            {
                Console.Write(value);
            }
        }

        public void SendChar(char value)
        {
            var hWnd = Process.GetCurrentProcess().MainWindowHandle;
            PostMessage(hWnd, WM_KEYDOWN, value, 0);
        }

        // ReSharper disable InconsistentNaming
        private const int WM_KEYDOWN = 0x100;
        // ReSharper restore InconsistentNaming

        [DllImport("User32.Dll", EntryPoint = "PostMessageA")]
        private static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);
    }
}