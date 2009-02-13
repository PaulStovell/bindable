using System;
using System.Linq.Expressions;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
	{
        /// <summary>
        /// Determines whether a sequence contains any elements.
        /// </summary>
        /// <param name="source">The <see cref="IBindableCollection{TElement}" /> to check for emptiness.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <returns>true if the source sequence contains any elements; otherwise, false.</returns>
        public static IBindable<bool> Any<TSource>(this IBindableCollection<TSource> source)
        {
            source.ShouldNotBeNull("source");
            return source.Count()
                .Switch()
                    .Case(count => count >= 1, true)
                    .Default(false)
                .EndSwitch();
        }

        /// <summary>
        /// Determines whether any element of a sequence satisfies a condition.
        /// </summary>
        /// <param name="source">An <see cref="IBindableCollection{TElement}" /> whose elements to apply the predicate to.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <returns>true if any elements in the source sequence pass the test in the specified predicate; otherwise, false.</returns>
        public static IBindable<bool> Any<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, bool>> predicate) where TSource : class
        {
            source.ShouldNotBeNull("source");
            predicate.ShouldNotBeNull("predicate");
            return source.Where(predicate).Any();
        }
	}
}
