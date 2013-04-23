using System;

namespace BadSnowstorm
{
    public sealed class UpdateViewModelResult : IUpdateViewModelResult
    {
        private readonly Action<ViewModel> _updateAction;

        private UpdateViewModelResult(Action<ViewModel> updateAction)
        {
            _updateAction = updateAction;
        }

        internal static UpdateViewModelResult Create<TViewModel>(Action<TViewModel> updateAction)
            where TViewModel : ViewModel
        {
            return new UpdateViewModelResult(viewModel => updateAction((TViewModel)viewModel));
        }

        void IUpdateViewModelResult.Update(ViewModel viewModel)
        {
            _updateAction(viewModel);
        }
    }
}