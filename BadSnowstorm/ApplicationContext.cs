namespace BadSnowstorm
{
    // ReSharper disable UnusedTypeParameter
    public class ApplicationContext<TInitialController>
        where TInitialController : Controller
    {
        public ApplicationContext()
        {
            Console = new DefaultConsole();
            ControllerFactory = new DefaultControllerFactory();
            ViewFactory = new DefaultViewFactory();
        }

        public IConsole Console { get; set; }
        public IControllerFactory ControllerFactory { get; set; }
        public IViewFactory ViewFactory { get; set; }
    }

    public class ApplicationContext<TSplashScreen, TInitialController> : ApplicationContext<TInitialController>
        where TSplashScreen : Screen, new()
        where TInitialController : Controller
    {
    }
    // ReSharper restore UnusedTypeParameter
}