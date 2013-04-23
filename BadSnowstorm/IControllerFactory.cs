namespace BadSnowstorm
{
    public interface IControllerFactory
    {
        Controller Create<TController>() where TController : Controller;
    }
}