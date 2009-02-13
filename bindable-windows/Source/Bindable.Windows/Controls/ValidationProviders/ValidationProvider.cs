using System.Collections.Generic;
using System.Windows;
using Bindable.Windows.Framework;

namespace Bindable.Windows.Controls.ValidationProviders
{
    public abstract class ValidationProvider : FrameworkContentElement
    {
        public abstract bool CanValidate(ValidationContext context);
        public abstract IEnumerable<IValidationFieldResult> Validate(ValidationContext context);
    }
}
