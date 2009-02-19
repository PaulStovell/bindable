using System;
using System.Linq.Expressions;
using Bindable.Core.Helpers;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
	{
        /// <summary>
        /// Projects each element of a sequence to an <see cref="IBindableCollection{TElement}"/> and flattens the resulting sequences into one sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">The type of the elements of the sequence returned by <paramref name="selector"/>.</typeparam>
        /// <param name="source">A sequence of values to project.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>
        /// An <see cref="IBindableCollection{TElement}"/> whose elements are the result of invoking the one-to-many transform function on each element of the input sequence.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="source"/> or <paramref name="selector"/> is null.</exception>
	    public static IBindableCollection<TResult> SelectMany<TSource, TResult>(this IBindableCollection<TSource> source, Expression<Func<TSource, IBindableCollection<TResult>>> selector)
	        where TSource : class
	        where TResult : class
	    {
	        return SelectMany(source, selector, DefaultDependencyAnalysis);
	    }

	    /// <summary>
        /// Projects each element of a sequence to an <see cref="IBindableCollection{TElement}"/> and flattens the resulting sequences into one sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">The type of the elements of the sequence returned by <paramref name="selector"/>.</typeparam>
        /// <param name="source">A sequence of values to project.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <param name="dependencyAnalysisMode">The dependency analysis mode.</param>
        /// <returns>
        /// An <see cref="IBindableCollection{TElement}"/> whose elements are the result of invoking the one-to-many transform function on each element of the input sequence.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static IBindableCollection<TResult> SelectMany<TSource, TResult>(this IBindableCollection<TSource> source, Expression<Func<TSource, IBindableCollection<TResult>>> selector, DependencyDiscovery dependencyAnalysisMode)
            where TSource : class
            where TResult : class
        {
            Guard.NotNull(source, "source");
            return source.Select(selector, dependencyAnalysisMode).UnionAll();
        }
	}
}
