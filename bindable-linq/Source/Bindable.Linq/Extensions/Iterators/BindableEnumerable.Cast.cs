using Bindable.Linq.Interfaces;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
	{
        /// <summary>
        /// Converts the elements of an <see cref="IBindableCollection"/> to the specified type.
        /// </summary>
        /// <typeparam name="TResult">The type to convert the elements of <paramref name="source"/> to.</typeparam>
        /// <param name="source">The <see cref="IBindableCollection"/> that contains the elements to be converted.</param>
        /// <returns>
        /// An <see cref="IBindableCollection{TElement}"/> that contains each element of the source sequence converted to the specified type.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="source"/> is null.</exception>
        /// <exception cref="T:System.InvalidCastException">An element in the sequence cannot be cast to the required type.</exception>
        public static IBindableCollection<TResult> Cast<TResult>(this IBindableCollection source) where TResult : class
        {
            return AsBindable<TResult>(source, source.Dispatcher);
        }
	}
}
