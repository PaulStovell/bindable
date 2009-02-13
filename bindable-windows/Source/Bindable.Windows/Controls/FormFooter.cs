using System.Windows;
using System.Windows.Controls;
using Bindable.Windows.Controls;

namespace Bindable.Windows.Controls
{
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(FormButtonBar))]
    public class FormFooter : ItemsControl
    {
        public FormFooter()
        {
#if SILVERLIGHT
            DefaultStyleKey = typeof(FormFooter);
#endif
        }
    }
}