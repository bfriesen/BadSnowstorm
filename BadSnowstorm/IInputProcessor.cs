namespace BadSnowstorm
{
    public interface IInputProcessor
    {
        IAcceptsInput Process(IConsole console);
    }
}