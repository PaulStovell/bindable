using System;
using System.Collections.Generic;
using System.Linq;
using Bindable.Core.EventMonitoring;

namespace Bindable.Core.EventCatchers
{
    /// <summary>
    /// A helper class for testing Bindable LINQ queries.
    /// </summary>
    public abstract class EventCatcher<TPublisher, TEventArgs> : IDisposable, IEventCatcher<TEventArgs>
        where TPublisher : class
    {
        private readonly Queue<CapturedEvent<TEventArgs>> _capturedEvents = new Queue<CapturedEvent<TEventArgs>>();
        private TPublisher _publisher;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventCatcher&lt;TPublisher, TEventArgs&gt;"/> class.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        protected EventCatcher(TPublisher publisher)
        {
            _publisher = publisher;
            Subscribe(publisher);
        }

        /// <summary>
        /// Subscribes to the specified publisher.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        protected abstract void Subscribe(TPublisher publisher);

        /// <summary>
        /// Unsubscribes from the specified publisher.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        protected abstract void Unsubscribe(TPublisher publisher);

        /// <summary>
        /// Dequeues the specified number of events from the queue of events.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public CapturedEvent<TEventArgs>[] DequeueNextEvents(int count)
        {
            var results = new List<CapturedEvent<TEventArgs>>();
            for (var i = 0; i < count; i++)
            {
                if (_capturedEvents.Count == 0)
                {
                    break;
                }
                results.Add(_capturedEvents.Dequeue());
            }
            return results.ToArray();
        }

        /// <summary>
        /// Dequeues the next event from the queue.
        /// </summary>
        /// <returns></returns>
        public CapturedEvent<TEventArgs> DequeueNextEvent()
        {
            return DequeueNextEvents(1).FirstOrDefault();
        }

        /// <summary>
        /// Records the event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="TEventArgs"/> instance containing the event data.</param>
        protected void RecordEvent(object sender, TEventArgs e)
        {
            _capturedEvents.Enqueue(new CapturedEvent<TEventArgs>(sender, e));
        }

        /// <summary>
        /// Clears all events.
        /// </summary>
        public void Clear()
        {
            _capturedEvents.Clear();
        }

        /// <summary>
        /// Gets the count of the events currently in the queue of captured events.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get
            {
                return _capturedEvents.Count;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Unsubscribe from the event and also remove the reference to the publisher, so that we aren't adding to the list of 
            // living references as far as the GC is concerned.
            Clear();
            Unsubscribe(_publisher);
            _publisher = null;
        }
    }
}