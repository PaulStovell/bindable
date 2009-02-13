using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bindable.Windows.ValidationSamples.Smart
{
    public static class ValidationExtensions
    {
        public static bool IsEmpty(this string value)
        {
            return value == null || value.Trim().Length == 0;
        }
    }
}
