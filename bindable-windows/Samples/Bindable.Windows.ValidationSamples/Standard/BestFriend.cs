using System.ComponentModel;
using System.Text;

namespace Bindable.Windows.ValidationSamples.Standard
{
    public class BestFriend : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _name;
        private string _age;

        public BestFriend(string name, int age)
        {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
            }
        }

        public string Age
        {
            get { return _age; }
            set
            {
                _age = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Age"));
            }
        }

        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get
            {
                var result = new StringBuilder();
                if (!_isDiscarded)
                {
                    if (columnName == "Name" && (Name == null || Name.Trim().Length == 0))
                    {
                        result.AppendLine("Please enter the best friend's name.");
                    }
                    var age = 0;
                    if (columnName == "Age" && (Age == null || Age.Trim().Length == 0 || !int.TryParse(Age, out age)))
                    {
                        result.AppendLine("The age appears to be invalid or is not supplied.");
                    }
                }
                return result.ToString().TrimEnd();
            }
        }

        private bool _isDiscarded;
        public void Discard()
        {
            _isDiscarded = true;
            OnPropertyChanged(new PropertyChangedEventArgs("IsDiscarded"));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, e);
        }
    }
}
