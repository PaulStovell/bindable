using System.Windows.Controls;
using System.Windows;
using System.ComponentModel;
using System.Windows.Media;

namespace Bindable.Windows.Controls
{
    public class FormHeader : ItemsControl
    {
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(object), typeof(FormHeader), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(FormHeader), new PropertyMetadata(null));

        public FormHeader()
        {
#if SILVERLIGHT
            DefaultStyleKey = typeof(FormHeader);
#endif
        }

        [Category("Appearance")]
        public object Title
        {
            get { return GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        [Category("Appearance")]
        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
    }
}