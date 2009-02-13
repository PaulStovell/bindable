using System;
using System.Collections.Generic;
using System.ComponentModel;
using Bindable.Windows.Framework;

namespace Bindable.Windows.Controls.ValidationProviders
{
    /// <summary>
    /// Default implementation of an <see cref="ValidationProvider"/> that enables support for IDataErrorInfo.
    /// </summary>
    public class DataErrorInfoValidationProvider : ValidationProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataErrorInfoValidationProvider"/> class.
        /// </summary>
        public DataErrorInfoValidationProvider()
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
            return context.ValidationTarget is IDataErrorInfo;
        }

        /// <summary>
        /// Validates the specified object, returning a collection of validation rules.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override IEnumerable<IValidationFieldResult> Validate(ValidationContext context)
        {
            var results = new List<IValidationFieldResult>();
            var dataErrorInfo = context.ValidationTarget as IDataErrorInfo;
            if (dataErrorInfo != null)
            {
                var fields = "";
                foreach (var field in context.FieldNamesToValidate)
                {
                    fields += field + " ";
                    var message = dataErrorInfo[field];
                    if ((message ?? string.Empty).Trim().Length > 0)
                    {
                        results.Add(new ValidationFieldResult
                                         {
                                             AssociatedProperties = new[] { field },
                                             Category = ValidationCategory.Error,
                                             Message = message
                                         });
                    }
                }
                Console.WriteLine("Validating fields: {" + fields.Trim() + "} on source " + context.ValidationTarget.GetType().Name);
            }
            return results;
        }
    }
}