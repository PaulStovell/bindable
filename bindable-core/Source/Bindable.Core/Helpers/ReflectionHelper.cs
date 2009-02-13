using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.IO;

namespace Bindable.Core.Helpers
{
    public static class ReflectionHelper
    {
        public static R[] GetAttributesValue<T, R>(object o, Func<T, R> reader)
        {
            return GetAttributesValue<T, R>(o.GetType(), reader);
        }

        public static R[] GetAttributesValue<T, R>(object o, Func<T, R> reader, Func<T, bool> filter)
        {
            return GetAttributesValue<T, R>(o.GetType(), reader, filter);
        }

        public static R GetAttributeValue<T, R>(object o, Func<T, R> reader, R defaultValue)
        {
            return GetAttributeValue<T, R>(o.GetType(), reader, defaultValue);
        }

        public static R[] GetAttributesValue<T, R>(ICustomAttributeProvider o, Func<T, R> reader)
        {
            return o.GetCustomAttributes(typeof(T), true).Cast<T>().Select(reader).ToArray();
        }

        public static R[] GetAttributesValue<T, R>(ICustomAttributeProvider o, Func<T, R> reader, Func<T, bool> filter)
        {
            return o.GetCustomAttributes(typeof(T), true).Cast<T>().Where(filter).Select(reader).ToArray();
        }

        public static R GetAttributeValue<T, R>(ICustomAttributeProvider o, Func<T, R> reader, R defaultValue)
        {
            var values = GetAttributesValue(o, reader);
            return values.Length == 0 ? defaultValue : values.First();
        }

        public static Type[] FindConcreteTypesBasedOn(Type interfaceType, IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(a => a.GetTypes()).Where(type => interfaceType.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract).ToArray();
        }

        public static string GetAssemblyTitle(Assembly assembly)
        {
            return GetAttributeValue<AssemblyTitleAttribute, string>(assembly, att => att.Title, assembly.GetName().Name);
        }

        public static string GetAssemblyDescription(Assembly assembly)
        {
            return GetAttributeValue<AssemblyDescriptionAttribute, string>(assembly, att => att.Description, assembly.GetName().Name);
        }

        public static string GetExecutableFileName()
        {
            return Path.GetFileName(Assembly.GetExecutingAssembly().Location);
        }

        public static string GetExecutableTitle()
        {
            return GetAssemblyTitle(Assembly.GetExecutingAssembly());
        }

        public static string GetExecutableDescription()
        {
            return GetAssemblyDescription(Assembly.GetExecutingAssembly());
        }
    }
}