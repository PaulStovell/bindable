using System.Collections.Generic;

namespace Bindable.Windows.Framework
{
    public interface IValidatable
    {
        IEnumerable<IValidationFieldResult> Validate();
    }
}