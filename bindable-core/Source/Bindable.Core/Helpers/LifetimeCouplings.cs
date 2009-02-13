using System.Collections.Generic;
using System;

namespace Bindable.Core.Helpers
{
    /// <summary>
    /// Provides a way for object instances to be "coupled" to the hosting class's object lifetime. Essentially, it's just an object store that is used to store things that the 
    /// garbage collector would otherwise clean up. 
    /// </summary>
    public sealed class LifetimeCouplings : IDisposable
    {
        private readonly List<object> _objects;

        /// <summary>
        /// Initializes a new instance of the <see cref="LifetimeCouplings"/> class.
        /// </summary>
        public LifetimeCouplings()
        {
            _objects = new List<object>();
        }

        /// <summary>
        /// Adds an object that will be kept alive by the host class.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public void Add(object instance)
        {
            _objects.Add(instance);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _objects.Clear();
        }
    }
}