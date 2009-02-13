using System;
using System.Linq.Expressions;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
	{
        /// <summary>Returns the first element of a sequence, or a default value if the sequence contains no elements.</summary>
        /// <returns>default(<typeparamref name="TSource" />) if <paramref name="source" /> is empty; otherwise, the first element in <paramref name="source" />.</returns>
        /// <param name="source">The <see cref="IBindableCollection{TElement}" /> to return the first element of.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is null.</exception>
        public static IBindable<TSource> First<TSource>(this IBindableCollection<TSource> source)
        {
            return source.FirstOrDefault();
        }

        /// <summary>Returns the first element of the sequence that satisfies a condition or a default value if no such element is found.</summary>
        /// <returns>default(<typeparamref name="TSource" />) if <paramref name="source" /> is empty or if no element passes the test specified by <paramref name="predicate" />; otherwise, the first element in <paramref name="source" /> that passes the test specified by <paramref name="predicate" />.</returns>
        /// <param name="source">An <see cref="IBindableCollection{TElement}" /> to return an element from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="predicate" /> is null.</exception>
        public static IBindable<TSource> First<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, bool>> predicate) where TSource : class
        {
            return source.FirstOrDefault(predicate);
        }

        /// <summary>Returns the first element of a sequence, or a default value if the sequence contains no elements.</summary>
        /// <returns>default(<typeparamref name="TSource" />) if <paramref name="source" /> is empty; otherwise, the first element in <paramref name="source" />.</returns>
        /// <param name="source">The <see cref="IBindableCollection{TElement}" /> to return the first element of.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is null.</exception>
        public static IBindable<TSource> FirstOrDefault<TSource>(this IBindableCollection<TSource> source)
        {
            source.ShouldNotBeNull("source");
            return source.ElementAtOrDefault(0);
        }

        /// <summary>Returns the first element of the sequence that satisfies a condition or a default value if no such element is found.</summary>
        /// <returns>default(<typeparamref name="TSource" />) if <paramref name="source" /> is empty or if no element passes the test specified by <paramref name="predicate" />; otherwise, the first element in <paramref name="source" /> that passes the test specified by <paramref name="predicate" />.</returns>
        /// <param name="source">An <see cref="IBindableCollection{TElement}" /> to return an element from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="predicate" /> is null.</exception>
        public static IBindable<TSource> FirstOrDefault<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, bool>> predicate) where TSource : class
        {
            return source.Where(predicate).FirstOrDefault();
        }
	}
}
