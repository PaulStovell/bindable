using Bindable.Windows.Controls;

namespace Bindable.Windows.Controls
{
    /// <summary>
    /// Represents the various modes of behavior for the <see cref="ValidationScope"/> control.
    /// </summary>
    public enum ValidationTrigger
    {
        /// <summary>
        /// Validation will take place as soon as the ValidationSource is set and the controls are loaded into the tree.
        /// </summary>
        Automatic,

        /// <summary>
        /// Validation will take place only on controls which have had focus.
        /// </summary>
        AutomaticAfterFocus,

        /// <summary>
        /// Validation will take place automatically, only once validation has been done manually.
        /// </summary>
        AutomaticAfterManual,

        /// <summary>
        /// Validation will only take place when the ValidateExplicit method is invoked, and from then on will behave as automatic.
        /// </summary>
        Manual
    }
}