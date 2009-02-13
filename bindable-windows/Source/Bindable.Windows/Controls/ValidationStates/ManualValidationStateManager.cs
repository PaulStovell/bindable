using System.Linq;
using Bindable.Windows.Controls.ValidationStates;

namespace Bindable.Windows.Controls.ValidationStates
{
    /// <summary>
    /// A validation state manager that only triggers validation when the user explicitly requests it, ignoring any of the usual signs 
    /// for validation.
    /// </summary>
    public class ManualValidationStateManager : ValidationStateManager
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
            return reason == ValidationReason.ExplicitValidationRequested;
        }

        /// <summary>
        /// Gets a list of fields that should be validated during the validation process.
        /// </summary>
        /// <param name="reason">The reason validation has been requested.</param>
        /// <returns>An array of field names.</returns>
        public override string[] GetFieldsToValidate(ValidationReason reason)
        {
            var results = new string[0];
            if (reason == ValidationReason.ExplicitValidationRequested)
            {
                results = ValidationRequirements.Values.SelectMany(fields => fields.AsEnumerable()).Distinct().ToArray();
            }
            return results;
        }
    }
}