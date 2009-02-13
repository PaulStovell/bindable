using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System;

namespace Bindable.Windows.Controls.ValidationStates
{
    /// <summary>
    /// Serves as an abstract base class for all Validation State Managers.
    /// </summary>
    public abstract class ValidationStateManager
    {
        private Dictionary<FrameworkElement, string[]> _validationRequirements;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationStateManager"/> class.
        /// </summary>
        protected ValidationStateManager()
        {
            _validationRequirements = new Dictionary<FrameworkElement, string[]>();
        }

        /// <summary>
        /// Occurs when this ValidationStateManager wishes to trigger validation.
        /// </summary>
        public event ValidateRequestEventHandler ValidateRequested;

        /// <summary>
        /// Occurs when this ValidationStateManager needs the Validation Scope to invalidate itself and any parents.
        /// </summary>
        public event EventHandler Invalidated;

        /// <summary>
        /// Gets or sets a whether the last validation result is still relevant (e.g., the results should not have changed) or whether it has been invalidated.
        /// </summary>
        protected bool LastValidationResultIsStillRelevant { get; private set; }

        /// <summary>
        /// Gets a mapping of the elements to validate and the fields each element is interested in.
        /// </summary>
        protected IDictionary<FrameworkElement, string[]> ValidationRequirements
        {
            get { return _validationRequirements; }
        }

        /// <summary>
        /// Resets all information captured by this validaton state, such as whether explicit validation has taken place yet.
        /// </summary>
        public virtual void Reset()
        {
            LastValidationResultIsStillRelevant = false;
        }

        /// <summary>
        /// When switching between validation states, allows the fields and controls registered with the VSM to be copied to the new state.
        /// </summary>
        public virtual void CopyValidationRequirementsFrom(ValidationStateManager other)
        {
            _validationRequirements = other._validationRequirements;
        }

        /// <summary>
        /// Adds a validation requirement to validate a specific control and a set of fields it is interested in.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="fieldNames">The field names that the control is interested in.</param>
        public virtual void AddValidationRequirements(FrameworkElement control, string[] fieldNames)
        {
            _validationRequirements.Add(control, fieldNames);
            control.Loaded += HandleChildControlLoaded;
            control.GotFocus += HandleChildControlGotFocus;
            control.LostFocus += HandleChildControlLostFocus;
            control.Unloaded += HandleChildControlUnloaded;
            Invalidate();
        }

        /// <summary>
        /// Removes the validation requirements.
        /// </summary>
        /// <param name="control">The control.</param>
        public virtual void RemoveValidationRequirements(FrameworkElement control)
        {
            if (_validationRequirements.ContainsKey(control))
            {
                _validationRequirements.Remove(control);
            }
        }

        /// <summary>
        /// For a given field, returns the list of elements that are interested in the field. This is used to wire up errors from the validation provider
        /// with errors on screen.
        /// </summary>
        /// <param name="fieldName">Name of the field/property.</param>
        /// <returns></returns>
        public virtual FrameworkElement[] GetElementsAssociatedWithField(string fieldName)
        {
            return _validationRequirements.Where(kvp => kvp.Value.Contains(fieldName)).Select(kvp => kvp.Key).ToArray();
        }

        /// <summary>
        /// Records the fact that validation has taken place.
        /// </summary>
        /// <param name="reason">The reason.</param>
        public virtual void RecordValidation(ValidationReason reason)
        {
            LastValidationResultIsStillRelevant = true;
        }

        /// <summary>
        /// Returns a value indicating whether or not validation should be performed at this point.
        /// </summary>
        /// <param name="reason">The reason validation has been requested.</param>
        /// <returns>True if validation should proceed, otherwise false.</returns>
        public abstract bool ShouldValidate(ValidationReason reason);

        /// <summary>
        /// Gets a list of fields that should be validated during the validation process.
        /// </summary>
        /// <param name="reason">The reason validation has been requested.</param>
        /// <returns>An array of field names.</returns>
        public abstract string[] GetFieldsToValidate(ValidationReason reason);

        /// <summary>
        /// Marks the last validation result as being irellevant. Next time validation is requested it will proceed.
        /// </summary>
        public virtual void Invalidate()
        {
            if (LastValidationResultIsStillRelevant)
            {
                LastValidationResultIsStillRelevant = false;
                OnInvalidated(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Handles a child control being loaded.
        /// </summary>
        protected virtual void HandleChildControlLoaded(object sender, RoutedEventArgs e)
        {
            Invalidate();
        }

        /// <summary>
        /// Handles a child control getting focus.
        /// </summary>
        protected virtual void HandleChildControlGotFocus(object sender, RoutedEventArgs e)
        {
#if SILVERLIGHT
            var control = sender as Control;
            if (control != null)
            {
                control.UpdateLayout();
                //if (control == FocusManager.GetFocusedElement())
                {
                    control.Focus();
                    control.Focus();
                }
            }
#endif
        }

        /// <summary>
        /// Handles a child control losing focus.
        /// </summary>
        protected virtual void HandleChildControlLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement)
            {
                ((FrameworkElement)sender).UpdateLayout();
            }
        }

        /// <summary>
        /// Handles a child control being unloaded.
        /// </summary>
        protected virtual void HandleChildControlUnloaded(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Raises the <see cref="ValidateRequested"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Bindable.Windows.Controls.ValidationStates.ValidateRequestEventArgs"/> instance containing the event data.</param>
        protected virtual void OnValidateRequested(ValidateRequestEventArgs e)
        {
            var handler = ValidateRequested;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="Invalidated"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnInvalidated(EventArgs e)
        {
            var handler = Invalidated;
            if (handler != null) handler(this, e);
        }
    }
}