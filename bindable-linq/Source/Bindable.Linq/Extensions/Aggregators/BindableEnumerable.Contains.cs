using System.Collections.Generic;
using Bindable.Core.Helpers;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
	{
        /// <summary>Determines whether a sequence contains a specified element by using the default equality comparer.</summary>
        /// <returns>true if the source sequence contains an element that has the specified value; otherwise, false.</returns>
        /// <param name="source">A sequence in which to locate a value.</param>
        /// <param name="value">The value to locate in the sequence.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is null.</exception>
        public static IBindable<bool> Contains<TSource>(this IBindableCollection<TSource> source, TSource value) where TSource : class
        {
            return source.Contains(value, null);
        }

        /// <summary>Determines whether a sequence contains a specified element by using a specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.</summary>
        /// <returns>true if the source sequence contains an element that has the specified value; otherwise, false.</returns>
        /// <param name="source">A sequence in which to locate a value.</param>
        /// <param name="value">The value to locate in the sequence.</param>
        /// <param name="comparer">An equality comparer to compare values.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is null.</exception>
        public static IBindable<bool> Contains<TSource>(this IBindableCollection<TSource> source, TSource value, IEqualityComparer<TSource> comparer) where TSource : class
        {
            comparer = comparer ?? new DefaultComparer<TSource>();
            value.ShouldNotBeNull("value");
            return source.Where(s => comparer.Equals(s, value)).Any();
        }
	}
}
