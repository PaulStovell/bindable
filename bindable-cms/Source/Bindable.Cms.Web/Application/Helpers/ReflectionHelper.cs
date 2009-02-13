using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bindable.Cms.Web.Application.Helpers
{
    public class ReflectionHelper
    {
        public static Type[] GetTypesFromArguments(object[] args)
        {
            var list = new List<Type>();
            foreach (var arg in args)
            {
                if (arg == null)
                {
                    list.Add(null);
                }
                else
                {
                    list.Add(arg.GetType());
                }
            }
            return list.ToArray();
        }

        public static string DescribeType(Type type)
        {
            if (type == null) return "<null>";

            var result = string.Empty;
            result = type.Name;
            if (type.IsGenericType)
            {
                result = result.Substring(0, result.IndexOf("`"));
                result += "<";
                var args = type.GetGenericArguments();
                for (var i = 0; i < args.Length; i++)
                {
                    result += DescribeType(args[i]);
                    if (i < args.Length - 1)
                    {
                        result += ",";
                    }
                }
                result += ">";
            }
            if (type.IsArray)
            {
                result += "[]";
            }
            return result;
        }

        public static string DescribeParameter(ParameterInfo parameter)
        {
            var result = string.Empty;
            if (parameter.IsOut) result += "out ";
            if (parameter.IsRetval) result += "ref ";
            if (parameter.GetCustomAttributes(typeof(ParamArrayAttribute), true).Length > 0) result += "params ";
            result += DescribeType(parameter.ParameterType);
            return result.Trim();
        }
    }
}