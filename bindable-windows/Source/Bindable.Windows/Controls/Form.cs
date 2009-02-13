using System.Windows;
using System.Windows.Controls;

namespace Bindable.Windows.Controls
{
    public class Form : ItemsControl
    {
        static Form()
        {
#if !SILVERLIGHT
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Form), new FrameworkPropertyMetadata(typeof(Form)));
#endif
        }

        public Form()
        {
#if SILVERLIGHT
            DefaultStyleKey = typeof(Form);
#endif
        }
    }
}