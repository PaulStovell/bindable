using System.Collections.Generic;
using Bindable.Windows.Framework;

namespace Bindable.Windows.Controls.ValidationProviders
{
    /// <summary>
    /// This interface is implemented by objects which are adaptors to various validation systems. Objects which might implement this interface are 
    /// validators that make use of IDataErrorInfo, objects which use an internal ruleset, or objects which wrap Enterprise Library validation. 
    /// </summary>
    /// <remarks>
    /// For single-threaded, simple validation, implement the Validate method, perform your validation logic, and populate a class implementing 
    /// IValidationResult. To allow background evaluation of rules, you can return the single-threaded rules, and then fire off your background thread. When 
    /// the results are in, raise the ValidationResultsChanged event to indicate them.
    /// </remarks>
    public interface IValidationProvider
    {


        /// <summary>
        /// Validates the specified object, returning a collection of validation rules.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        IEnumerable<IValidationFieldResult> Validate(ValidationContext context);
    }
}