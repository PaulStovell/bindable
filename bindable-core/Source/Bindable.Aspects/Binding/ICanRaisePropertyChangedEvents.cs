namespace Bindable.Aspects.Binding
{
    /// <summary>
    /// This interface is designed to be implemented explicitly (TODO: MSDN link) 
    /// by classes to allow the NotifyChange aspect to trigger property changed events.
    /// </summary>
    public interface ICanRaisePropertyChangedEvents
    {
        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        void RaisePropertyChangedEvent(string propertyName);
    }
}
