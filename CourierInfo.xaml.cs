using System.Data;
using System.Linq;
using System.Windows;

namespace OOPWPFProject
{
    public partial class CourierInfo : Window
    {
        public CourierInfo(string courierName)
        {
            InitializeComponent();
            Courier courier = new Courier();
            DataTable dt = courier.loadCourier();
            var filtered = dt.AsEnumerable()
                .Where(r => r["ElementName"].ToString() == courierName);
            if (filtered.Any())
                CourierGrid.ItemsSource = filtered.CopyToDataTable().DefaultView;
        }
    }
}