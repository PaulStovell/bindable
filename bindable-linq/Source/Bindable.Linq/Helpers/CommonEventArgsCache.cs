using System.Collections.Specialized;
using System.ComponentModel;

namespace Bindable.Linq.Helpers
{
    /// <summary>
    /// An internal cache for storing static <see cref="PropertyChangedEventArgs"/> instances.
    /// </summary>
    internal static class CommonEventArgsCache
    {
        public static readonly NotifyCollectionChangedEventArgs Reset = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
        public static readonly PropertyChangedEventArgs Count = new PropertyChangedEventArgs("Count");
        public static readonly PropertyChangedEventArgs Current = new PropertyChangedEventArgs("Current");
        public static readonly PropertyChangedEventArgs HasEvaluated = new PropertyChangedEventArgs("HasEvaluated");
    }
}