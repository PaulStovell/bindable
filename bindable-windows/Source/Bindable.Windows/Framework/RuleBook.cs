using System;
using System.Collections.Generic;
using System.Threading;

namespace Bindable.Windows.Framework
{
    public class RuleBook
    {
        private readonly List<IValidationRule> _rules = new List<IValidationRule>();

        public void AddRule(IValidationRule rule)
        {
            _rules.Add(rule);
        }

        public void AddRule(string propertyName, string errorMessage, Func<object, bool> invalidWhen)
        {
            _rules.Add(new LambdaValidationRule() { ErrorMessage =  errorMessage, Properties = new[] { propertyName}, ValidateCallback = () => !invalidWhen(null) });
        }

        public void AddRule(string propertyName, string errorMessage, Func<object, bool> invalidWhen, ValidationCategory category)
        {
            _rules.Add(new LambdaValidationRule() { ErrorMessage = errorMessage, Properties = new[] { propertyName }, ValidateCallback = () => !invalidWhen(null), Category = category });
        }

        public void AddRule(string[] properties, string errorMessage, Func<object, bool> invalidWhen)
        {
            _rules.Add(new LambdaValidationRule() { ErrorMessage = errorMessage, Properties = properties, ValidateCallback = () => !invalidWhen(null) });
        }

        public IEnumerable<IValidationFieldResult> Validate()
        {
            var results = new List<IValidationFieldResult>();
            foreach (var rule in _rules)
            {
                var result = rule.Validate();
                if (result != null) results.Add(result);
            }
            return results;
        }
    }
}
