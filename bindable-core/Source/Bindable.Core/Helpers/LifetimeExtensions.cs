using Bindable.Core.Helpers;

namespace Bindable.Core.Helpers
{
    /// <summary>
    /// Extensions for adding objects to a Lifetime Couplings manager.
    /// </summary>
    public static class LifetimeExtensions
    {
        /// <summary>
        /// Remembers the specified item for the lifetime of the host of the LifetimeCouplings object.
        /// </summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="item">The item.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <returns></returns>
        public static TItem KeepAlive<TItem>(this TItem item, LifetimeCouplings lifetime)
        {
            lifetime.Add(item);
            return item;
        }
    }
}