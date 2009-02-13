using System.Windows;
using System;

namespace Bindable.Windows.ValidationSamples.Smart
{
    public partial class StudentRegistrationForm : Window
    {
        public StudentRegistrationForm()
        {
            InitializeComponent();

            var rego = new StudentRegistration(
                new RegistrationCourseGroup(".NET", 3, 
                                            new RegistrationCourse("Introduction to C#"),
                                            new RegistrationCourse("Introduction to VB"),
                                            new RegistrationCourse("Inversion of Control"),
                                            new RegistrationCourse("Windows Presentation Foundation")),
                new RegistrationCourseGroup("Ruby", 3,
                                            new RegistrationCourse("Introduction to Ruby"),
                                            new RegistrationCourse("Introduction to Rails"),
                                            new RegistrationCourse("ActiveRecord"),
                                            new RegistrationCourse("Rake"))
                );
            for (int i = 0; i < 4; i++)
            {
                var bf = new BestFriend("Friend " + i, i);
                if (i == 43)
                {
                    bf.Name = "";
                }
                rego.BestFriends.Add(bf);
            }
            DataContext = rego;
        }

        private void SaveButton_Clicked(object sender, RoutedEventArgs e)
        {
            if (_validationScope.Validate().WasSuccessful)
            {
                MessageBox.Show("Saved!");
            }
            else
            {
                _validationScope.GetFirstValidationFailure().Element.Focus();
            }
        }

        private void AddFriendButton_Clicked(object sender, RoutedEventArgs e)
        {
            var rego = (StudentRegistration) DataContext;
            rego.BestFriends.Add(new BestFriend("", 0));
        }

        private void DeleteFriendButton_Clicked(object sender, RoutedEventArgs e)
        {
            var item = ((FrameworkElement) sender).DataContext as BestFriend;
            if (item != null)
            {
                var rego = (StudentRegistration)DataContext;
                item.Discard();
                rego.BestFriends.Remove(item);
            }
        }

        private void MoreInfoClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("More info");
        }
    }
}