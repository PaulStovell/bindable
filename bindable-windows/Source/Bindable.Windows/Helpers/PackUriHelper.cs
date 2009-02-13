using System;
using System.Reflection;

namespace Bindable.Windows.Helpers
{
    public static class PackUriHelper
    {
        public static Uri Create(string path)
        {
            return Create(path, Assembly.GetCallingAssembly());
        }

        public static Uri Create(string path, Assembly assembly)
        {
            var assemblyName = assembly.GetName().Name;
            return Create(path, assemblyName);
        }

        public static Uri Create(string path, string assemblyName)
        {
            var resourceName = path;
            if (!resourceName.StartsWith("/"))
            {
                resourceName = "/" + resourceName;
            }
            if (assemblyName == null)
            {
                return new Uri(resourceName, UriKind.Absolute);
            }
            return new Uri(string.Format("pack://application:,,,/{0};Component{1}", assemblyName, resourceName));
        }
    }
}
