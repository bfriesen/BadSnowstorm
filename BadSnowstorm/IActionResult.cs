using System.Collections.Generic;

namespace BadSnowstorm
{
    public interface IActionResult
    {
    }

    public interface INavigationResult : IActionResult
    {
        Controller GetController(IControllerFactory controllerFactory, Stack<Controller> controllerHistory);
    }

    public interface IUpdateViewModelResult : IActionResult
    {
        void Update(ViewModel viewModel);
    }
}