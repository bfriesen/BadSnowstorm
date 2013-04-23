using System.Collections.Generic;

namespace BadSnowstorm
{
    public sealed class GoBack : INavigationResult
    {
        internal GoBack()
        {
        }

        Controller INavigationResult.GetController(IControllerFactory controllerFactory, Stack<Controller> controllerHistory)
        {
            if (controllerHistory.Count > 0)
            {
                return controllerHistory.Pop();
            }

            return null;
        }
    }
}