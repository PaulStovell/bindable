using System;

namespace Bindable.Windows.Framework
{
    public class LambdaValidationRule : IValidationRule
    {
        public string[] Properties { get; set; }
        public Func<bool> ValidateCallback { get; set; }
        public string ErrorMessage { get; set; }
        public ValidationCategory Category { get; set; }

        public IValidationFieldResult Validate()
        {
            if (ValidateCallback() == false)
            {
                return new ValidationFieldResult
                           {
                               AssociatedProperties = Properties,
                               Category = Category,
                               Message = ErrorMessage
                           };
            }
            return null;
        }
    }
}
