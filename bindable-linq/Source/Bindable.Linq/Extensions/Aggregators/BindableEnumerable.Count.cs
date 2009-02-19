using System;
using System.Linq.Expressions;
using Bindable.Core.Helpers;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
    {
        /// <summary>Returns the number of elements in a sequence.</summary>
        /// <returns>The number of elements in the input sequence.</returns>
        /// <param name="source">A sequence that contains elements to be counted.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.OverflowException">The number of elements in <paramref name="source" /> is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
        public static IBindable<int> Count<TSource>(this IBindableCollection<TSource> source)
        {
            return Aggregate(source, sources => sources.Count);
        }

        /// <summary>Returns a number that represents how many elements in the specified sequence satisfy a condition.</summary>
        /// <returns>A number that represents how many elements in the sequence satisfy the condition in the predicate function.</returns>
        /// <param name="source">A sequence that contains elements to be tested and counted.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="predicate" /> is null.</exception>
        /// <exception cref="T:System.OverflowException">The number of elements in <paramref name="source" /> is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
        public static IBindable<int> Count<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, bool>> predicate) where TSource : class
        {
            Guard.NotNull(predicate, "predicate");
            return source.Where(predicate).Count();
        }
	}
}
