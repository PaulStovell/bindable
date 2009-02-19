namespace Bindable.Linq.Tests.MockObjectModel
{
    using System.ComponentModel;

    /// <summary>
    /// Base class for sample business objects.
    /// </summary>
    public class BindableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets a value indicating whether this instance has property changed subscribers.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has property changed subscribers; otherwise, <c>false</c>.
        /// </value>
        public bool HasPropertyChangedSubscribers
        {
            get { return PropertyChanged != null; }
        }

        #region INotifyPropertyChanged Members
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        /// <summary>
        /// Raises the <see cref="E:PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
    }
}