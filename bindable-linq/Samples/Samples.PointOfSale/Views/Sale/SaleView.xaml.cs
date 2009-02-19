using System.Windows.Controls;
using System.Windows.Data;

namespace Samples.PointOfSale.Views.Sale
{
    /// <summary>
    /// Interaction logic for SaleView.xaml
    /// </summary>
    public partial class SaleView : UserControl
    {
        public SaleView()
        {
            InitializeComponent();
        }

        public SaleViewModel Model
        {
            get { return (SaleViewModel)((ObjectDataProvider)Resources["SaleViewModel"]).ObjectInstance; }
            set { ((ObjectDataProvider)Resources["SaleViewModel"]).ObjectInstance = value; }
        }

        private void AddButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Model.AddItem(239873);
        }
    }
}
