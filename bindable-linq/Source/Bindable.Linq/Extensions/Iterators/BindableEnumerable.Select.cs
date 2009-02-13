using System;
using System.Linq.Expressions;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Iterators;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
    {
        /// <summary>
        /// Projects each element of a sequence into a new form.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to invoke a transform function on.</param>
        /// <returns>
        /// An <see cref="IBindableCollection{TElement}"/> whose elements are the result of invoking the transform function on each element of <paramref name="source"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IBindableCollection<TSource> Select<TSource>(this IBindableCollection<TSource> source) where TSource : class
        {
            return Select(source, DefaultDependencyAnalysis);
        }

        /// <summary>
        /// Projects each element of a sequence into a new form.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
        /// <param name="source">A sequence of values to invoke a transform function on.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>
        /// An <see cref="IBindableCollection{TElement}"/> whose elements are the result of invoking the transform function on each element of <paramref name="source"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static IBindableCollection<TResult> Select<TSource, TResult>(this IBindableCollection<TSource> source, Expression<Func<TSource, TResult>> selector) where TSource : class
        {
            return Select(source, selector, DefaultDependencyAnalysis);
        }

	    /// <summary>
        /// Projects each element of a sequence into a new form.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to invoke a transform function on.</param>
        /// <param name="dependencyAnalysisMode">The dependency analysis mode.</param>
        /// <returns>
        /// An <see cref="IBindableCollection{TElement}"/> whose elements are the result of invoking the transform function on each element of <paramref name="source"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IBindableCollection<TSource> Select<TSource>(this IBindableCollection<TSource> source, DependencyDiscovery dependencyAnalysisMode) where TSource : class
        {
            return source.Select(s => s, dependencyAnalysisMode);
        }

	    /// <summary>
        /// Projects each element of a sequence into a new form.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
        /// <param name="source">A sequence of values to invoke a transform function on.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <param name="dependencyAnalysisMode">The dependency analysis mode.</param>
        /// <returns>
        /// An <see cref="IBindableCollection{TElement}"/> whose elements are the result of invoking the transform function on each element of <paramref name="source"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static IBindableCollection<TResult> Select<TSource, TResult>(this IBindableCollection<TSource> source, Expression<Func<TSource, TResult>> selector, DependencyDiscovery dependencyAnalysisMode) where TSource : class
        {
            source.ShouldNotBeNull("source");
            selector.ShouldNotBeNull("selector");
            var result = new SelectIterator<TSource, TResult>(source, selector.Compile(), source.Dispatcher);
            if (dependencyAnalysisMode == DependencyDiscovery.Enabled)
            {
                result = result.DependsOnExpression(selector.Body, selector.Parameters[0]);
            }
            return result;
        }
	}
}
