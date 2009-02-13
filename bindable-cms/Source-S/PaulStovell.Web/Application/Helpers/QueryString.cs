using System;
using System.ComponentModel;
using System.Web;

namespace PaulStovell.Web.Application.Helpers
{
    public class QueryString
    {
        public static T GetOptional<T>(string paramName, T defaultValue)
        {
            try
            {
                var value = HttpContext.Current.Request.QueryString[paramName];
                if (value == null || value.Trim().Length == 0)
                {
                    return defaultValue;
                }

                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter == null)
                {
                    return (T) Convert.ChangeType(value, typeof(T));
                }
                if (converter.CanConvertFrom(typeof(string)))
                {
                    return (T) converter.ConvertFromString(value);
                }
            }
            catch { }
            return defaultValue;
        }

        public static T GetRequired<T>(string paramName)
        {
            var value = HttpContext.Current.Request.QueryString[paramName];
            if (value == null || value.Trim().Length == 0)
            {
                throw new Exception(string.Format("Query string parameter '{0}' is required.", paramName));
            }

            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter == null)
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            if (converter.CanConvertFrom(typeof(string)))
            {
                return (T)converter.ConvertFromString(value);
            }
            throw new Exception(string.Format("Query string parameter '{0}' is required.", paramName));
        }
    }
}