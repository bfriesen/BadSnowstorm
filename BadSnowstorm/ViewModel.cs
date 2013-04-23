namespace BadSnowstorm
{
    using System;

    public abstract class ViewModel
    {
        public static TViewModel Create<TViewModel>(Action<TViewModel> configureViewModel = null)
            where TViewModel : ViewModel, new()
        {
            var viewModel = new TViewModel();
           
            if (configureViewModel != null)
            {
                configureViewModel(viewModel);
            }

            return viewModel;
        }

        public static TViewModel CreateWithMenu<TViewModel>(Menu menu)
            where TViewModel : ViewModel, new()
        {
            return Create<TViewModel>(viewModel =>
            {
                viewModel.Input = menu;
            });
        }

        public static TViewModel CreateWithTextInput<TViewModel>(TextInput textInput)
            where TViewModel : ViewModel, new()
        {
            return Create<TViewModel>(viewModel =>
            {
                viewModel.Input = textInput;
            });
        }

        public static TViewModel CreateWithAutoUpdateInput<TViewModel>(string initialInputContent, Action<TViewModel> updateAction, Input postUpdateInput, int postUpdateDelayMilliseconds = 0)
            where TViewModel : ViewModel, new()
        {
            return Create<TViewModel>(viewModel =>
            {
                viewModel.Input = AutoUpdateInput.Create<TViewModel>(initialInputContent, updateAction, postUpdateInput, postUpdateDelayMilliseconds);
            });
        }

        public static TViewModel CreateWithAutoReloadInput<TViewModel>(Controller currentController)
            where TViewModel : ViewModel, new()
        {
            return Create<TViewModel>(viewModel =>
            {
                viewModel.Input = new AutoReloadInput(currentController);
            });
        }

        public static TViewModel CreateWithAutoReloadInput<TViewModel>(Controller currentController, string initialInputContent, int delayMilliseconds = 0)
            where TViewModel : ViewModel, new()
        {
            return Create<TViewModel>(viewModel =>
            {
                viewModel.Input = new AutoReloadInput(currentController, initialInputContent, delayMilliseconds);
            });
        }

        public Input Input { get; internal set; }
    }

    public static class ViewModelExtensions
    {
        public static TInput SetPrompt<TInput>(this TInput input, string prompt)
            where TInput : Input
        {
            input.Prompt = prompt;
            return input;
        }

        public static TViewModel WithMenu<TViewModel>(this TViewModel viewModel, Action<Menu> configureMenu)
            where TViewModel : ViewModel
        {
            var menu = new Menu();
            configureMenu(menu);
            viewModel.Input = menu;
            return viewModel;
        }

        public static Menu AddMenuItem(this Menu menu, MenuItem menuItem)
        {
            menu.MenuItems.Add(menuItem);
            return menu;
        }

        public static TViewModel WithTextInput<TViewModel>(
            this TViewModel viewModel,
            Func<IActionResult> textEnteredResult,
            Action<TextInput> configureTextInput = null)
            where TViewModel : ViewModel
        {
            var textInput = new TextInput(textEnteredResult);
            configureTextInput(textInput);
            viewModel.Input = textInput;
            return viewModel;
        }

        public static TextInput SetTextEnteredCallback(this TextInput textInput, Action<string> onTextEntered)
        {
            textInput.OnInputAccepted = () => onTextEntered(textInput.Value);
            return textInput;
        }
    }
}