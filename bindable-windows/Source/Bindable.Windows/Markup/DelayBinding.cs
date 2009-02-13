using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Bindable.Windows.Markup
{
    [MarkupExtensionReturnType(typeof(object))]
    public class DelayBinding : MarkupExtension
    {
        public DelayBinding()
        {
            Delay = TimeSpan.FromSeconds(0.5);
            CommitOnEnterKey = true;
            CommitOnLostFocus = true;
        }

        public DelayBinding(PropertyPath path)
            : this()
        {
            Path = path;
        }

        public IValueConverter Converter { get; set; }
        public object ConverterParamter { get; set; }
        public string ElementName { get; set; }
        public RelativeSource RelativeSource { get; set; }
        public object Source { get; set; }
        public bool ValidatesOnDataErrors { get; set; }
        public bool ValidatesOnExceptions { get; set; }
        public TimeSpan Delay { get; set; }
        public bool CommitOnLostFocus { get; set; }
        public bool CommitOnEnterKey { get; set; }
        [ConstructorArgument("path")]
        public PropertyPath Path { get; set; }
        [TypeConverter(typeof(CultureInfoIetfLanguageTagConverter))]
        public CultureInfo ConverterCulture { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var valueProvider = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (valueProvider != null)
            {
                var bindingTarget = valueProvider.TargetObject as DependencyObject;
                var bindingProperty = valueProvider.TargetProperty as DependencyProperty;
                if (bindingProperty == null || bindingTarget == null)
                {
                    throw new NotSupportedException(string.Format(
                                                        "The property '{0}' on target '{1}' is not valid for a DelayBinding. The DelayBinding target must be a DependencyObject, "
                                                        + "and the target property must be a DependencyProperty.",
                                                        valueProvider.TargetProperty,
                                                        valueProvider.TargetObject));
                }

                var binding = new Binding();
                binding.Path = Path;
                binding.Converter = Converter;
                binding.ConverterCulture = ConverterCulture;
                binding.ConverterParameter = ConverterParamter;
                if (ElementName != null) binding.ElementName = ElementName;
                if (RelativeSource != null) binding.RelativeSource = RelativeSource;
                if (Source != null) binding.Source = Source;
                binding.ValidatesOnDataErrors = ValidatesOnDataErrors;
                binding.ValidatesOnExceptions = ValidatesOnExceptions;

                DelayBindingInstance.SetBinding(bindingTarget, bindingProperty, Delay, binding, CommitOnEnterKey, CommitOnLostFocus);
                return bindingTarget.GetValue(bindingProperty);
            }
            return null;
        }
    }
}