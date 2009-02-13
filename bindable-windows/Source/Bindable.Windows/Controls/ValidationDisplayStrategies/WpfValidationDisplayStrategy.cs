using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Bindable.Windows.Extensions;

namespace Bindable.Windows.Controls.ValidationDisplayStrategies
{
    /// <summary>
    /// Represents the default WPF validation strategy, which uses the Adorner layer to display errors. This strategy is defined as part of the default style 
    /// for validation scopes in WPF, and is set via the Generic.xaml. If users wish to provide a more complicated visualization of errors, they can 
    /// assign a different IValidationDisplayStrategy as the default for the validation scope.
    /// </summary>
    public sealed class WpfValidationDisplayStrategy : IValidationDisplayStrategy
    {
        private static readonly DependencyProperty ValidationErrorAdornerProperty = DependencyProperty.RegisterAttached("ValidationErrorAdorner", typeof(Adorner), typeof(WpfValidationDisplayStrategy), new UIPropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="WpfValidationDisplayStrategy"/> class.
        /// </summary>
        public WpfValidationDisplayStrategy()
        {
        }

        /// <summary>
        /// Shows a validation failure.
        /// </summary>
        /// <param name="validationFailure">Information about the validation failure, including the element on which to show the validation and the validation result.</param>
        public void ShowValidationFailure(ValidationElementResult validationFailure)
        {
            var errorTemplate = ValidationScope.GetValidationTemplate(validationFailure.Element);
            if (errorTemplate != null)
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer(validationFailure.Element);
                if (adornerLayer != null)
                {
                    validationFailure.Element.Loaded += Element_Loaded;
                    FocusExtensions.Monitor(validationFailure.Element);

                    var existingAdorner = validationFailure.Element.GetValue(ValidationErrorAdornerProperty) as TemplatedAdorner;
                    if (existingAdorner != null)
                    {
                        // Update the message displayed in the existing adorner
                        existingAdorner.DataContext = validationFailure.Result.Message;
                    }
                    else
                    {
                        var adorner = new TemplatedAdorner(validationFailure.Element, validationFailure.Result.Message, errorTemplate);
                        adornerLayer.Add(adorner);
                        validationFailure.Element.SetValue(ValidationErrorAdornerProperty, adorner);
                    }
                }
            }
        }

        void Element_Loaded(object sender, RoutedEventArgs e)
        {
            var adorner = ((DependencyObject) sender).GetValue(ValidationErrorAdornerProperty) as TemplatedAdorner;
            if (adorner != null)
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer(sender as Visual);
                adornerLayer.Remove(adorner);
                adornerLayer.Add(adorner);
                //adorner.InvalidateArrange();
            }
        }

        /// <summary>
        /// Removes the validation failure.
        /// </summary>
        /// <param name="validationFailure">Information about the validation failure, including the element on which to show the validation and the validation result.</param>
        public void RemoveValidationFailure(ValidationElementResult validationFailure)
        {
            var adorner = validationFailure.Element.GetValue(ValidationErrorAdornerProperty) as Adorner;
            if (adorner != null)
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer(validationFailure.Element);
                if (adornerLayer != null)
                {
                    adornerLayer.Remove(adorner);
                    validationFailure.Element.SetValue(ValidationErrorAdornerProperty, null);
                }
            }
        }
    }
}