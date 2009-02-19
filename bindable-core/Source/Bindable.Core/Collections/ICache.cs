using System;

namespace Bindable.Core.Collections
{
    /// <summary>
    /// Implemented by classes whihc provide a very simple mechanism for storing and retreiving objects using lambdas.
    /// The use of lambda predicates implies O(n) lookup speed, so this cache is intended only for relatively small numbers of items.
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// Gets an item using the specified filter.
        /// </summary>
        /// <typeparam name="T">The type of item to search for.</typeparam>
        /// <param name="filter">The predicate to use to find the item.</param>
        /// <returns>
        /// The item if found, otherwise the default for the type <typeparamref name="T"/>.
        /// </returns>
        T Get<T>(Func<T, bool> filter);

        /// <summary>
        /// Gets an item using the specified filter, or creates it if none was found.
        /// </summary>
        /// <typeparam name="T">The type of item to search for.</typeparam>
        /// <param name="filter">The predicate to use to find the item.</param>
        /// <param name="create">The delegate to use to create the item.</param>
        /// <returns>
        /// The item if found, otherwise the default for the type <typeparamref name="T"/>.
        /// </returns>
        T GetOrCreate<T>(Func<T, bool> filter, Func<T> create);

        /// <summary>
        /// Adds the specified entry to the cache.
        /// </summary>
        /// <typeparam name="T">The type of item to add.</typeparam>
        /// <param name="t">The t.</param>
        void Put<T>(T t);
    }
}