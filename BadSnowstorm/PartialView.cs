using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadSnowstorm
{
    public class PartialView<TParentView, TPartialView, TParentViewModel, TPartialViewModel> : ContentArea, IViewBase
        where TParentView : View<TParentView, TParentViewModel>, IView
        where TPartialView : PartialView<TParentView, TPartialView, TParentViewModel, TPartialViewModel>
        where TParentViewModel : ViewModel
        where TPartialViewModel : ViewModel
    {
        private readonly BindingCollection<TPartialView, TPartialViewModel> _bindings;

        private readonly Func<TParentView, TPartialView> _getPartialView;
        private readonly Func<TParentViewModel, TPartialViewModel> _getPartialViewModel;

        public PartialView(Func<TParentView, TPartialView> getPartialView, Func<TParentViewModel, TPartialViewModel> getPartialViewModel)
        {
            _getPartialView = getPartialView;
            _getPartialViewModel = getPartialViewModel;
            _bindings = new BindingCollection<TPartialView, TPartialViewModel>();
        }

        public BindingCollection<TPartialView, TPartialViewModel> Bindings
        {
            get { return _bindings; }
        }

        ViewModel IViewBase.ViewModel { get; set; }

        internal IEnumerable<Binding<TParentView, TParentViewModel>> GetBindings()
        {
            return _bindings.Select(b =>
                new Binding<TParentView, TParentViewModel>(
                    view => b.GetContentArea(_getPartialView(view)),
                    viewModel => b.GetContent(_getPartialViewModel(viewModel))));
        }
    }
}
