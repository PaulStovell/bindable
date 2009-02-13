using System.ComponentModel;
using Bindable.Core.EventCatchers;

namespace Bindable.Core.EventCatchers
{
    /// <summary>
    /// An event monitor for properties via <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    public sealed class PropertyEventCatcher : EventCatcher<INotifyPropertyChanged, PropertyChangedEventArgs>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyEventCatcher"/> class.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        public PropertyEventCatcher(INotifyPropertyChanged publisher)
            : base(publisher)
        {
            
        }

        /// <summary>
        /// Subscribes to the specified publisher.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        protected override void Subscribe(INotifyPropertyChanged publisher)
        {
            publisher.PropertyChanged += RecordEvent;
        }

        /// <summary>
        /// Unsubscribes from the specified publisher.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        protected override void Unsubscribe(INotifyPropertyChanged publisher)
        {
            publisher.PropertyChanged -= RecordEvent;
        }
    }
}