namespace Bindable.Linq.Tests.MockObjectModel
{
    using System.ComponentModel;

    /// <summary>
    /// Represents a sample object used for testing against.
    /// </summary>
    public class Contact : BindableObject
    {
        private static readonly PropertyChangedEventArgs CompanyPropertyChangedEventArgs = new PropertyChangedEventArgs("Company");
        private static readonly PropertyChangedEventArgs ContactIdPropertyChangedEventArgs = new PropertyChangedEventArgs("ContactId");
        private static readonly PropertyChangedEventArgs NamePropertyChangedEventArgs = new PropertyChangedEventArgs("Name");
        private string _company;
        private string _name;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(NamePropertyChangedEventArgs);
            }
        }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        public string Company
        {
            get { return _company; }
            set
            {
                _company = value;
                OnPropertyChanged(CompanyPropertyChangedEventArgs);
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Name == ((Contact) obj).Name && Company == ((Contact) obj).Company;
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return (Name + ":::" + Company).GetHashCode();
        }
    }
}