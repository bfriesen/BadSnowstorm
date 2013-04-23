using System;
using System.Collections.Generic;
using System.Threading;

namespace BadSnowstorm
{
    public static class Application
    {
        public static void Run<TSplashScreen, TInitialController>(ApplicationContext<TSplashScreen, TInitialController> context)
            where TSplashScreen : Screen, new()
            where TInitialController : Controller
        {
            var console = new BufferedConsole(context.Console);

            var screen = Activator.CreateInstance<TSplashScreen>();
            screen.Bounds = console.GetBounds(); // Top-level UI element should fill the entire console.
            Show(console, screen);
            console.Clear();

            Run<TInitialController>(console, context.ControllerFactory, context.ViewFactory);
        }

        public static void Run<TInitialController>(ApplicationContext<TInitialController> context)
            where TInitialController : Controller
        {
            Run<TInitialController>(new BufferedConsole(context.Console), context.ControllerFactory, context.ViewFactory);
        }

        private static void Run<TInitialController>(BufferedConsole console, IControllerFactory controllerFactory, IViewFactory viewFactory)
            where TInitialController : Controller
        {
            var controllerHistory = new Stack<Controller>();
            var controller = controllerFactory.Create<TInitialController>();

            while (controller != null)
            {
                var viewModel = controller.Index();
                viewModel.ThrowIfNull(string.Format("ViewModel returned from Controller '{0}', cannot be null.", controller.GetType()));

                var view = viewFactory.CreateView(viewModel);
                view.Bounds = console.GetBounds(); // Top-level UI element should fill the entire console.
                view.ViewModel = viewModel;
                Show(console, view);

                IActionResult nextAction;
                while ((nextAction = view.GetNextAction(console)) is IUpdateViewModelResult)
                {
                    ((IUpdateViewModelResult)nextAction).Update(viewModel);
                    Show(console, view);
                }

                var navigation = nextAction as INavigationResult;
                if (navigation == null)
                {
                    throw new InvalidOperationException("");
                }

                controller = navigation.GetController(controllerFactory, controllerHistory);
            }
        }

        private static void Show(BufferedConsole console, IShowable showable)
        {
            var originalForegroundColor = console.ForegroundColor;
            var originalBackgroundColor = console.BackgroundColor;

            using (console.BeginBuffer())
            {
                showable.Show(console);
            }

            console.ForegroundColor = originalForegroundColor;
            console.BackgroundColor = originalBackgroundColor;

            var screen = showable as Screen;

            if (screen != null)
            {
                if (screen.Song != null)
                {
                    screen.Song.Play(console, cancelOnEscape: true);
                }

                Thread.Sleep(screen.Pause);
            }
        }
    }
}