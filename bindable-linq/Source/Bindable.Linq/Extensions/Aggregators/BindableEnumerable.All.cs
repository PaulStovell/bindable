using System;
using System.Linq.Expressions;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
	{
        /// <summary>
        /// Determines whether all elements of a sequence satisfy a condition.
        /// </summary>
        /// <param name="source">An <see cref="IBindableCollection{TElement}" /> that contains the elements to apply the predicate to.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <returns>
        /// true if every element of the source sequence passes the test in the specified 
        /// predicate, or if the sequence is empty; otherwise, false.
        /// </returns>
        public static IBindable<bool> All<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, bool>> predicate) where TSource : class
        {
            source.ShouldNotBeNull("source");
            predicate.ShouldNotBeNull("predicate");
            return source.Where(predicate).Count()
                .Switch()
                    .Case(count => count >= 1, true)
                    .Default(false)
                .EndSwitch();
        }
	}
}
