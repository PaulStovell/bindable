using System.ComponentModel;

namespace Bindable.Windows.ValidationSamples.Smart
{
    public class RegistrationCourse : INotifyPropertyChanged
    {
        private bool _taken = true;

        public RegistrationCourse(string name)
        {
            CourseName = name;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string CourseName { get; set; }
        
        public bool Taken
        {
            get { return _taken; }
            set
            {
                _taken = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Taken"));
            }
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, e);
        }
    }
}