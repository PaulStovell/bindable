using System.Collections;
using System.Collections.Specialized;
using Bindable.Core.Helpers;
using Bindable.Core.Threading;

namespace Bindable.Linq.Adapters.Incoming
{
    internal class ObservableCollectionToBindableCollectionAdapter<TElement> : BindableCollectionAdapterBase<TElement>
    {
        public ObservableCollectionToBindableCollectionAdapter(IEnumerable sourceCollection, IDispatcher dispatcher)
            : base(sourceCollection, dispatcher)
        {
            Guard.NotNull(sourceCollection, "sourceCollection");
            var observable = sourceCollection as INotifyCollectionChanged;
            if (observable != null)
            {
                observable.CollectionChanged += Weak.Event<NotifyCollectionChangedEventArgs>( (sender, e) => Dispatcher.Dispatch(() => OnCollectionChanged(e))).KeepAlive(InstanceLifetime).HandlerProxy.Handler;
            }
        }
    }
}
