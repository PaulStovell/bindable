using System;
using System.Collections.Generic;
using System.Diagnostics;
using PostSharp.Laos;

namespace PaulStovell.Domain.Framework.Aspects
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, AllowMultiple = false), Serializable]
    public class OneTimeAttribute : OnMethodBoundaryAspect
    {
        private readonly Dictionary<int, List<string>> _setProperties = new Dictionary<int, List<string>>();

        [DebuggerNonUserCode]
        public override void OnEntry(MethodExecutionEventArgs eventArgs)
        {
            var key = eventArgs.Instance.GetHashCode();
            var set = null as List<string>;
            if (!_setProperties.ContainsKey(key))
            {
                set = new List<string>();
                _setProperties.Add(eventArgs.Instance.GetHashCode(), set);
            }
            else
            {
                set = _setProperties[key];
            }

            if (set.Contains(eventArgs.Method.Name))
            {
                throw new InvalidOperationException(
                    string.Format(
                        "The method '{0}' on type '{1}' could not be invoked because it has already been invoked and was declared as OneTime only.",
                        eventArgs.Method.Name, eventArgs.Method.DeclaringType.Name));
            }
            set.Add(eventArgs.Method.Name);
        }
    }
}