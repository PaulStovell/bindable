using System.Linq;
using Bindable.Windows.Controls.ValidationStates;

namespace Bindable.Windows.Controls.ValidationStates
{
    /// <summary>
    /// A validation state manager that triggers validation as soon as the controls load and any time a change is detected.
    /// </summary>
    public class AutomaticValidationStateManager : ValidationStateManager
    {
        /// <summary>
        /// Returns a value indicating whether or not validation should be performed at this point.
        /// </summary>
        /// <param name="reason">The reason validation has been requested.</param>
        /// <returns>
        /// True if validation should proceed, otherwise false.
        /// </returns>
        public override bool ShouldValidate(ValidationReason reason)
        {
            return reason == ValidationReason.ExplicitValidationRequested || !LastValidationResultIsStillRelevant;
        }

        /// <summary>
        /// Gets a list of fields that should be validated during the validation process.
        /// </summary>
        /// <param name="reason">The reason validation has been requested.</param>
        /// <returns>An array of field names.</returns>
        public override string[] GetFieldsToValidate(ValidationReason reason)
        {
            return ValidationRequirements.Values.SelectMany(fields => fields.AsEnumerable()).Distinct().ToArray();
        }
    }
}