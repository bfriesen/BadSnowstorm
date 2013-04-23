using System.Text;

namespace BadSnowstorm
{
    public class TextInputProcessor : IInputProcessor
    {
        private readonly TextInput _textInput;

        public TextInputProcessor(TextInput textInput)
        {
            _textInput = textInput;
        }

        public IAcceptsInput Process(IConsole console)
        {
            _textInput.Value = null;

            var isCancelled = false;
            var sb = new StringBuilder();

            console.CursorVisible = true;
            var x = console.CursorLeft;
            var y = console.CursorTop;

            while (true)
            {
                var isFinished = false;
                char c;
                switch ((c = console.ReadKey()))
                {
                    case (char)8: // backspace
                        if (sb.Length > 0)
                        {
                            console.SetCursorPosition(x + sb.Length - 1, y);
                            console.Write(' ');
                            sb.Remove(sb.Length - 1, 1);
                            console.SetCursorPosition(x + sb.Length, y);
                        }
                        break;
                    case (char)13: // enter
                        isFinished = true;
                        break;
                    case (char)27: // escape
                        isCancelled = true;
                        break;
                    default:
                        if (!char.IsControl(c))
                        {
                            sb.Append(c);
                            console.Write(c);
                            console.SetCursorPosition(x + sb.Length, y);
                        }
                        break;
                }

                if (isFinished || isCancelled)
                {
                    break;
                }
            }

            console.CursorVisible = false;
            for (int i = 0; i < sb.Length; i++)
            {
                console.SetCursorPosition(x + i, y);
                console.Write(' ');
            }

            _textInput.Value = isCancelled ? null : sb.ToString();
            return _textInput;
        }
    }
}