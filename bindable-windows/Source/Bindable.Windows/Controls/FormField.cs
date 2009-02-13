using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Bindable.Windows.Controls
{
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(TextBox))]
    public class FormField : ItemsControl
    {
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(FormField), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(object), typeof(FormField), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty IsRequiredProperty = DependencyProperty.Register("IsRequired", typeof(bool), typeof(FormField), new PropertyMetadata(false));

        public FormField()
        {
#if SILVERLIGHT
            DefaultStyleKey = typeof(FormField);
#endif   
        }

        [Category("Appearance")]
        public object Description
        {
            get { return (object)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        [Category("Appearance")]
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        [Category("Appearance")]
        public bool IsRequired
        {
            get { return (bool)GetValue(IsRequiredProperty); }
            set { SetValue(IsRequiredProperty, value); }
        }
    }
}