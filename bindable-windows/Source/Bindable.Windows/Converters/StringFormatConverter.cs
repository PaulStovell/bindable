using System;
using System.Globalization;
using System.Windows.Data;
using System.ComponentModel;

namespace Bindable.Windows.Converters
{
    /// <summary>
    /// An IValueConverter designed for formatting strings. This version is intended to be used in single bindings.
    /// </summary>
    [ValueConversion(typeof(object), typeof(string))]
    public sealed class StringFormatConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets the default parameter.
        /// </summary>
        /// <value>The default parameter.</value>
        public string DefaultFormat { get; set; }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = (value ?? string.Empty).ToString();
            if (parameter != null)
            {
                result = string.Format((string)parameter, result);
            }
            else if (DefaultFormat != null)
            {
                result = string.Format(DefaultFormat, result);
            }
            var converter = TypeDescriptor.GetConverter(targetType);
            return converter == null ? result : converter.ConvertTo(result, targetType);
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}