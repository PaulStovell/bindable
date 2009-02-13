using System.Collections.Generic;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;
using Bindable.Core.Helpers;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
	{
        /// <summary>
        /// Returns distinct elements from a sequence by using the default equality comparer to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to remove duplicate elements from.</param>
        /// <returns>
        /// An <see cref="IBindableCollection{TElement}"/> that contains distinct elements from the source sequence.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="source"/> is null.</exception>
        public static IBindableCollection<TSource> Distinct<TSource>(this IBindableCollection<TSource> source) where TSource : class
        {
            return source.Distinct(null);
        }

        /// <summary>
        /// Returns distinct elements from a sequence by using a specified <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to remove duplicate elements from.</param>
        /// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> to compare values.</param>
        /// <returns>
        /// An <see cref="IBindableCollection{TElement}"/> that contains distinct elements from the source sequence.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="source"/> is null.</exception>
        public static IBindableCollection<TSource> Distinct<TSource>(this IBindableCollection<TSource> source, IEqualityComparer<TSource> comparer) where TSource : class
        {
            if (comparer == null)
            {
                comparer = new DefaultComparer<TSource>();
            }
            return source.GroupBy(c => comparer.GetHashCode(c), DependencyDiscovery.Disabled).Select(group => group.First().Current, DependencyDiscovery.Disabled);
        }
	}
}
