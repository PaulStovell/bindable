using Bindable.Aspects.Binding;

namespace Bindable.Windows.ValidationSamples.Views.BookingCreation.Model
{
    public class Traveller
    {
        public string Name { get; [NotifyChange]set; }
        public string EmailAddress { get; [NotifyChange]set; }
        public string PhoneNumber { get; [NotifyChange]set; }
        public string CostCentre { get; [NotifyChange]set; }
    }
}