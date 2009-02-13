using System.Windows;
using System.Windows.Controls;

namespace Bindable.Windows.Controls
{
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(Button))]
    public class FormButtonBar : ItemsControl
    {
        public FormButtonBar()
        {
#if SILVERLIGHT
            DefaultStyleKey = typeof(FormButtonBar);
#endif
        }
    }
}