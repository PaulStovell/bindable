using System;
using System.Collections.Generic;
using System.Linq;

namespace Bindable.Core.Collections
{
    /// <summary>
    /// A simple implementation of <see cref="ICache"/> which just uses a list of items in memory.
    /// </summary>
    public class Cache : ICache
    {
        private readonly List<object> _cache = new List<object>();
        private readonly object _lock = new object();

        /// <summary>
        /// Gets an item using the specified filter.
        /// </summary>
        /// <typeparam name="T">The type of item to search for.</typeparam>
        /// <param name="filter">The predicate to use to find the item.</param>
        /// <returns>
        /// The item if found, otherwise the default for the type <typeparamref name="T"/>.
        /// </returns>
        public T Get<T>(Func<T, bool> filter)
        {
            lock (_lock)
            {
                return _cache.Cast<object>().Where(o => o is T).Select(o => (T)o).Where(filter).FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets an item using the specified filter, or creates it if none was found.
        /// </summary>
        /// <typeparam name="T">The type of item to search for.</typeparam>
        /// <param name="filter">The predicate to use to find the item.</param>
        /// <param name="create">The delegate to use to create the item.</param>
        /// <returns>
        /// The item if found, otherwise the default for the type <typeparamref name="T"/>.
        /// </returns>
        public T GetOrCreate<T>(Func<T, bool> filter, Func<T> create)
        {
            lock (_lock)
            {
                var existing = Get(filter);
                if (existing == null)
                {
                    existing = create();
                    Put(existing);
                }
                return existing;
            }
        }

        /// <summary>
        /// Adds the specified entry to the cache.
        /// </summary>
        /// <typeparam name="T">The type of item to add.</typeparam>
        /// <param name="t">The t.</param>
        public void Put<T>(T t)
        {
            lock (_lock)
            {
                _cache.Add(t);
            }
        }
    }
}
