using System;

namespace BadSnowstorm
{
    public class DefaultControllerFactory : IControllerFactory
    {
        public Controller Create<TController>()
            where TController : Controller
        {
            // Since other implementations of IControllerFactory might not have the default constructor restrition,
            // IControllerFactory does not have a new() generic constraint on TController. Because of this, we need
            // to manually ensure that TController has a default constructor.
            typeof(TController).ThrowIfNoDefaultConstructor();
            return Activator.CreateInstance<TController>();
        }
    }
}