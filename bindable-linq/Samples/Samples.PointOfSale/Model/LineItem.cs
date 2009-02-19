using Bindable.Aspects.Binding;
using Samples.PointOfSale.Infrastructure;

namespace Samples.PointOfSale.Model
{
    public class LineItem : DomainObject
    {
        public LineItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public Product Product { get; private set; }
        public int Quantity { get; [NotifyChange]set; }
        public int Discount { get; [NotifyChange]set; }

        public decimal TotalPrice
        {
            [DependsOn("Product", "Quantity", "Discount")]
            get
            {
                return (Product.Price * Quantity) * (1.00M + (Discount / 100.00M));
            }
        }
    }
}