using System.Windows;
using System.Windows.Controls;

namespace Bindable.Windows.Extensions
{
    public static class ItemsControlExtensions
    {
        public static readonly DependencyProperty CreateTemplatesOnLoadProperty = DependencyProperty.RegisterAttached("CreateTemplatesOnLoad", typeof(bool), typeof(ItemsControlExtensions), new UIPropertyMetadata(false, CreateTemplatesOnLoadPropertySet));

        public static bool GetCreateTemplatesOnLoad(DependencyObject obj)
        {
            return (bool)obj.GetValue(CreateTemplatesOnLoadProperty);
        }

        public static void SetCreateTemplatesOnLoad(DependencyObject obj, bool value)
        {
            obj.SetValue(CreateTemplatesOnLoadProperty, value);
        }

        private static void CreateTemplatesOnLoadPropertySet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var itemsControl = d as FrameworkElement;
            if (itemsControl == null) return;

            itemsControl.Arrange(new Rect());
            itemsControl.InvalidateArrange();
        }
    }
}