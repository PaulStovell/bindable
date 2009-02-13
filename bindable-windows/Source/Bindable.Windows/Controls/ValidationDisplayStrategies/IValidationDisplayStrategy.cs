
namespace Bindable.Windows.Controls.ValidationDisplayStrategies
{
    /// <summary>
    /// This interface can be implemented to plug into the ValidationScope in order to show validation in different ways. Since WPF prefers to use Adorners, while Silverlight 
    /// uses template swapping, users may wish to provider their own validation visualization system.
    /// </summary>
    public interface IValidationDisplayStrategy
    {
        /// <summary>
        /// Shows a validation failure.
        /// </summary>
        /// <param name="validationFailure">Information about the validation failure, including the element on which to show the validation and the validation result.</param>
        void ShowValidationFailure(ValidationElementResult validationFailure);

        /// <summary>
        /// Removes the validation failure.
        /// </summary>
        /// <param name="validationFailure">Information about the validation failure, including the element on which to show the validation and the validation result.</param>
        void RemoveValidationFailure(ValidationElementResult validationFailure);
    }
}