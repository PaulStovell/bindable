using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
	{
        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in ascending order according to a key.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <param name="source">An <see cref="T:System.Linq.IOrderedEnumerable`1"/> that contains elements to sort.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <returns>
        /// An <see cref="T:System.Linq.IOrderedEnumerable`1"/> whose elements are sorted according to a key.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
	    public static IOrderedBindableCollection<TSource> ThenBy<TSource, TKey>(this IOrderedBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector) where TSource : class
	    {
	        return ThenBy(source, keySelector, DefaultDependencyAnalysis);
        }

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in ascending order by using a specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <param name="source">An <see cref="T:System.Linq.IOrderedEnumerable`1"/> that contains elements to sort.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="comparer">An <see cref="T:System.Collections.Generic.IComparer`1"/> to compare keys.</param>
        /// <returns>
        /// An <see cref="T:System.Linq.IOrderedEnumerable`1"/> whose elements are sorted according to a key.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        public static IOrderedBindableCollection<TSource> ThenBy<TSource, TKey>(this IOrderedBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer) where TSource : class
        {
            return ThenBy(source, keySelector, comparer, DefaultDependencyAnalysis);
        }

	    /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in ascending order according to a key.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <param name="source">An <see cref="T:System.Linq.IOrderedEnumerable`1"/> that contains elements to sort.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="dependencyAnalysisMode">The dependency analysis mode.</param>
        /// <returns>
        /// An <see cref="T:System.Linq.IOrderedEnumerable`1"/> whose elements are sorted according to a key.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        public static IOrderedBindableCollection<TSource> ThenBy<TSource, TKey>(this IOrderedBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, DependencyDiscovery dependencyAnalysisMode) where TSource : class
        {
            return source.ThenBy(keySelector, null, dependencyAnalysisMode);
        }

	    /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in ascending order by using a specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <param name="source">An <see cref="T:System.Linq.IOrderedEnumerable`1"/> that contains elements to sort.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="comparer">An <see cref="T:System.Collections.Generic.IComparer`1"/> to compare keys.</param>
        /// <param name="dependencyAnalysisMode">The dependency analysis mode.</param>
        /// <returns>
        /// An <see cref="T:System.Linq.IOrderedEnumerable`1"/> whose elements are sorted according to a key.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        public static IOrderedBindableCollection<TSource> ThenBy<TSource, TKey>(this IOrderedBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer, DependencyDiscovery dependencyAnalysisMode) where TSource : class
        {
            source.ShouldNotBeNull("source");
            keySelector.ShouldNotBeNull("keySelector");
            var result = source.CreateOrderedIterator(keySelector.Compile(), comparer, false);
            if (dependencyAnalysisMode == DependencyDiscovery.Enabled)
            {
                return result.DependsOnExpression(keySelector.Body, keySelector.Parameters[0]);
            }
            return result;
        }
	}
}
