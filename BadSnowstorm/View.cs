using System;
using System.Linq;

namespace BadSnowstorm
{
    public abstract class View<TView, TViewModel> : Screen, IView
        where TView : View<TView, TViewModel>, IView
        where TViewModel : ViewModel
    {
        private readonly IInputArea _inputArea;
        private readonly BindingCollection<TView, TViewModel> _bindings;

        protected View()
        {
            _inputArea = CreateInputArea();
            _bindings = new BindingCollection<TView, TViewModel>();
            Bindings.Add(view => view.InputArea, viewModel => viewModel.Input);
        }

        protected abstract InputArea CreateInputArea();

        public override string Name
        {
            get { return GetType().ToString(); }
        }

        public TViewModel ViewModel { get; private set; }

        public IInputArea InputArea
        {
            get { return _inputArea; }
        }

        public BindingCollection<TView, TViewModel> Bindings
        {
            get { return _bindings; }
        }

        ViewModel IViewBase.ViewModel
        {
            get { return ViewModel; }
            set
            {
                if (ViewModel != null)
                {
                    throw new InvalidOperationException("ViewModel property has already been set - it can only set ViewModel once.");
                }

                var tViewModel = value as TViewModel;
                if (tViewModel == null)
                {
                    throw new InvalidOperationException("Invalid type for value. Must be " + typeof(TViewModel));
                }

                ViewModel = tViewModel;
            }
        }

        public IActionResult GetNextAction(IConsole console)
        {
            return InputArea.GetNextAction(ViewModel, console);
        }

        public override void Show(IConsole console)
        {
            ClearContents(console);
            
            BorderRenderer.Render(console, this.GetSelfAndDescendants().ToList());
            
            foreach (var binding in Bindings)
            {
                binding.Render((TView)this, console);
            }

            console.SetCursorPosition(this.InputArea.LastCursorLocation.Left, this.InputArea.LastCursorLocation.Top);
        }

        private void ClearContents(IConsole console)
        {
            foreach (var clientArea in this.GetDescendants())
            {
                clientArea.ClearContent(console);
            }
        }
    }
}