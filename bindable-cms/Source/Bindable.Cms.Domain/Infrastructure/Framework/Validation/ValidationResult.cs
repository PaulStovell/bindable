using System.Collections.Generic;

namespace Bindable.Cms.Domain.Framework.Validation
{
    public class ValidationResult
    {
        private readonly Dictionary<string, string> _results = new Dictionary<string, string>();

        public ValidationResult Required(string validationRuleName, string requiredTextValue, string errorMessage)
        {
            if (requiredTextValue == null || requiredTextValue.Trim().Length == 0)
            {
                _results.Add(validationRuleName, errorMessage);
            }
            return this;
        }

        public string ForRule(string ruleName, object childItem)
        {
            return _results.ContainsKey(ruleName) ? _results[ruleName] : "";
        }

        public string ForRule(string ruleName)
        {
            return _results.ContainsKey(ruleName) ? _results[ruleName] : "";
        }

        public bool Valid
        {
            get { return _results.Count == 0; }
        }

        public void FlagRule(string ruleName, string message)
        {
            _results.Add(ruleName, message);
        }
    }
}