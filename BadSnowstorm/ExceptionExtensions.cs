using System;
using System.Linq;
using System.Reflection;

namespace BadSnowstorm
{
    public static class ExceptionExtensions
    {
        public static T ThrowIfNull<T>(this T instance, string message = null)
            where T : class
        {
            if (instance == null)
            {
                throw new NullReferenceException(message);
            }

            return instance;
        }

        public static T ThrowIfArgumentNull<T>(this T instance, string paramName)
            where T : class
        {
            if (instance == null)
            {
                throw new ArgumentNullException(paramName);
            }

            return instance;
        }

        public static Type ThrowIfNoDefaultConstructor(this Type type)
        {
            if (type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Count(c => c.GetParameters().Length == 0) != 1)
            {
                throw new InvalidOperationException(string.Format("No public parameterless constructor exists for type '{0}'", type));
            }

            return type;
        }
    }
}