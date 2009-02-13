using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Bindable.Windows.Controls.ValidationStates;

namespace Bindable.Windows.Controls.ValidationStates
{
    /// <summary>
    /// A validation state manager that triggers validation only when a control has had focus.
    /// </summary>
    /// <remarks>
    /// Only fields that have had focus will be validated in most situations. However, if explicit validation is 
    /// requested (e.g., user hits 'Save'), then all fields should be validated.
    /// </remarks>
    public class AutomaticAfterFocusValidationStateManager : ValidationStateManager
    {
        private readonly List<FrameworkElement> _fieldsThatHaveHadFocus = new List<FrameworkElement>();

        /// <summary>
        /// Handles a child control losing focus.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void HandleChildControlLostFocus(object sender, RoutedEventArgs e)
        {
            base.HandleChildControlLostFocus(sender, e);
            _fieldsThatHaveHadFocus.Add(sender as FrameworkElement);
            Invalidate();
            OnValidateRequested(new ValidateRequestEventArgs(ValidationReason.AutomaticValidationTriggered));
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
            // Always validate on explicit validation
            return reason == ValidationReason.ExplicitValidationRequested || 
                (!LastValidationResultIsStillRelevant);
        }

        /// <summary>
        /// Gets a list of fields that should be validated during the validation process.
        /// </summary>
        /// <param name="reason">The reason validation has been requested.</param>
        /// <returns>An array of field names.</returns>
        public override string[] GetFieldsToValidate(ValidationReason reason)
        {
            // Validate all fields in explicit validation; otherwise, only fields that have had focus
            return reason == ValidationReason.ExplicitValidationRequested 
                ? ValidationRequirements.Values.SelectMany(fields => fields.AsEnumerable()).Distinct().ToArray() 
                : ValidationRequirements.Where(kvp => _fieldsThatHaveHadFocus.Contains(kvp.Key)).SelectMany(kvp => kvp.Value).Distinct().ToArray();
        }

        /// <summary>
        /// Records the fact that validation has taken place.
        /// </summary>
        /// <param name="reason">The reason.</param>
        public override void RecordValidation(ValidationReason reason)
        {
            base.RecordValidation(reason);
            
            // After validation has taken place explicitly, treat every field as if it has had focus, as they should all 
            // be validated from this point on.
            if (reason == ValidationReason.ExplicitValidationRequested)
            {
                _fieldsThatHaveHadFocus.AddRange(ValidationRequirements.Keys);
            }
        }

        /// <summary>
        /// Resets all information captured by this validaton state, such as whether explicit validation has taken place yet.
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            _fieldsThatHaveHadFocus.Clear();
        }
    }
}