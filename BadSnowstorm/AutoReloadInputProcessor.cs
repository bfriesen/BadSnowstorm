namespace BadSnowstorm
{
    public class AutoReloadInputProcessor : IInputProcessor
    {
        private readonly AutoReloadInput _autoReloadInput;

        public AutoReloadInputProcessor(AutoReloadInput autoReloadInput)
        {
            _autoReloadInput = autoReloadInput;
        }

        public IAcceptsInput Process(IConsole console)
        {
            return _autoReloadInput;
        }
    }
}
