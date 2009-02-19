using System;
using Bindable.Core.EventMonitoring;

namespace Bindable.Core.EventCatchers
{
    /// <summary>
    /// Captures events raised from a single source.
    /// </summary>
    /// <typeparam name="TEventArgs">The type of the event args.</typeparam>
    public interface IEventCatcher<TEventArgs> : IDisposable
    {
        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        int Count { get;}

        /// <summary>
        /// Clears this instance.
        /// </summary>
        void Clear();

        /// <summary>
        /// Dequeues the next event.
        /// </summary>
        CapturedEvent<TEventArgs> Dequeue();

        /// <summary>
        /// Dequeues up to the given number of events.
        /// </summary>
        /// <param name="count">The count.</param>
        CapturedEvent<TEventArgs>[] DequeueMany(int count);
    }
}