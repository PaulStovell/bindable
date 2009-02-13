using Bindable.Linq.Collections;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Iterators;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
	{
        /// <summary>
        /// Concatenates two sequences.
        /// </summary>
        /// <typeparam name="TElement">The type of the elements of the input sequences.</typeparam>
        /// <param name="first">The first sequence to concatenate.</param>
        /// <param name="second">The sequence to concatenate to the first sequence.</param>
        /// <returns>
        /// An <see cref="IBindableCollection{TElement}"/> that contains the concatenated elements of the two input sequences.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="first"/> or <paramref name="second"/> is null.</exception>
        public static IBindableCollection<TElement> Concat<TElement>(this IBindableCollection<TElement> first, IBindableCollection<TElement> second) where TElement : class
        {
            var sources = new BindableCollection<IBindableCollection<TElement>>(first.Dispatcher) { first, second };
            return new UnionIterator<TElement>(sources, first.Dispatcher);
        }
	}
}
