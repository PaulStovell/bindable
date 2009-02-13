namespace Bindable.Core.EventMonitoring
{
    /// <summary>
    /// Represents an event that has been captured by an event monitor.
    /// </summary>
    /// <typeparam name="TEventArgs">The type of the event args.</typeparam>
    public sealed class CapturedEvent<TEventArgs>
    {
        private readonly object _sender;
        private readonly TEventArgs _arguments;

        /// <summary>
        /// Initializes a new instance of the <see cref="CapturedEvent&lt;TEventArgs&gt;"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="details">The <see cref="TEventArgs"/> instance containing the event data.</param>
        public CapturedEvent(object sender, TEventArgs details)
        {
            _sender = sender;
            _arguments = details;
        }

        /// <summary>
        /// Gets the object that raised the event.
        /// </summary>
        /// <value>The sender.</value>
        public object Sender 
        {
            get { return _sender; }
        }

        /// <summary>
        /// Gets the arguments raised by the event.
        /// </summary>
        /// <value>The arguments.</value>
        public TEventArgs Arguments
        {
            get { return _arguments; }
        }
    }
}