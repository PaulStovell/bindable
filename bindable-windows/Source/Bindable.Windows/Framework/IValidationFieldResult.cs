using System.ComponentModel;

namespace Bindable.Windows.Framework
{
    /// <summary>
    /// Implemented by classes which represent the result of a validation attempt on an object.
    /// </summary>
    public interface IValidationFieldResult : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the message.
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Gets a value indicating whether or not this result counts as a failure, or is merely a warning or similar.
        /// </summary>
        bool CountsAsFailure { get; }

        /// <summary>
        /// Gets or sets the properties associated with this event.
        /// </summary>
        /// <value>The associated properties.</value>
        string[] AssociatedProperties { get; }
    }
}