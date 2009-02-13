using System.Linq;

namespace Bindable.Linq.Interfaces
{
    /// <summary>
    /// Implemented by classes that represent a bindable collection of objects with a common key.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TElement">The type of the element.</typeparam>
    public interface IBindableGrouping<TKey, TElement> : IGrouping<TKey, TElement>, IBindableCollection<TElement>
    {
        
    }
}