using System;
using System.Collections.Generic;

namespace BadSnowstorm
{
    public class InputArea : ContentArea, IInputArea
    {
        private static readonly Dictionary<InputType, Func<Input, IInputProcessor>> InputHandlerMap;

        static InputArea()
        {
            InputHandlerMap = new Dictionary<InputType, Func<Input, IInputProcessor>>
            {
                { InputType.Menu, input => new MenuInputProcessor(((Menu)input).MenuItems) },
                { InputType.Text, input => new TextInputProcessor((TextInput)input) },
                { InputType.AutoUpdate, input => new AutoUpdateInputProcessor((AutoUpdateInput)input) },
                { InputType.AutoReload, input => new AutoReloadInputProcessor((AutoReloadInput)input) }
            };
        }

        public InputArea()
            : base("__inputArea")
        {
            ContentType = ContentType.Text;
        }

        public IActionResult GetNextAction(ViewModel viewModel, IConsole console)
        {
            var inputProcessor = InputHandlerMap[viewModel.Input.InputType](viewModel.Input);
            var input = inputProcessor.Process(console);
            if (input.OnInputAccepted != null)
            {
                input.OnInputAccepted();
            }
            return input.ActionResult();
        }
    }
}