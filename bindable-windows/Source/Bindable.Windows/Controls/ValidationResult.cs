using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Bindable.Linq;
using Bindable.Linq.Interfaces;
using Bindable.Windows.Framework;

namespace Bindable.Windows.Controls
{
    /// <summary>
    /// Represents the result of a call to Validate() on a <see cref="ValidationScope"/>.
    /// </summary>
    public class ValidationResult : INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult"/> class.
        /// </summary>
        /// <param name="elementResults">The element results.</param>
        /// <param name="fieldResults">The field results.</param>
        /// <param name="childResults">The child results.</param>
        public ValidationResult(IEnumerable<ValidationElementResult> elementResults, IEnumerable<IValidationFieldResult> fieldResults, IEnumerable<ValidationResult> childResults)
        {
            ElementDirectResults = elementResults.ToList();
            ElementDirectFailures = elementResults.Where(result => result.Result.CountsAsFailure).ToList();
            ElementResults = childResults.SelectMany(res => res.ElementResults).Union(elementResults.ToList()).ToList();
            ElementFailures = ElementResults.Where(result => result.Result.CountsAsFailure);
            Results = childResults.SelectMany(res => res.Failures).Union(fieldResults.ToList()).ToList();
            Failures = Results.Where(result => result.CountsAsFailure);
            ElementResultsCount = ElementResults.AsBindable().Count();
            ElementFailureCount = ElementFailures.AsBindable().Count();
            ResultsCount = Results.AsBindable().Count();
            FailureCount = Failures.AsBindable().Count();
            IsSuccessful = FailureCount.Project(count => count == 0);
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<ValidationElementResult> ElementDirectResults { get; private set; }
        public IEnumerable<ValidationElementResult> ElementDirectFailures { get; private set; }
        public IEnumerable<IValidationFieldResult> Results { get; private set; }
        public IEnumerable<IValidationFieldResult> Failures { get; private set; }
        public IEnumerable<ValidationElementResult> ElementResults { get; private set; }
        public IEnumerable<ValidationElementResult> ElementFailures { get; private set; }
        public IBindable<int> ResultsCount { get; private set; }
        public IBindable<int> FailureCount { get; private set; }
        public IBindable<int> ElementResultsCount { get; private set; }
        public IBindable<int> ElementFailureCount { get; private set; }
        public IBindable<bool> IsSuccessful { get; private set; }
        public bool WasSuccessful { get { return IsSuccessful.Current; } }
    }
}
