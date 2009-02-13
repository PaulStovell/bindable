using System.Windows;
using System.Windows.Input;

namespace Bindable.Windows.Extensions
{
    public static class FocusExtensions
    {
        public static readonly DependencyProperty IsFocusedProperty = DependencyProperty.RegisterAttached("IsFocused", typeof(bool), typeof(FocusExtensions), new UIPropertyMetadata(false, IsFocusedPropertySet, CC));

        private static object CC(DependencyObject d, object baseValue)
        {
            return baseValue;
        }

        public static bool GetIsFocused(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsFocusedProperty);
        }

        public static void SetIsFocused(DependencyObject obj, bool value)
        {
            obj.SetValue(IsFocusedProperty, value);
        }

        private static void IsFocusedPropertySet(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var targetElement = target as FrameworkElement;
            if (targetElement == null) return;

            var isFocussed = (bool)e.NewValue;
            if (!isFocussed) return;
            
            targetElement.Focus();
        }

        public static void Monitor(FrameworkElement targetElement)
        {
            targetElement.LostKeyboardFocus -= TargetElement_LostKeyboardFocus;
            targetElement.LostKeyboardFocus += TargetElement_LostKeyboardFocus;
            targetElement.GotKeyboardFocus -= TargetElement_GetKeyboardFocus;
            targetElement.GotKeyboardFocus += TargetElement_GetKeyboardFocus;
        }

        private static void TargetElement_GetKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            SetIsFocused((DependencyObject)sender, true);
        }

        private static void TargetElement_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            SetIsFocused((DependencyObject)sender, false);
        }
    }
}
