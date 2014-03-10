using System;
namespace BadSnowstorm
{
    public interface IInputArea : IContentArea
    {
        IActionResult GetNextAction(ViewModel viewModel, IConsole console);
    }
}
