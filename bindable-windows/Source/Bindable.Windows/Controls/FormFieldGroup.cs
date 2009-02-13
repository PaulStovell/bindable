using System.Windows.Controls;

namespace Bindable.Windows.Controls
{
    public class FormFieldGroup : ItemsControl
    {
        public FormFieldGroup()
        {
#if SILVERLIGHT
            DefaultStyleKey = typeof (FormFieldGroup);
#endif
        }
    }
}
