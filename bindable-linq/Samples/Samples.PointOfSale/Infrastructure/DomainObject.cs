using System.ComponentModel;
using Bindable.Aspects.Binding;

namespace Samples.PointOfSale.Infrastructure
{
    public abstract class DomainObject : INotifyPropertyChanged, ICanRaisePropertyChangedEvents
    {
        public event PropertyChangedEventHandler PropertyChanged;

        void ICanRaisePropertyChangedEvents.RaisePropertyChangedEvent(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, e);
        }
    }
}
