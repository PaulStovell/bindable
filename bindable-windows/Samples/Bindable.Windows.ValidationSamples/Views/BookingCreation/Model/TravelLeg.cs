using Bindable.Aspects.Binding;

namespace Bindable.Windows.ValidationSamples.Views.BookingCreation.Model
{
    public class TravelLeg
    {
        public string FromDestination { get; [NotifyChange]set; }
        public string ToDestination { get; [NotifyChange]set; }
        public string Date { get; [NotifyChange]set; }
        public string TravelTime { get; [NotifyChange]set; }
    }
}