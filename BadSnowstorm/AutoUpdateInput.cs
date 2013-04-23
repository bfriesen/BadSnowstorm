using System;
using System.Threading;

namespace BadSnowstorm
{
    public class AutoUpdateInput : Input, IAcceptsInput
    {
        private readonly string _initialInputContent;
        private readonly Func<IUpdateViewModelResult> _createUpdateViewModelResult;
        private readonly Input _postUpdateInput;

        private AutoUpdateInput(string initialInputContent, Func<IUpdateViewModelResult> createUpdateViewModelResult)
            : base(InputType.AutoUpdate)
        {
            _initialInputContent = initialInputContent;
            _createUpdateViewModelResult = createUpdateViewModelResult;
        }

        internal static AutoUpdateInput Create<TViewModel>(string initialInputContent, Action<TViewModel> updateAction, Input postUpdateInput, int postUpdateDelayMilliseconds)
            where TViewModel : ViewModel
        {
            Action<TViewModel> appliedUpdateAction = viewModel =>
            {
                updateAction((TViewModel)viewModel);
                // TODO: abstract the Thread.Sleep call and inject it (or, gag, use a service locator).
                Thread.Sleep(postUpdateDelayMilliseconds);
                viewModel.Input = postUpdateInput;
            };

            return new AutoUpdateInput(
                initialInputContent,
                () => UpdateViewModelResult.Create<TViewModel>(appliedUpdateAction));
        }

        public override string GetContent()
        {
            return _initialInputContent;
        }

        public Action OnInputAccepted { get; set; }

        public Func<IActionResult> ActionResult
        {
            get { return _createUpdateViewModelResult; }
        }
    }
}
