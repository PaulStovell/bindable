using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Bindable.Windows.Controls.ValidationDisplayStrategies;
using Bindable.Windows.Controls.ValidationProviders;
using Bindable.Windows.Controls.ValidationStates;
using Bindable.Windows.Framework;

namespace Bindable.Windows.Controls
{
    /// <summary>
    /// A validation scope that can decorate a group of elements and provides an alternative approach to validation.
    /// </summary>
    public class ValidationScope : ContentControl
    {
#if SILVERLIGHT
        public static readonly DependencyProperty OwnerValidationScopeProperty = DependencyProperty.RegisterAttached("OwnerValidationScope", typeof(ValidationScope), typeof(ValidationScope), new PropertyMetadata(null));
#else
        public static readonly DependencyProperty OwnerValidationScopeProperty = DependencyProperty.RegisterAttached("OwnerValidationScope", typeof(ValidationScope), typeof(ValidationScope), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, OwnerValidationScopePropertySet));
#endif  
        public static readonly DependencyProperty ValidationTriggerProperty = DependencyProperty.Register("ValidationTrigger", typeof(ValidationTrigger), typeof(ValidationScope), new PropertyMetadata(ValidationTrigger.Automatic, ValidationTriggerPropertySet));
        public static readonly DependencyProperty ValidationSourceProperty = DependencyProperty.Register("ValidationSource", typeof(object), typeof(ValidationScope), new PropertyMetadata(null, ValidationSourcePropertySet));
        public static readonly DependencyProperty ValidationProviderProperty = DependencyProperty.Register("ValidationProvider", typeof(ValidationProvider), typeof(ValidationScope), new PropertyMetadata(null, ValidationProviderPropertySet));
        public static readonly DependencyProperty ValidateFieldProperty = DependencyProperty.RegisterAttached("ValidateField", typeof(string), typeof(ValidationScope), new PropertyMetadata(string.Empty, ValidateFieldPropertySetOnChildElement));
        public static readonly DependencyProperty IsValidProperty = DependencyProperty.RegisterAttached("IsValid", typeof(bool), typeof(ValidationScope), new PropertyMetadata(true));
        public static readonly DependencyProperty ValidationDisplayStrategyProperty = DependencyProperty.Register("ValidationDisplayStrategy", typeof(IValidationDisplayStrategy), typeof(ValidationScope), new PropertyMetadata(null));
        public static readonly DependencyProperty ValidationTemplateProperty = DependencyProperty.RegisterAttached("ValidationTemplate", typeof(ControlTemplate), typeof(ValidationScope), new PropertyMetadata(null));
        public static readonly DependencyProperty ValidationResultProperty = DependencyProperty.Register("ValidationResult", typeof(ValidationResult), typeof(ValidationScope), new UIPropertyMetadata(null));
        public static readonly DependencyProperty ElementValidationResultProperty = DependencyProperty.RegisterAttached("ElementValidationResult", typeof(ValidationElementResult), typeof(ValidationScope), new UIPropertyMetadata(null));
        private readonly List<WeakReference> _childValidationScopes = new List<WeakReference>();
        private ValidationStateManager _stateManager;

