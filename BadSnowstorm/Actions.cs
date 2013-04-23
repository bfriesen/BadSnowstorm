using System;

namespace BadSnowstorm
{
    public class Actions
    {
        private readonly Controller controller;

        public Actions(Controller controller)
        {
            this.controller = controller;
        }

        public IActionResult GoTo<TController>() where TController : Controller
        {
            return new GoTo<TController>(controller);
        }

        public IActionResult PopAndGoTo<TController>() where TController : Controller
        {
            return new PopAndGoTo<TController>(controller);
        }

        public IActionResult GoBack()
        {
            return new GoBack();
        }

        public IActionResult Reload()
        {
            return new Reload(controller);
        }

        public IActionResult UpdateViewModel<TViewModel>(Action<TViewModel> updateAction)
            where TViewModel : ViewModel
        {
            return UpdateViewModelResult.Create(updateAction);
        }

        public IActionResult PopToNearest<TController>()
            where TController : Controller
        {
            return new PopToNearest<TController>();
        }
    }
}