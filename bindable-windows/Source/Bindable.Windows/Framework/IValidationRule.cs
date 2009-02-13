using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bindable.Windows.Framework
{
    public interface IValidationRule
    {
        IValidationFieldResult Validate();
    }
}
