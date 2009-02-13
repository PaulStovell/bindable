using System.Linq;

namespace Bindable.Windows.Controls.ValidationStates
{
    /// <summary>
    /// A validation state manager that only triggers validation when the user explicitly requests it, and from then on, any time a change is detected.
    /// </summary>
    public class AutomaticAfterManualValidationStateManager : ManualValidationStateManager
    {
        private bool _manualValidationPerformed;

        /// <summary>
        /// Records the fact that validation has taken place.
        /// </summary>
        /// <param name="reason">The reason.</param>
        public override void RecordValidation(ValidationReason reason)
        {
            base.RecordValidation(reason);
            if (reason == ValidationReason.ExplicitValidationRequested)
            {
                _manualValidationPerformed = true;
            }
        }

        /// <summary>
        /// Resets all information captured by this validaton state, such as whether explicit validation has taken place yet.
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            _manualValidationPerformed = false;
        }

        /// <summary>
        /// Returns a value indicating whether or not validation should be performed at this point.
        /// </summary>
        /// <param name="reason">The reason validation has been requested.</param>
        /// <returns>
        /// True if validation should proceed, otherwise false.
        /// </returns>
        public override bool ShouldValidate(ValidationReason reason)
        {
            return base.ShouldValidate(reason) || (_manualValidationPerformed && !LastValidationResultIsStillRelevant);
        }

        /// <summary>
        /// Gets a list of fields that should be validated during the validation process.
        /// </summary>
        /// <param name="reason">The reason validation has been requested.</param>
        /// <returns>An array of field names.</returns>
        public override string[] GetFieldsToValidate(ValidationReason reason)
        {
            var results = new string[0];
            if (ShouldValidate(reason))
            {
                results = ValidationRequirements.Values.SelectMany(fields => fields.AsEnumerable()).Distinct().ToArray();
            }
            return results;
        }
    }
}