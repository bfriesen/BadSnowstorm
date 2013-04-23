using System.Collections.Generic;

namespace BadSnowstorm
{
    public class Reload : INavigationResult
    {
        private readonly Controller _currentController;

        internal Reload(Controller currentController)
        {
            _currentController = currentController;
        }

        Controller INavigationResult.GetController(IControllerFactory controllerFactory, Stack<Controller> controllerHistory)
        {
            return _currentController;
        }
    }
}
