using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BadSnowstorm
{
    public class DefaultViewFactory : IViewFactory
    {
        private readonly Dictionary<Type, Func<IView>> _factoryMethods = new Dictionary<Type, Func<IView>>();

        public DefaultViewFactory()
        {
            InitializeViewFactory();
        }

        public IView CreateView(ViewModel viewModel)
        {
            if (!_factoryMethods.ContainsKey(viewModel.GetType()))
            {
                // If the viewModel type is unknown, it is technically possible that an assembly was loaded after we last checked, so reinitialize.
                InitializeViewFactory();

                if (!_factoryMethods.ContainsKey(viewModel.GetType()))
                {
                    // Ok, we really, *really* tried to find the view. Sorry about the exception. :(
                    throw new Exception(String.Format("Unable to locate a View that corresponds to ViewModel type: {0}.", viewModel.GetType()));
                }
            }

            return _factoryMethods[viewModel.GetType()]();
        }

        private void InitializeViewFactory()
        {
            var viewModelRegex = new Regex("^([A-Za-z_][A-Za-z0-9_]*?)ViewModel$");

            var viewAndViewModelTypes =
                (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                 where assembly.GetName().Name != "mscorlib" && !assembly.GetName().Name.StartsWith("System") && !assembly.GetName().Name.StartsWith("Microsoft")
                 from type in assembly.GetTypes()
                 where !type.IsAbstract && (typeof(ViewModel).IsAssignableFrom(type) || typeof(IViewBase).IsAssignableFrom(type))
                 select type).ToList();

            foreach (var viewModelType in viewAndViewModelTypes.Where(t => typeof(ViewModel).IsAssignableFrom(t)))
            {
                if (!viewModelRegex.IsMatch(viewModelType.Name))
                {
                    throw new Exception("ViewModel class name must be in the format '______ViewModel'. For example, 'MainMenuViewModel'");
                }

                var viewModelName = viewModelRegex.Match(viewModelType.Name).Groups[1].Value;

                var viewRegex = new Regex(string.Format(@"{0}View(?:`\d+)?", viewModelName));

                var viewType =
                    viewAndViewModelTypes
                        .SingleOrDefault(t =>
                            typeof(IViewBase).IsAssignableFrom(t)
                            && viewModelType.Namespace == t.Namespace
                            && viewRegex.IsMatch(t.Name));

                viewType.ThrowIfNull(string.Format("View with name '{0}View' not found.", viewModelName));

                if (!IsPartialView(viewType))
                {
                    viewType.ThrowIfNoDefaultConstructor();

                    // ReSharper disable AssignNullToNotNullAttribute
                    _factoryMethods[viewModelType] = () => (IView)Activator.CreateInstance(viewType);
                    // ReSharper restore AssignNullToNotNullAttribute
                }
            }
        }

        private bool IsPartialView(Type t)
        {
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(PartialView<,,,>))
            {
                return true;
            }

            if (t == typeof(object))
            {
                return false;
            }

            return IsPartialView(t.BaseType);
        }
    }
}