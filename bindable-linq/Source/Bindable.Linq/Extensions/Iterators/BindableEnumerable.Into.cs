using System;
using System.Linq.Expressions;
using Bindable.Linq.Interfaces;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
	{
        /// <summary>
        /// Projects the groups from a GroupBy into a new element type.
        /// </summary>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="resultSelector"/>.</typeparam>
        /// <typeparam name="TElement">The type of the elements in each <see cref="T:System.Linq.IGrouping`2"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result value returned by <paramref name="resultSelector"/>.</typeparam>
        /// <param name="source">An <see cref="IBindableCollection{TElement}"/> whose elements to group.</param>
        /// <param name="resultSelector">A function to create a result value from each group.</param>
        /// <returns>
        /// A collection of elements of type <typeparamref name="TResult"/> where each element represents a projection over a group and its key.
        /// </returns>
        public static IBindableCollection<TResult> Into<TKey, TElement, TResult>(this IBindableCollection<IBindableGrouping<TKey, TElement>> source, Expression<Func<TKey, IBindableCollection<TElement>, TResult>> resultSelector)
            where TElement : class
            where TResult : class
        {
            var func = resultSelector.Compile();
            return source.Select(g => func(g.Key, g), DependencyDiscovery.Disabled);
        }
	}
}
