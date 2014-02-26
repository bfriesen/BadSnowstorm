using BadSnowstorm;
using System;

namespace HelloWorld
{
    public class HelloWorldController : Controller
    {
        public override ViewModel Index()
        {
            var menu = new Menu();

            menu.AddMenuItem(new MenuItem { Text = "Goodbye, cruel world!", Id = '1', ActionResult = Actions.GoBack });

            var viewModel = ViewModel.CreateWithMenu<HelloWorldViewModel>(menu);

            viewModel.Message = "Hello, world!";
            viewModel.MessageArt = @"
  _    _      _ _                             _     _ _ 
 | |  | |    | | |                           | |   | | |
 | |__| | ___| | | ___    __      _____  _ __| | __| | |
 |  __  |/ _ \ | |/ _ \   \ \ /\ / / _ \| '__| |/ _` | |
 | |  | |  __/ | | (_) |   \ V  V / (_) | |  | | (_| |_|
 |_|  |_|\___|_|_|\___( )   \_/\_/ \___/|_|  |_|\__,_(_)
                      |/                                
                                                        
";

            return viewModel;
        }
    }
}
