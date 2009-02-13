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
        /// Bindable LINQ: Filters a sequence of values based on a predicate, and automatically detects dependencies. Each item will be re-evaluated if it raises a property changed event, or if any properties
        /// accessed in the filter expression raise events. Items will be re-evaluated when the source collection raises CollectionChanged events. The entire collection
        /// will be re-evaluated if the source collection raises a Reset event, or if any addtional dependencies added via the <see cref="WithDependencies{TResult}"/>
        /// extension method tell it to re-evaluate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="IBindableCollection{TElement}"/> to filter.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        /// An <see cref="IBindableCollection{TElement}"/> that contains elements from the input sequence that satisfy the condition.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
	    public static IBindableCollection<TSource> Where<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, bool>> predicate) where TSource : class
	    {
	        return Where(source, predicate, DefaultDependencyAnalysis);
	    }

	    /// <summary>
        /// Bindable LINQ: Filters a sequence of values based on a predicate. Each item will be re-evaluated if it raises a property changed event, or if any properties
        /// accessed in the filter expression raise events. Items will be re-evaluated when the source collection raises CollectionChanged events. The entire collection
        /// will be re-evaluated if the source collection raises a Reset event, or if any addtional dependencies added via the <see cref="WithDependencies{TResult}"/>
        /// extension method tell it to re-evaluate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="IBindableCollection{TElement}"/> to filter.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="dependencyAnalysisMode">The dependency analysis mode.</param>
        /// <returns>
        /// An <see cref="IBindableCollection{TElement}"/> that contains elements from the input sequence that satisfy the condition.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        public static IBindableCollection<TSource> Where<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, bool>> predicate, DependencyDiscovery dependencyAnalysisMode) where TSource : class
        {
            source.ShouldNotBeNull("source");
            predicate.ShouldNotBeNull("predicate");
            var result = new WhereIterator<TSource>(source, predicate.Compile(), source.Dispatcher);
            if (dependencyAnalysisMode == DependencyDiscovery.Enabled)
            {
                return result.DependsOnExpression(predicate.Body, predicate.Parameters[0]);
            }
	        return result;
        }
	}
}
