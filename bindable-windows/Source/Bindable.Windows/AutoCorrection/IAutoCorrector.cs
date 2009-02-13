using System;
using System.Globalization;

namespace Bindable.Windows.AutoCorrection
{
    public interface IAutoCorrector
    {
        Correction Correct(object originalValue, Type targetType, CultureInfo currentCulture);
    }
}