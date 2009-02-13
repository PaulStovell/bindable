using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Iterators;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
	{
        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <returns>
        /// An <see cref="T:System.Linq.IOrderedEnumerable`1"/> whose elements are sorted according to a key.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
	    public static IOrderedBindableCollection<TSource> OrderBy<TSource, TKey>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector) where TSource : class
	    {
	        return OrderBy(source, keySelector, DefaultDependencyAnalysis);
        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order by using a specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="comparer">An <see cref="T:System.Collections.Generic.IComparer`1"/> to compare keys.</param>
        /// <returns>
        /// An <see cref="T:System.Linq.IOrderedEnumerable`1"/> whose elements are sorted according to a key.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        public static IOrderedBindableCollection<TSource> OrderBy<TSource, TKey>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer) where TSource : class
        {
            return OrderBy(source, keySelector, comparer, DefaultDependencyAnalysis);
        }

	    /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="dependencyAnalysisMode">The dependency analysis mode.</param>
        /// <returns>
        /// An <see cref="T:System.Linq.IOrderedEnumerable`1"/> whose elements are sorted according to a key.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        public static IOrderedBindableCollection<TSource> OrderBy<TSource, TKey>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, DependencyDiscovery dependencyAnalysisMode) where TSource : class
        {
            return source.OrderBy(keySelector, null, dependencyAnalysisMode);
        }

	    /// <summary>
        /// Sorts the elements of a sequence in ascending order by using a specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="comparer">An <see cref="T:System.Collections.Generic.IComparer`1"/> to compare keys.</param>
        /// <param name="dependencyAnalysisMode">The dependency analysis mode.</param>
        /// <returns>
        /// An <see cref="T:System.Linq.IOrderedEnumerable`1"/> whose elements are sorted according to a key.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        public static IOrderedBindableCollection<TSource> OrderBy<TSource, TKey>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer, DependencyDiscovery dependencyAnalysisMode) where TSource : class
        {
            source.ShouldNotBeNull("source");
            keySelector.ShouldNotBeNull("keySelector");
            var result = new OrderByIterator<TSource, TKey>(source, new ItemSorter<TSource, TKey>(null, keySelector.Compile(), comparer, true), source.Dispatcher);
            if (dependencyAnalysisMode == DependencyDiscovery.Enabled)
            {
                return result.DependsOnExpression(keySelector.Body, keySelector.Parameters[0]);
            }
            return result;
        }
	}
}
