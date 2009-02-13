using System;
using System.Diagnostics;
using PostSharp.Laos;

namespace Bindable.Cms.Domain.Framework.Aspects
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, AllowMultiple = false), Serializable]
    public class SealAttribute : OnMethodBoundaryAspect
    {
        public override void CompileTimeInitialize(System.Reflection.MethodBase method)
        {
            base.CompileTimeInitialize(method);

            if (!typeof(ISealable).IsAssignableFrom(method.DeclaringType))
            {
                throw new Exception(string.Format("The property '{0}' on type '{1}' is marked with the [Seal] attribute, but the declaring type does not implement the ISealable interface. Make '{1}' implement ISealable (implicitly).", method.Name, method.DeclaringType));
            }
        }

        [DebuggerNonUserCode]
        public override void OnExit(MethodExecutionEventArgs eventArgs)
        {
            var sealable = eventArgs.Instance as ISealable;
            if (sealable != null && sealable.IsSealed)
            {
                throw new InvalidOperationException(string.Format("The method '{0}' on type '{1}' could not be invoked because the instance has been marked as sealed.", eventArgs.Method.Name, eventArgs.Method.DeclaringType.Name));
            }
        }
    }
}
