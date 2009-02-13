using System.Windows.Threading;

namespace Bindable.Windows.Controls.ValidationProviders
{
    public class ValidationContext
    {
        public ValidationContext(object validationTarget, string[] fieldNamesToValidate, ValidationReason reason, Dispatcher dispatcher)
        {
            ValidationTarget = validationTarget;
            FieldNamesToValidate = fieldNamesToValidate;
            Reason = reason;
            Dispatcher = dispatcher;
        }

        public object ValidationTarget { get; private set; }

        public string[] FieldNamesToValidate { get; private set; }

        public ValidationReason Reason { get; private set; }

        public Dispatcher Dispatcher { get; private set; }

        public override string ToString()
        {
            return string.Format("Validation context: {0}", Reason);
        }
    }
}