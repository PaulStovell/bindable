using System;

namespace Bindable.Windows.Controls.ValidationStates
{
    public delegate void ValidateRequestEventHandler(object sender, ValidateRequestEventArgs e);

    public class ValidateRequestEventArgs : EventArgs
    {
        public ValidateRequestEventArgs(ValidationReason reason)
        {
            ValidationReason = reason;
        }

        public ValidationReason ValidationReason { get; private set; }
    }
}