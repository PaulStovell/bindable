using System.ComponentModel;
using Bindable.Linq.Adapters.Outgoing;
using Bindable.Linq.Interfaces;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
	{
#if !SILVERLIGHT
        // Silverlight does not provide an IBindingList interface, so this code would 
        // not compile.

        /// <summary>
        /// Converts a Bindable LINQ query into a <see cref="IBindingList"/> compatible with Windows Forms and WPF.
        /// </summary>
        /// <typeparam name="TElement">The type of the element.</typeparam>
        /// <param name="bindableCollection">The bindable collection.</param>
        /// <returns></returns>
        public static IBindingList ToBindingList<TElement>(this IBindableCollection<TElement> bindableCollection) where TElement : class
        {
            return new BindingListAdapter<TElement>(bindableCollection, bindableCollection.Dispatcher);
        }
#endif
	}
}
