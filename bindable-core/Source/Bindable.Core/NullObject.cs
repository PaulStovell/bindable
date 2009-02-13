using System;
using System.Collections.Generic;
using Bindable.Core.ObjectGeneration;

namespace Bindable.Core
{
    /// <summary>
    /// Provides null object pattern implementations for given interfaces.
    /// </summary>
    public static class NullObject
    {
        private static readonly Dictionary<Type, NullObjectBuilder> _objectBuilders = new Dictionary<Type, NullObjectBuilder>();

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
            if (!_objectBuilders.ContainsKey(interfaceType))
            {
                _objectBuilders.Add(interfaceType, new NullObjectBuilder(interfaceType));
            }
            return _objectBuilders[interfaceType].Instantiate();
        }
    }
}