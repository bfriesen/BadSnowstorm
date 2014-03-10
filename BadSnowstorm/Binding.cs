using System;
using System.Linq;
using System.Linq.Expressions;

namespace BadSnowstorm
{
    public class Binding<TView, TViewModel>
        where TView : IViewBase
        where TViewModel : ViewModel
    {
        private readonly Func<TView, IContentArea> _getContentArea;
        private readonly Func<TViewModel, string> _getContent;

        internal Binding(Func<TView, IContentArea> getContentArea, Func<TViewModel, string> getContent)
        {
            _getContentArea = getContentArea;
            _getContent = getContent;
        }

        public static Binding<TView, TViewModel> Create(
            Expression<Func<TView, IContentArea>> contentAreaExpression,
            Expression<Func<TViewModel, string>> viewModelPropertyExpression)
        {
            return Create(
                contentAreaExpression,
                viewModelPropertyExpression,
                value => value);
        }

        public static Binding<TView, TViewModel> Create(
            Expression<Func<TView, IContentArea>> contentAreaExpression,
            Expression<Func<TViewModel, Input>> viewModelPropertyExpression)
        {
            return Create(contentAreaExpression, viewModelPropertyExpression, input => input.GetContent());
        }

        public static Binding<TView, TViewModel> Create<TViewModelValueType>(
            Expression<Func<TView, IContentArea>> contentAreaExpression,
            Expression<Func<TViewModel, TViewModelValueType>> viewModelPropertyExpression,
            Func<TViewModelValueType, string> formatPropertyValue)
        {
            var getContentArea = contentAreaExpression.Compile();
            var getPropertyValue = viewModelPropertyExpression.Compile();

            return new Binding<TView, TViewModel>(
                getContentArea,
                viewModel => formatPropertyValue(getPropertyValue(viewModel)));
        }

        public void Render(TView view, IConsole console)
        {
            var contentArea = GetContentArea(view);
            contentArea.Content = GetContent((TViewModel)view.ViewModel);
            contentArea.RenderContent(console);
        }

        internal IContentArea GetContentArea(TView view)
        {
            return _getContentArea(view);
        }

        internal string GetContent(TViewModel viewModel)
        {
            return _getContent(viewModel);
        }
    }
}