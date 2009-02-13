using System;
using System.Diagnostics;
using System.Reflection;
using PostSharp.Laos;

namespace Bindable.Aspects.Parameters
{
    /// <summary>
    /// Must be applied to a method if the <see>NotNullAttribute</see> attribute is on a parameter
    /// This is temporary until PostSharp gets a OnParameterBoundaryAspect.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, AllowMultiple = false), Serializable]
    public class NeverNullAttribute : OnMethodBoundaryAspect
    {
        public override void CompileTimeInitialize(MethodBase method)
        {
            base.CompileTimeInitialize(method);

            if (((MethodInfo)method).ReturnType != typeof(string))
            {
                throw new Exception(string.Format("Method '{0}' on class '{1}' has the attribute NeverNull applied, but the property is not a string The NeverNull attribute can only be applied to properties of type string.", method.Name, method.DeclaringType.Name));
            }
        }

        [DebuggerNonUserCode]
        public override void OnExit(MethodExecutionEventArgs eventArgs)
        {
            if (!eventArgs.Method.Name.StartsWith("get_") && ((MethodInfo)eventArgs.Method).ReturnType != typeof(string)) return;

            eventArgs.ReturnValue = eventArgs.ReturnValue ?? string.Empty;
        }
    }
}