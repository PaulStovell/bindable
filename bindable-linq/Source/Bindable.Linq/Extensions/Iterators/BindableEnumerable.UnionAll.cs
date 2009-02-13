using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Iterators;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
    {
        /// <summary>
        /// Produces the set union of multiple sequences.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
        /// <param name="sources">An <see cref="IBindableCollection{TElement}"/> whose elements are also <see cref="IBindableCollection{TElement}"/> of the elements to be combined.</param>
        /// <returns>
        /// An <see cref="IBindableCollection{TElement}"/> that contains the elements from both input sequences, excluding duplicates.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="sources"/> is null.</exception>
        public static IBindableCollection<TSource> UnionAll<TSource>(this IBindableCollection<IBindableCollection<TSource>> sources) where TSource : class
        {
            return new UnionIterator<TSource>(sources, sources.Dispatcher);
        }
	}
}
