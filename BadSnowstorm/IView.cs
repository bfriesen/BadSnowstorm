namespace BadSnowstorm
{
    public interface IViewBase
    {
        ViewModel ViewModel { get; set; }
    }

    public interface IView : IViewBase, IShowable
    {
        Rectangle Bounds { get; set; }
        IActionResult GetNextAction(IConsole console);
    }
}