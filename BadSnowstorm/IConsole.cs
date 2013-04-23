using System;

namespace BadSnowstorm
{
    public interface IConsole
    {
        ConsoleColor BackgroundColor { get; set; }
        int CursorLeft { get; set; }
        int CursorTop { get; set; }
        bool CursorVisible { get; set; }
        ConsoleColor ForegroundColor { get; set; }
        int WindowHeight { get; set; }
        int WindowLeft { get; set; }
        int WindowTop { get; set; }
        int WindowWidth { get; set; }
        Rectangle GetBounds();
        void Beep(int frequency, int duration);
        void Clear();
        void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop);
        char ReadKey();
        void SetCursorPosition(int left, int top);
        void Write(string value);
        void Write(char value);
        void SendChar(char value);
    }
}