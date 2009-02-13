using System.ComponentModel;
using Bindable.Aspects.Binding;

namespace Bindable.Windows.ValidationSamples.Smart
{
    public class BindableObject : INotifyPropertyChanged, ICanRaisePropertyChangedEvents
    {
        public BindableObject()
        {
            
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChangedEvent(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, e);
        }
    }
}
