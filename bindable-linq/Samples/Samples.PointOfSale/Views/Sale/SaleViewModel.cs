using System.Collections.ObjectModel;
using Bindable.Linq;
using Bindable.Linq.Interfaces;
using Samples.PointOfSale.Model;

namespace Samples.PointOfSale.Views.Sale
{
    public class SaleViewModel
    {
        private readonly ObservableCollection<LineItem> _lineItems = new ObservableCollection<LineItem>();
        private readonly IBindable<decimal> _total;
        
        public SaleViewModel()
        {
            _lineItems = new ObservableCollection<LineItem>();
            _total = _lineItems.AsBindable()
                        .Sum(lineItem => lineItem.TotalPrice)
                        .Project(total => total * (1M + Discount / 100M))
                        .Switch()
                            .Case(total => total < 10M, total => 10M)
                            .Default(total => total)
                        .EndSwitch();
        }

        public int Discount { get; set; }
        
        public ObservableCollection<LineItem> LineItems
        {
            get { return _lineItems; }
        }

        public IBindable<decimal> Total
        {
            get { return _total; }
        }

        public void AddItem(int productBarcode)
        {
            _lineItems.Add(new LineItem(new Product { Name = "Dirty diapers", Price = 4.00M }, 1));
        }
    }
}
