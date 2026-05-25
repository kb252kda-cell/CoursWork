using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OOPWPFProject
{
    /// <summary>
    /// Логика взаимодействия для Courier.xaml
    /// </summary>
    public partial class CourierInfo : Window
    {
        public CourierInfo()
        {
            InitializeComponent();
            CourierGrid.ItemsSource = couriers.loadCourier().DefaultView;
        }
        Courier couriers = new Courier();
       
    }
    
    
}
