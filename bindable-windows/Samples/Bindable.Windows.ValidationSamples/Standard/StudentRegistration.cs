using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Bindable.Windows.ValidationSamples.Standard;

namespace Bindable.Windows.ValidationSamples.Standard
{
    public class StudentRegistration : IDataErrorInfo, INotifyPropertyChanged
    {
        private readonly ObservableCollection<RegistrationCourseGroup> _courseGroups;
        private readonly ObservableCollection<BestFriend> _bestFriends;
        private string _firstName;
        private string _lastName;
        private string _username;
        private string _password;
        private string _repeatPassword;
        private string _emailAddress;
        private bool _emailNotifications;
        private string _notes;
        
        public StudentRegistration(params RegistrationCourseGroup[] groups)
        {
            _courseGroups = new ObservableCollection<RegistrationCourseGroup>(groups);
            _bestFriends = new ObservableCollection<BestFriend>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FirstName"));
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LastName"));
            }
        }

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Username"));
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Password"));
            }
        }

        public string RepeatPassword
        {
            get { return _repeatPassword; }
            set
            {
                _repeatPassword = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RepeatPassword"));
            }
        }

        public string EmailAddress
        {
            get { return _emailAddress; }
            set
            {
                _emailAddress = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmailAddress"));
            }
        }

        public bool EmailNotifications
        {
            get { return _emailNotifications; }
            set
            {
                _emailNotifications = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmailNotifications"));
            }
        }

        public string Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Notes"));
            }
        }

        public ObservableCollection<RegistrationCourseGroup> CourseGroups
        {
            get { return _courseGroups; }
        }

        public ObservableCollection<BestFriend> BestFriends
        {
            get { return _bestFriends; }
        }

        public string Error
        {
            get { return null;}
        }

        public string this[string columnName]
        {
            get
            {
                var result = new StringBuilder();
                if (columnName == "FirstName" && (FirstName == null || FirstName.Trim().Length == 0))
                {
                    result.AppendLine("Please enter the student's first name.");
                }
                if (columnName == "LastName" && (LastName == null || LastName.Trim().Length == 0))
                {
                    result.AppendLine("Please enter the student's last name.");
                }
                if (columnName == "Password" && (Password == null || Password.Trim().Length == 0))
                {
                    result.AppendLine("Please enter a password.");
                }
                if (columnName == "PasswordsMatch" && (RepeatPassword != Password))
                {
                    result.AppendLine("The passwords entered do not match.");
                }
                if (columnName == "EmailAddress" && (EmailAddress == null || EmailAddress.Trim().Length == 0) && EmailNotifications)
                {
                    result.AppendLine("For email notifications please supply an email address.");
                }
                return result.ToString().TrimEnd();
            }
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, e);
        }
    }
}