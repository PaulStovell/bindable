using Bindable.Linq.Interfaces;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
	{
        /// <summary>
        /// Filters the elements of an <see cref="IBindableCollection"/> based on a specified type.
        /// </summary>
        /// <typeparam name="TResult">The type to filter the elements of the sequence on.</typeparam>
        /// <param name="source">The <see cref="IBindableCollection"/> whose elements to filter.</param>
        /// <returns>
        /// An <see cref="IBindableCollection{TElement}"/> that contains elements from the input sequence of type <typeparamref name="TResult"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="source"/> is null.</exception>
        public static IBindableCollection<TResult> OfType<TResult>(this IBindableCollection source) where TResult : class
        {
            return AsBindable<object>(source, source.Dispatcher).Where(s => s is TResult, DependencyDiscovery.Disabled).Select(s => (TResult)s, DependencyDiscovery.Disabled);
        }
	}
}
