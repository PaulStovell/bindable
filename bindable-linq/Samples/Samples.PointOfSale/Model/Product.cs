using Bindable.Aspects.Binding;
using Samples.PointOfSale.Infrastructure;

namespace Samples.PointOfSale.Model
{
    public class Product : DomainObject
    {
        public string Name { get; [NotifyChange]set; }
        public decimal Price { get; [NotifyChange]set; }
    }
}
