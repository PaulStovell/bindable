using System.Windows;

namespace Bindable.Windows.Extensions
{
    /// <summary>
    /// Extends framework elements by providing an IsHighContrast property holder that can be used from within styles to indicate whether the SystemParameters.IsHighContrast flag is specified.
    /// </summary>
    public static class AccessibilityExtensions
    {
        /// <summary>
        /// Identifies the IsHighContrast property.
        /// </summary>
        public static readonly DependencyProperty IsHighContrastProperty = DependencyProperty.RegisterAttached("IsHighContrast", typeof(bool), typeof(AccessibilityExtensions), new UIPropertyMetadata(false));

        /// <summary>
        /// Gets the is high contrast.
        /// </summary>
        public static bool GetIsHighContrast(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsHighContrastProperty);
        }

        /// <summary>
        /// Sets the is high contrast.
        /// </summary>
        public static void SetIsHighContrast(DependencyObject obj, bool value)
        {
            obj.SetValue(IsHighContrastProperty, value);
        }
    }
}
