
namespace Bindable.Windows.Controls
{
    /// <summary>
    /// Indicates the reason for which validation is being requested.
    /// </summary>
    public enum ValidationReason
    {
        /// <summary>
        /// Validation is being requested because of an automatic trigger, such as when a property on the source object changes or when 
        /// a control loses focus.
        /// </summary>
        AutomaticValidationTriggered,

        /// <summary>
        /// Validation is being requested because of an explicit validation request, because the ValidateExplicitly method has been called 
        /// from code behind.
        /// </summary>
        ExplicitValidationRequested
    }
}
