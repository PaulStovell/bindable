using System.Windows;
using System.Windows.Controls;

namespace Bindable.Windows.Controls
{
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(FormField))]
    public class FormBody : ItemsControl
    {
        public FormBody()
        {
#if SILVERLIGHT
            DefaultStyleKey = typeof(FormBody);
#endif
        }
    }
}