using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.ComponentModel;

namespace Bindable.Windows.Converters
{
    /// <summary>
    /// An IValueConverter designed for formatting strings. This version is intended to be used in single bindings.
    /// </summary>
    public sealed class StringFormatMultiConverter : IMultiValueConverter
    {
        /// <summary>
        /// Gets or sets the default format.
        /// </summary>
        /// <value>The default format.</value>
        public string DefaultFormat { get; set; }

        /// <summary>
        /// Converts source values to a value for the binding target. The data binding engine calls this method when it propagates the values from source bindings to the binding target.
        /// </summary>
        /// <param name="values">The array of values that the source bindings in the <see cref="T:System.Windows.Data.MultiBinding"/> produces. The value <see cref="F:System.Windows.DependencyProperty.UnsetValue"/> indicates that the source binding has no value to provide for conversion.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value.If the method returns null, the valid null value is used.A return value of <see cref="T:System.Windows.DependencyProperty"/>.<see cref="F:System.Windows.DependencyProperty.UnsetValue"/> indicates that the converter did not produce a value, and that the binding will use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue"/> if it is available, or else will use the default value.A return value of <see cref="T:System.Windows.Data.Binding"/>.<see cref="F:System.Windows.Data.Binding.DoNothing"/> indicates that the binding does not transfer the value or use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue"/> or the default value.
        /// </returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 1 && values[0] == DependencyProperty.UnsetValue)
            {
                var stringValues = new string[values.Length];
                return string.Format(CultureInfo.CurrentCulture, parameter.ToString(), stringValues);
            }

            var s = string.Format(CultureInfo.CurrentCulture, parameter.ToString(), values);
            var tc = TypeDescriptor.GetConverter(targetType);

            return tc.CanConvertFrom(typeof(string)) ? tc.ConvertFrom(s) : s;
        }

        /// <summary>
        /// Converts the back.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="targetTypes">The target types.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public object[] ConvertBack(object values, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}