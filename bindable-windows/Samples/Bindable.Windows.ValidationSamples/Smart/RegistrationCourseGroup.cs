using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Bindable.Windows.ValidationSamples.Smart
{
    public class RegistrationCourseGroup : IDataErrorInfo, INotifyPropertyChanged
    {
        private readonly ObservableCollection<RegistrationCourse> _courses;
        private readonly int _maxAllowed;
        
        public RegistrationCourseGroup(string name, int maxAllowed, params RegistrationCourse[] courses)
        {
            GroupName = name;
            _maxAllowed = maxAllowed;
            _courses = new ObservableCollection<RegistrationCourse>(courses);
            foreach (var course in courses)
            {
                course.PropertyChanged += new PropertyChangedEventHandler(course_PropertyChanged);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string GroupName { get; set; }
        
        public bool TooManyCoursesSelected
        {
            get { return _courses.Where(course => course.Taken).Count() > _maxAllowed; }
        }

        public ObservableCollection<RegistrationCourse> Courses
        {
            get { return _courses; }
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
                if (columnName == "TooManyCoursesSelected" && TooManyCoursesSelected)
                {
                    result.AppendLine(string.Format("You have selected too many courses from this group. Only {0} are allowed.", _maxAllowed));
                }
                return result.ToString().TrimEnd();
            }
        }

        void course_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs("TooManyCoursesSelected"));
        }
    }
}