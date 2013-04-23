namespace BadSnowstorm
{
    public interface IViewFactory
    {
        IView CreateView(ViewModel viewModel);
    }
}