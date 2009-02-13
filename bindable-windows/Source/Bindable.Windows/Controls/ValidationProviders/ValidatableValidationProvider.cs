using System.Collections.Generic;
using System.Linq;
using Bindable.Windows.Framework;

namespace Bindable.Windows.Controls.ValidationProviders
{
    /// <summary>
    /// A validation provider for objects that implement IValidatable.
    /// </summary>
    public class ValidatableValidationProvider : ValidationProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatableValidationProvider"/> class.
        /// </summary>
        public ValidatableValidationProvider()
        {
            
        }

        /// <summary>
        /// Determines whether this instance can validate the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// 	<c>true</c> if this instance can validate the specified context; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanValidate(ValidationContext context)
        {
            return context.ValidationTarget is IValidatable;
        }

        /// <summary>
        /// Validates the specified object, returning a collection of validation rules.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override IEnumerable<IValidationFieldResult> Validate(ValidationContext context)
        {
            var validatable = context.ValidationTarget as IValidatable;
            if (validatable != null)
            {
                return validatable.Validate().Where(r => r.AssociatedProperties.Intersect(context.FieldNamesToValidate).Count() > 0);
            }
            return new IValidationFieldResult[0];
        }
    }
}