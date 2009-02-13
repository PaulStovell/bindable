using System.Windows;
using System.Windows.Controls.Primitives;

namespace Bindable.Windows.Extensions
{
    public static class ToggleButtonExtensions
    {
        public static readonly DependencyProperty UncheckOnLostFocusProperty = DependencyProperty.RegisterAttached("UncheckOnLostFocus", typeof(bool), typeof(ToggleButtonExtensions), new UIPropertyMetadata(false, UncheckOnLostFocusPropertySet));

        public static bool GetUncheckOnLostFocus(DependencyObject obj)
        {
            return (bool)obj.GetValue(UncheckOnLostFocusProperty);
        }

        public static void SetUncheckOnLostFocus(DependencyObject obj, bool value)
        {
            obj.SetValue(UncheckOnLostFocusProperty, value);
        }

        private static void UncheckOnLostFocusPropertySet(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                ((FrameworkElement) target).LostFocus -= ToggleButton_LostFocus;
                ((FrameworkElement) target).LostFocus += ToggleButton_LostFocus;
            }
        }

        private static void ToggleButton_LostFocus(object sender, RoutedEventArgs e)
        {
            ((ToggleButton) sender).IsChecked = false;
        }
    }
}
