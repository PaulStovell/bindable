using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bindable.Aspects.Binding;
using Bindable.Aspects.Parameters;
using Bindable.Windows.Framework;

namespace Bindable.Windows.ValidationSamples.Smart
{
    public class StudentRegistration : BindableObject, IValidatable
    {
        private readonly ObservableCollection<RegistrationCourseGroup> _courseGroups;
        private readonly ObservableCollection<BestFriend> _bestFriends;
        private readonly RuleBook _ruleBook = new RuleBook();
        
        public StudentRegistration(params RegistrationCourseGroup[] groups)
        {
            _courseGroups = new ObservableCollection<RegistrationCourseGroup>(groups);
            _bestFriends = new ObservableCollection<BestFriend>();
            _ruleBook.AddRule("FirstName", "Please enter the student's first name.", when => FirstName.IsEmpty(), ValidationCategory.Error);
            _ruleBook.AddRule("LastName", "Please enter the student's last name.", when => LastName.IsEmpty(), ValidationCategory.Error);
            _ruleBook.AddRule("Password", "Please enter a password.", when => Password.IsEmpty(), ValidationCategory.Error);
            _ruleBook.AddRule("Password", "This password is not complex enough and could be guessed. We suggest using a more complex password, with a mix of letters and numbers.", when => !Password.IsEmpty() && Password.Length < 6, ValidationCategory.Warning);
            _ruleBook.AddRule("PasswordsMatch", "The passwords to not match.", when => Password != RepeatPassword, ValidationCategory.Error);
        }

        public string FirstName { [NeverNull]get; [NotifyChange]set; }
        public string LastName { [NeverNull]get; [NotifyChange]set; }
        public string Username { [NeverNull]get; [NotifyChange]set; }
        public string Password { [NeverNull]get; [NotifyChange]set; }
        public string RepeatPassword { [NeverNull]get; [NotifyChange]set; }
        public string EmailAddress { [NeverNull]get; [NotifyChange]set; }
        public bool EmailNotifications { get; [NotifyChange]set; }
        public string Notes { [NeverNull]get; [NotifyChange]set; }

        public string FullName
        {
            [DependsOn("FirstName", "LastName")]
            get { return FirstName + " " + LastName; }
        }

        public ObservableCollection<RegistrationCourseGroup> CourseGroups
        {
            get { return _courseGroups; }
        }

        public ObservableCollection<BestFriend> BestFriends
        {
            get { return _bestFriends; }
        }

        public IEnumerable<IValidationFieldResult> Validate()
        {
            return _ruleBook.Validate();
        }
    }
}