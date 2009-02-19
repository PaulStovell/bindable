using Bindable.Core.Helpers;
using Bindable.Linq.Aggregators;
using Bindable.Linq.Interfaces;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
	{
        /// <summary>Returns the element at a specified index in a sequence or a default value if the index is out of range.</summary>
        /// <returns>default(<typeparamref name="TSource" />) if the index is outside the bounds of the source sequence; otherwise, the element at the specified position in the source sequence.</returns>
        /// <param name="source">An <see cref="IBindableCollection{TElement}" /> to return an element from.</param>
        /// <param name="index">The zero-based index of the element to retrieve.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is null.</exception>
        public static IBindable<TSource> ElementAtOrDefault<TSource>(this IBindableCollection<TSource> source, int index)
        {
            Guard.NotNull(source, "source");
            return new ElementAtAggregator<TSource>(source, index, source.Dispatcher);
        }
	}
}
