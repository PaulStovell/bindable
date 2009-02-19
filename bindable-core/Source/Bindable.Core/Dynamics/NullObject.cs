using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bindable.Core.Dynamics
{
    /// <summary>
    /// Provides null object pattern implementations for given interfaces.
    /// </summary>
    public static class NullObject
    {
        private static readonly Dictionary<Type, DelegatingTypeGenerator> _generators = new Dictionary<Type, DelegatingTypeGenerator>();

        /// <summary>
        /// Creates a null object pattern implementation for a given interface.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <returns></returns>
        public static TInterface For<TInterface>()
        {
            return (TInterface)For(typeof(TInterface));
        }

        /// <summary>
        /// Creates a null object pattern implementation for a given interface.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <returns></returns>
        public static object For(Type interfaceType)
        {
            var generator = _generators.GetOrCreate(interfaceType, 
                () => new DelegatingTypeGenerator(interfaceType)
                );

            return generator.Instantiate(new NullObjectImplementation());
        }

        private class NullObjectImplementation : IDynamicImplementation
        {
            public object HandleFunction(MethodBase method, object[] arguments)
            {
                var methodInfo = method as MethodInfo;
                if (methodInfo != null)
                {
                    if (methodInfo.ReturnType != null && methodInfo.ReturnType != typeof(void))
                    {
                        if (methodInfo.ReturnType.IsValueType)
                        {
                            return Activator.CreateInstance(methodInfo.ReturnType);
                        }
                        if (methodInfo.ReturnType.IsInterface)
                        {
                            return For(methodInfo.ReturnType);
                        }
                    }
                }
                return null;
            }

            public void HandleAction(MethodBase method, object[] arguments)
            {
            }
        }
    }
}