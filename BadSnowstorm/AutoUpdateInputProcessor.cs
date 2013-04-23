namespace BadSnowstorm
{
    public class AutoUpdateInputProcessor : IInputProcessor
    {
        private readonly AutoUpdateInput _autoUpdateInput;

        public AutoUpdateInputProcessor(AutoUpdateInput autoUpdateInput)
        {
            _autoUpdateInput = autoUpdateInput;
        }

        public IAcceptsInput Process(IConsole console)
        {
            return _autoUpdateInput;
        }
    }
}