        /// <summary>
        /// Initializes the <see cref="ValidationScope"/> class.
        /// </summary>
        static ValidationScope()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ValidationScope), new FrameworkPropertyMetadata(typeof(ValidationScope)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationScope"/> class.
        /// </summary>
        public ValidationScope()
        {
            //ValidationProvider = new AutomaticValidationProvider();
            StateManager = new AutomaticValidationStateManager();
#if SILVERLIGHT
            DefaultStyleKey = typeof(ValidationScope);
#endif
            Loaded += ValidationScope_Loaded;
            Unloaded += ValidationScope_Unloaded;
        }

        #region Dependency Properties

        public ValidationTrigger ValidationTrigger
        {
            get { return (ValidationTrigger)GetValue(ValidationTriggerProperty); }
            set { SetValue(ValidationTriggerProperty, value); }
        }

        public object ValidationSource
        {
            get { return GetValue(ValidationSourceProperty); }
            set { SetValue(ValidationSourceProperty, value); }
        }

        public IValidationDisplayStrategy ValidationDisplayStrategy
        {
            get { return (IValidationDisplayStrategy)GetValue(ValidationDisplayStrategyProperty); }
            set { SetValue(ValidationDisplayStrategyProperty, value); }
        }

        public ValidationProvider ValidationProvider
        {
            get { return (ValidationProvider)GetValue(ValidationProviderProperty); }
            set { SetValue(ValidationProviderProperty, value); }
        }

        public static ValidationScope GetOwnerValidationScope(DependencyObject child)
        {
            // In Silverlight property value inheritance does not exist, so we use this hook to read the value. Under WPF this will
            // just return from the first iteration as the value should have been inherited or set explicitly.
            var currentElement = child;
            while (currentElement != null)
            {
                var scope = (ValidationScope) child.GetValue(OwnerValidationScopeProperty);
                if (scope != null) 
                    return scope;
                currentElement = VisualTreeHelper.GetParent(currentElement);
            }
            return null;
        }

        public static void SetOwnerValidationScope(DependencyObject obj, ValidationScope value)
        {
            obj.SetValue(OwnerValidationScopeProperty, value);
        }

        public static string GetValidateField(DependencyObject obj)
        {
            return (string)obj.GetValue(ValidateFieldProperty);
        }

        public static void SetValidateField(DependencyObject obj, string value)
        {
            obj.SetValue(ValidateFieldProperty, value);
        }

        public static bool GetIsValid(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsValidProperty);
        }

        public static void SetIsValid(DependencyObject obj, bool value)
        {
            obj.SetValue(IsValidProperty, value);
        }

        public ValidationResult ValidationResult
        {
            get { return (ValidationResult)GetValue(ValidationResultProperty); }
            set { SetValue(ValidationResultProperty, value); }
        }

        public static ControlTemplate GetValidationTemplate(DependencyObject obj)
        {
            return (ControlTemplate)obj.GetValue(ValidationTemplateProperty);
        }

        public static void SetValidationTemplate(DependencyObject obj, ControlTemplate value)
        {
            obj.SetValue(ValidationTemplateProperty, value);
        }

        public static ValidationElementResult GetElementValidationResult(DependencyObject obj)
        {
            return (ValidationElementResult)obj.GetValue(ElementValidationResultProperty);
        }

        public static void SetElementValidationResult(DependencyObject obj, ValidationElementResult value)
        {
            obj.SetValue(ElementValidationResultProperty, value);
        }

        /// <summary>
        /// Gets or sets the state manager.
        /// </summary>
        protected ValidationStateManager StateManager
        {
            get { return _stateManager; }
            set
            {
                if (_stateManager != null) _stateManager.ValidateRequested -= StateManager_ValidateRequested;
                if (_stateManager != null) _stateManager.Invalidated -= StateManager_Invalidated;
                if (_stateManager != null) value.CopyValidationRequirementsFrom(_stateManager);
                _stateManager = value;
                if (_stateManager != null) _stateManager.ValidateRequested += StateManager_ValidateRequested;
                if (_stateManager != null) _stateManager.Invalidated += StateManager_Invalidated;
            }
        }

        #endregion

        #region Validation Process

        /// <summary>
        /// Validates the data context and shows any error messages for all fields (even those which have not had focus yet), 
        /// returning a value indicating if there any validation errors. 
        /// </summary>
        public ValidationResult Validate()
        {
            return Validate(ValidationReason.ExplicitValidationRequested);
        }

        /// <summary>
        /// Validates the data context and shows any error messages for fields which have had focus 
        /// (or for all fields if ValidatesOnLoad is true), returning a value indicating if there were
        /// any validation errors. 
        /// </summary>
        private ValidationResult Validate(ValidationReason reason)
        {   
            // Validate this scope, but defer any decision making to the Validation State Manager.
            if (ValidationProvider != null)
            {
                if (StateManager.ShouldValidate(reason))
                {
                    // Validate this validation scope
                    var fieldsToValidate = StateManager.GetFieldsToValidate(reason);    
                    var validationContext = new ValidationContext(ValidationSource, fieldsToValidate, reason, Dispatcher);
                    var validationResults = new IValidationFieldResult[0] as IEnumerable<IValidationFieldResult>;
                    if (fieldsToValidate.Count() > 0)
                    {
                        // Don't ask the Validation Provider to validate unless we actually have fields
                        // TODO: Perhaps this optimization should be up to the provider instead, or would that be counter-intuitive to provider writers?
                        validationResults = ValidationProvider.Validate(validationContext);
                    }

                    // Validate child validation scopes
                    var childValidationResults = new List<ValidationResult>();
                    foreach (var scope in _childValidationScopes.Select(wr => wr.Target as ValidationScope).Where(vs => vs != null).ToList())
                    {
                        var result = scope.Validate(validationContext.Reason);
                        childValidationResults.Add(result);
                    }

                    ProcessValidationResults(validationResults, childValidationResults, validationContext);
                    SetIsValid(this, ValidationResult.WasSuccessful);
                }
            }

            return ValidationResult;
        }

        /// <summary>
        /// Processes the results of the last validation.
        /// </summary>
        /// <param name="validationResults">The validation results.</param>
        /// <param name="childValidationResults">The child validation results.</param>
        /// <param name="context">The context.</param>
        private void ProcessValidationResults(IEnumerable<IValidationFieldResult> validationResults, IEnumerable<ValidationResult> childValidationResults, ValidationContext context)
        {
            // Map the validation results to the elements they relate to
            var currentResults = new ValidationElementResult[0] as IEnumerable<ValidationElementResult>;
            if (validationResults != null)
            {
                currentResults = validationResults.SelectMany(result 
                    => result.AssociatedProperties
                             .Where(prop 
                                => context.FieldNamesToValidate.Contains(prop))
                             .SelectMany(prop 
                                 => StateManager.GetElementsAssociatedWithField(prop).AsEnumerable()
                             .Select(element 
                                 => new ValidationElementResult() { Element = element, Result = result })));
            }

            // Show and hide the latest round of validation failures
            StateManager.RecordValidation(context.Reason);
            if (ValidationResult != null)
            {
                foreach (var discardedFailure in ValidationResult.ElementDirectResults.Except(currentResults, ValidationElementResult.EqualityComparer.Instance).ToList())
                {
                    RemoveValidationResultForElement(discardedFailure);
                }
            }
            foreach (var failure in currentResults)
            {
                ShowValidationResultForElement(failure);
            }

            ValidationResult = new ValidationResult(currentResults, validationResults, childValidationResults);
        }

        private void ShowValidationResultForElement(ValidationElementResult failure)
        {
            SetIsValid(failure.Element, false);
            SetElementValidationResult(failure.Element, failure);
            if (ValidationDisplayStrategy != null) ValidationDisplayStrategy.ShowValidationFailure(failure);
        }

        private void RemoveValidationResultForElement(ValidationElementResult failure)
        {
            SetIsValid(failure.Element, true);
            SetElementValidationResult(failure.Element, null);
            if (ValidationDisplayStrategy != null) ValidationDisplayStrategy.RemoveValidationFailure(failure);
        }

        public void InvalidateLastValidationResult()
        {
            StateManager.Invalidate();
            var parent = GetOwnerValidationScope(this);
            if (parent != null) parent.InvalidateLastValidationResult();
            else
            {
                Validate(ValidationReason.AutomaticValidationTriggered);
            }
        }

        #endregion

        #region Useful methods

        /// <summary>
        /// Gets the first validation failure.
        /// </summary>
        public ValidationElementResult GetFirstValidationFailure()
        {
            return ValidationResult.ElementFailures.FirstOrDefault();
        }

        #endregion

        #region Dependency property triggers

        /// <summary>
        /// Occurs when the ValidateOnLoad property is set on this validation scope.
        /// </summary>
        private static void ValidationTriggerPropertySet(DependencyObject scopeElement, DependencyPropertyChangedEventArgs e)
        {
            var newStateManager = null as ValidationStateManager;
            switch ((ValidationTrigger)e.NewValue)
            {
                case ValidationTrigger.Automatic:
                    newStateManager = new AutomaticValidationStateManager();
                    break;
                case ValidationTrigger.AutomaticAfterFocus:
                    newStateManager = new AutomaticAfterFocusValidationStateManager();
                    break;
                case ValidationTrigger.AutomaticAfterManual:
                    newStateManager = new AutomaticAfterManualValidationStateManager();
                    break;
                case ValidationTrigger.Manual:
                    newStateManager = new ManualValidationStateManager();
                    break;
            }
            
            var validationScope = (ValidationScope)scopeElement;
            validationScope.StateManager = newStateManager;
        }

        /// <summary>
        /// Handles the ValidateRequested event of the StateManager.
        /// </summary>
        private void StateManager_ValidateRequested(object sender, ValidateRequestEventArgs e)
        {
            if (IsLoaded) Validate(e.ValidationReason);
        }

        /// <summary>
        /// Handles the Invalidated event of the StateManager.
        /// </summary>
        private void StateManager_Invalidated(object sender, EventArgs e)
        {
            if (IsLoaded) InvalidateLastValidationResult();
        }

        /// <summary>
        /// Occurs when the ValidateOnLoad property is set on this validation scope.
        /// </summary>
        private static void ValidationSourcePropertySet(DependencyObject scopeElement, DependencyPropertyChangedEventArgs e)
        {
            var scope = (ValidationScope)scopeElement;
            if (e.OldValue is INotifyPropertyChanged)
            {
                var propertyChanged = (INotifyPropertyChanged)e.OldValue;
                propertyChanged.PropertyChanged -= scope.Source_PropertyChanged;
            }
            if (e.NewValue is INotifyPropertyChanged)
            {
                var propertyChanged = (INotifyPropertyChanged)e.NewValue;
                propertyChanged.PropertyChanged += scope.Source_PropertyChanged;
            }

            ((ValidationScope)scopeElement).StateManager.Reset();
        }

        /// <summary>
        /// Called when the <see cref="P:System.Windows.Controls.ContentControl.Content"/> property changes.
        /// </summary>
        /// <param name="oldContent">The old value of the <see cref="P:System.Windows.Controls.ContentControl.Content"/> property.</param>
        /// <param name="newContent">The new value of the <see cref="P:System.Windows.Controls.ContentControl.Content"/> property.</param>
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            SetOwnerValidationScope((DependencyObject) newContent, this);
        }

        /// <summary>
        /// Called when the ValidationProvider property is set. We use this to ensure the ValidationProvider is a part of the validation scope's logical tree, 
        /// so that any child elements may have bindings.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void ValidationProviderPropertySet(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var scope = target as ValidationScope;
            if (scope == null) return;

            if (e.OldValue != null) scope.RemoveLogicalChild(e.OldValue);
            if (e.NewValue != null) scope.AddLogicalChild(e.NewValue);
        }

        #endregion

        #region Validation scope lifecycle

        /// <summary>
        /// Handles the Loaded event of the ValidationScope control.
        /// </summary>
        private void ValidationScope_Loaded(object sender, RoutedEventArgs e)
        {
            Validate(ValidationReason.AutomaticValidationTriggered);
        }

        /// <summary>
        /// Handles the Unloaded event of the ValidationScope control.
        /// </summary>
        private void ValidationScope_Unloaded(object sender, RoutedEventArgs e)
        {
            InvalidateLastValidationResult();
        }

        #endregion

        #region Attached dependency property triggers

        /// <summary>
        /// Occurs when the OwnerValidationScope property is set on a child element.
        /// </summary>
        private static void OwnerValidationScopePropertySet(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var childValidationScope = target as ValidationScope;
            if (childValidationScope == null) return;

            var parent = e.NewValue as ValidationScope;
            if (parent != null)
            {
                parent.AddChildValidationScope(childValidationScope);
                var valueSource = DependencyPropertyHelper.GetValueSource(childValidationScope, ValidationTriggerProperty);
                if (valueSource.BaseValueSource == BaseValueSource.Default)
                {
                    childValidationScope.ValidationTrigger = parent.ValidationTrigger;
                }
            }
        }

        /// <summary>
        /// Occurs when the attached ValidateField property is set on a child element.
        /// </summary>
        private static void ValidateFieldPropertySetOnChildElement(DependencyObject childElement, DependencyPropertyChangedEventArgs e)
        {
            var scope = GetOwnerValidationScope(childElement);
            if (scope != null)
            {
                var frameworkElement = childElement as FrameworkElement;
                if (frameworkElement == null) return;

                if (scope.StateManager != null)
                {
                    scope.StateManager.RemoveValidationRequirements(frameworkElement);
                    scope.StateManager.AddValidationRequirements(frameworkElement, (e.NewValue ?? string.Empty).ToString().Split(',', '|'));
                }
            }
        }

        #endregion

        #region Validation Scope Nesting

        /// <summary>
        /// Adds a child validation scope.
        /// </summary>
        /// <param name="child">The child.</param>
        public void AddChildValidationScope(ValidationScope child)
        {
            var alreadyContainsChild = _childValidationScopes.Where(wr => wr.Target == child).Count() > 0;
            if (!alreadyContainsChild && child != this)
            {
                _childValidationScopes.Add(new WeakReference(child, true));
            }
        }

        /// <summary>
        /// Removes a child validation scope.
        /// </summary>
        /// <param name="child">The child.</param>
        public void RemoveChildValidationScope(ValidationScope child)
        {
            var childReference = _childValidationScopes.Where(wr => wr.Target == child).FirstOrDefault();
            if (childReference != null)
            {
                _childValidationScopes.Remove(childReference);
            }
        }

        #endregion

        #region Validation source lifecycle

        /// <summary>
        /// Occurs when the ValidationScope's ValidationSource raises a PropertyChanegd event.
        /// </summary>
        private void Source_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            InvalidateLastValidationResult();
        }

        #endregion
    }
}