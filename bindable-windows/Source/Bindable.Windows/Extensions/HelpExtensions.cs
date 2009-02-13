using System.Windows;

namespace Bindable.Windows.Extensions
{
    /// <summary>
    /// Provides additional properties to provide context sensitive help.
    /// </summary>
    public class HelpExtensions
    {
        public static readonly DependencyProperty HelpTopicIDProperty = DependencyProperty.RegisterAttached("HelpTopicID", typeof(int), typeof(HelpExtensions), new UIPropertyMetadata(0));


        public static int GetHelpTopicID(DependencyObject obj)
        {
            return (int)obj.GetValue(HelpTopicIDProperty);
        }

        public static void SetHelpTopicID(DependencyObject obj, int value)
        {
            obj.SetValue(HelpTopicIDProperty, value);
        }
    }
}
