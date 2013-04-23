using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadSnowstorm
{
    public sealed class PopToNearest<TController> : INavigationResult
        where TController : Controller
    {
        Controller INavigationResult.GetController(IControllerFactory controllerFactory, Stack<Controller> controllerHistory)
        {
            while (controllerHistory.Peek().GetType() != typeof(TController))
            {
                controllerHistory.Pop();
            }

            return controllerHistory.Pop();
        }
    }
}
