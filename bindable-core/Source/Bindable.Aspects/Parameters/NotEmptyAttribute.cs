using System;
using System.Diagnostics;
using System.Reflection;
using Bindable.Aspects.Parameters;

namespace Bindable.Aspects.Parameters
{
    /// <summary>
    /// Throws an error for all parameters it is applied to if the parameter is an empty string.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class NotEmptyAttribute : AbstractParameterValidationAttribute
    {
        [DebuggerNonUserCode]
        public override void ValidateParameter(ParameterInfo parameter, object value)
        {
            if (value is string && ((string)value).Trim().Length == 0)
            {
                throw new ArgumentException(string.Format("The parameter '{0}' passed to method '{1}' did not contain any non-whitespace characters.", parameter.Name, parameter.Member.Name));
            }
        }
    }
}