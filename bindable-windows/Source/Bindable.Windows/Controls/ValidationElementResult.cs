using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Bindable.Windows.Framework;

namespace Bindable.Windows.Controls
{
    /// <summary>
    /// Represents a validation failure.
    /// </summary>
    public sealed class ValidationElementResult : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        public IValidationFieldResult Result { get; set; }

        /// <summary>
        /// Gets or sets the element.
        /// </summary>
        public FrameworkElement Element { get; set; }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return Element.GetHashCode() + " | " + Result.Message;
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        /// <remarks>
        /// This event is not raised as items in this class are assumed to be sealed. However, this item is likely to be a target
        /// for binding, and so this event is provided to avoid memory leak issues with regards to binding and objects that do not
        /// implement INotifyPropertyChanged.
        /// </remarks>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// An equality comparer for validation failures.
        /// </summary>
        public class EqualityComparer : IEqualityComparer<ValidationElementResult>
        {
            private static readonly EqualityComparer _instance = new EqualityComparer();

            /// <summary>
            /// Initializes a new instance of the <see cref="EqualityComparer"/> class.
            /// </summary>
            private EqualityComparer()
            {
                
            }

            /// <summary>
            /// Gets the instance.
            /// </summary>
            /// <value>The instance.</value>
            public static EqualityComparer Instance
            {
                get { return _instance; }
            }

            /// <summary>
            /// Determines whether the specified objects are equal.
            /// </summary>
            /// <param name="x">The first object of type <paramref name="T"/> to compare.</param>
            /// <param name="y">The second object of type <paramref name="T"/> to compare.</param>
            /// <returns>
            /// true if the specified objects are equal; otherwise, false.
            /// </returns>
            public bool Equals(ValidationElementResult x, ValidationElementResult y)
            {
                return x.ToString() == y.ToString();
            }

            /// <summary>
            /// Returns a hash code for the specified object.
            /// </summary>
            /// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param>
            /// <returns>A hash code for the specified object.</returns>
            /// <exception cref="T:System.ArgumentNullException">
            /// The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.
            /// </exception>
            public int GetHashCode(ValidationElementResult obj)
            {
                return obj.ToString().GetHashCode();
            }
        }
    }
}