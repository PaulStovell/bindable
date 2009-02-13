using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Bindable.Cms.Web.Application.Helpers;
using System.ComponentModel;

namespace Bindable.Cms.Web.Application.Framework.NVelocitySupport
{
    /// <summary>
    /// Captures metadata about instance or extension methods so that they can be invoked dynamically from NVelocity without too 
    /// much overhead. This class would lend itself well to use in multiple dispatch scenarios.
    /// </summary>
    public class DynamicDispatchMethod
    {
        private readonly MethodInfo _method;
        private readonly string _methodName;
        private readonly Type[] _parameters;
        private readonly bool _takesParamArray;
        private readonly Type _paramArrayType;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicDispatchMethod"/> class.
        /// </summary>
        /// <param name="method">The method.</param>
        public DynamicDispatchMethod(MethodInfo method)
        {
            var parameters = method.GetParameters();
            _method = method;
            _methodName = method.Name;
            _parameters = parameters.Select(p => p.ParameterType).ToArray();
            _takesParamArray = parameters.Length > 0 && parameters[parameters.Length - 1].GetCustomAttributes(typeof(ParamArrayAttribute), true).Length > 0;
            if (_takesParamArray)
            {
                _paramArrayType = _parameters[_parameters.Length - 1].GetElementType();
            }
        }

        /// <summary>
        /// Determines whether this instance is compatable for the given method call.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="args">The args.</param>
        public int MatchAndRank(string method, Type[] args)
        {
            // Make a few basic assumptions
            if (method != _methodName) return -1;
            if (!_takesParamArray && _parameters.Length != args.Length) return -1;

            var score = 10;
            for (var i = 0; i < Math.Max(args.Length, _parameters.Length); i++)
            {
                if (i >= args.Length)
                {
                    // We are out of arguments. Because of the assertion near the top, this means the next parameter is the params array, but we 
                    // are out of arguments. That's OK, but it should be ranked below all other alternatives.
                    return 1;
                }

                var given = args[i];
                var expected = 
                    (i < _parameters.Length 
                    ? (i == _parameters.Length - 1 && _takesParamArray 
                        ? _parameters[i].GetElementType() 
                        : _parameters[i]) 
                    : _paramArrayType);

                if (given == null && expected.IsValueType) return -1;
                if (given != null && expected == given) score += 10;
                else if (given == null || expected.IsAssignableFrom(given)) score += 5;
                else return -1;
            }
            return score;
        }

        /// <summary>
        /// Invokes the method.
        /// </summary>
        /// <param name="on">The object instance to invoke the method on.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public object Call(object on, object[] args)
        {
            var sanitizedArguments = new List<object>();
            var paramArrayElements = new List<object>();
            for (var i = 0; i < args.Length; i++)
            {
                var expectedType =
                    (i < _parameters.Length
                    ? (i == _parameters.Length - 1 && _takesParamArray
                        ? _parameters[i].GetElementType()
                        : _parameters[i])
                    : _paramArrayType);

                var sanitized = args[i];
                if (sanitized != null)
                {
                    if (sanitized.GetType() != expectedType)
                    {
                        // TODO: Use conversion/implicit assignment operators here
                    }
                }

                if (i >= _parameters.Length - 1 && _takesParamArray)
                {
                    paramArrayElements.Add(sanitized);
                }
                else
                {
                    sanitizedArguments.Add(sanitized);
                }
            }

            if (_takesParamArray)
            {
                if (paramArrayElements.Count == 0) sanitizedArguments.Add(null);
                else
                {
                    var paramArray = (Array)Activator.CreateInstance(_paramArrayType.MakeArrayType(), paramArrayElements.Count);
                    for (var i = 0; i < paramArrayElements.Count; i++)
                    {
                        paramArray.SetValue(paramArrayElements[i], i);
                    }
                    sanitizedArguments.Add(paramArray);
                }
            }

            return _method.Invoke(on, sanitizedArguments.ToArray());
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            var result = new StringBuilder();
            var parameters = _method.GetParameters();
            result.AppendFormat("{0}.{1}(", _method.DeclaringType.FullName, _method.Name);
            for (var i = 0; i < parameters.Length; i++)
            {
                result.Append(ReflectionHelper.DescribeParameter(parameters[i]));
                if (i < _parameters.Length - 1)
                {
                    result.Append(", ");
                }
            }
            result.AppendLine(")");
            return result.ToString().Trim();
        }

        public class ArgumentJudge
        {
            public int Score { get; set; }

            public bool CompareNext(Type given, Type expected)
            {
                
                return true;
            }
        }
    }
}