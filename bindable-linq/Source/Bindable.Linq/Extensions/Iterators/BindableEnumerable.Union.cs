using System.Collections.Generic;
using Bindable.Linq.Collections;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Iterators;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
	{
        /// <summary>
        /// Produces the set union of two sequences by using the default equality comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
        /// <param name="first">An <see cref="IBindableCollection{TElement}"/> whose distinct elements form the first set for the union.</param>
        /// <param name="second">An <see cref="IBindableCollection{TElement}"/> whose distinct elements form the second set for the union.</param>
        /// <returns>
        /// An <see cref="IBindableCollection{TElement}"/> that contains the elements from both input sequences, excluding duplicates.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="first"/> or <paramref name="second"/> is null.</exception>
        public static IBindableCollection<TSource> Union<TSource>(this IBindableCollection<TSource> first, IBindableCollection<TSource> second) where TSource : class
        {
            return Union(first, second, null);
        }

        /// <summary>
        /// Produces the set union of two sequences by using a specified <see cref="T:System.Collections.Generic.IEqualityComparer`1"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
        /// <param name="first">An <see cref="IBindableCollection{TElement}"/> whose distinct elements form the first set for the union.</param>
        /// <param name="second">An <see cref="IBindableCollection{TElement}"/> whose distinct elements form the second set for the union.</param>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> to compare values.</param>
        /// <returns>
        /// An <see cref="IBindableCollection{TElement}"/> that contains the elements from both input sequences, excluding duplicates.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="first"/> or <paramref name="second"/> is null.</exception>
        public static IBindableCollection<TSource> Union<TSource>(this IBindableCollection<TSource> first, IBindableCollection<TSource> second, IEqualityComparer<TSource> comparer) where TSource : class
        {
            comparer = comparer ?? ElementComparerFactory.Create<TSource>();
            var sources = new BindableCollection<IBindableCollection<TSource>>(first.Dispatcher) { first, second };
            return new UnionIterator<TSource>(sources, first.Dispatcher).Distinct(comparer);
        }
	}
}
