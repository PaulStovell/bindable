using System;
using System.Linq.Expressions;
using Bindable.Linq.Interfaces;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
	{
        /// <summary>Returns the only element of a sequence, and throws an exception if there is not exactly one element in the sequence.</summary>
        /// <returns>The single element of the input sequence.</returns>
        /// <param name="source">An <see cref="IBindableCollection{TElement}" /> to return the single element of.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">The input sequence contains more than one element.-or-The input sequence is empty.</exception>
        public static IBindable<TSource> Single<TSource>(this IBindableCollection<TSource> source)
        {
            return source.FirstOrDefault();
        }

        /// <summary>Returns the only element of a sequence that satisfies a specified condition, and throws an exception if more than one such element exists.</summary>
        /// <returns>The single element of the input sequence that satisfies a condition.</returns>
        /// <param name="source">An <see cref="IBindableCollection{TElement}" /> to return a single element from.</param>
        /// <param name="predicate">A function to test an element for a condition.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="predicate" /> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">No element satisfies the condition in <paramref name="predicate" />.-or-More than one element satisfies the condition in <paramref name="predicate" />.-or-The source sequence is empty.</exception>
        public static IBindable<TSource> Single<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, bool>> predicate) where TSource : class
        {
            return source.FirstOrDefault(predicate);
        }
	}
}
