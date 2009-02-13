using System;
using System.Linq.Expressions;
using Bindable.Linq.Interfaces;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
	{
        /// <summary>Returns the only element of a sequence, or a default value if the sequence is empty; this method throws an exception if there is more than one element in the sequence.</summary>
        /// <returns>The single element of the input sequence, or default(<typeparamref name="TSource" />) if the sequence contains no elements.</returns>
        /// <param name="source">An <see cref="IBindableCollection{TElement}" /> to return the single element of.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">The input sequence contains more than one element.</exception>
        public static IBindable<TSource> SingleOrDefault<TSource>(this IBindableCollection<TSource> source)
        {
            return source.FirstOrDefault();
        }

        /// <summary>Returns the only element of a sequence that satisfies a specified condition or a default value if no such element exists; this method throws an exception if more than one element satisfies the condition.</summary>
        /// <returns>The single element of the input sequence that satisfies the condition, or default(<typeparamref name="TSource" />) if no such element is found.</returns>
        /// <param name="source">An <see cref="IBindableCollection{TElement}" /> to return a single element from.</param>
        /// <param name="predicate">A function to test an element for a condition.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="predicate" /> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">More than one element satisfies the condition in <paramref name="predicate" />.</exception>
        public static IBindable<TSource> SingleOrDefault<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, bool>> predicate) where TSource : class
        {
            return source.FirstOrDefault(predicate);
        }
	}
}
