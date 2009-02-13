using System.Collections.Specialized;
using Bindable.Core.EventCatchers;

namespace Bindable.Core.EventCatchers
{
    /// <summary>
    /// Represents an event monitor for monitoring events raised by collections through <see cref="INotifyCollectionChanged"/>.
    /// </summary>
    public sealed class CollectionEventCatcher : EventCatcher<INotifyCollectionChanged, NotifyCollectionChangedEventArgs>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionEventCatcher"/> class.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        public CollectionEventCatcher(INotifyCollectionChanged publisher)
            : base(publisher)
        {
            
        }

        /// <summary>
        /// Subscribes to the specified publisher.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        protected override void Subscribe(INotifyCollectionChanged publisher)
        {
            publisher.CollectionChanged += RecordEvent;
        }

        /// <summary>
        /// Unsubscribes from the specified publisher.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        protected override void Unsubscribe(INotifyCollectionChanged publisher)
        {
            publisher.CollectionChanged -= RecordEvent;
        }
    }
}