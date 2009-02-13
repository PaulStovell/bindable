using System.ComponentModel;

namespace Bindable.Windows.Framework
{
    public class ValidationFieldResult : IValidationFieldResult
    {
        public event PropertyChangedEventHandler PropertyChanged;        
        public string MessageID { get; set; }
        public string Message { get; set; }
        public ValidationCategory Category { get; set; }
        public string[] AssociatedProperties { get; set; }
        public bool CountsAsFailure { get { return Category == ValidationCategory.Error; } }
    }
}
