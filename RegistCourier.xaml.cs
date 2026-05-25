using Microsoft.Win32;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
    /// Логика взаимодействия для RegistCourier.xaml
    /// </summary>
    public partial class RegistCourier : Window
    {

        Courier couriers = new Courier();
        private void LoadCouriers()
        {
            CourierGrid.ItemsSource = couriers.loadCourier().DefaultView;
        }
        public RegistCourier()
        {
            InitializeComponent();
            LoadCouriers();
        }
       

        private void RegistrCourier_Click(object sender, RoutedEventArgs e)
        {
            bool valid = true;
            couriers.nameEmployee = fmaim_nameCour.Text;
            couriers.GenderCourier = (GenderCour.SelectedItem as ComboBoxItem)?.Content.ToString();
            couriers.codeLogin = CodeCourier.Text;
            couriers.cityCourier = (cityCour.SelectedItem as ComboBoxItem)?.Content.ToString();
            couriers.photoCourier = Photo1.Text;
            couriers.numberCourier = Number.Text;

            if (!fmaim_nameCour.Text.All(c => char.IsLetter(c) || c == ' ') || string.IsNullOrWhiteSpace(fmaim_nameCour.Text))
            {
                fmaim_nameCour.ToolTip = "Введіть корректно прізвище або ім'я";
                fmaim_nameCour.Background = Brushes.Red;
                valid = false;
            }
            else
            {
                fmaim_nameCour.ToolTip = "";
                fmaim_nameCour.Background = Brushes.Transparent;
            }
            if (!Number.Text.All(x => char.IsDigit(x) || x == '+') || Number.Text.Trim() == "" )
            {
                Number.ToolTip = "Введіть корректний номер телефону!";
                Number.Background = Brushes.Red;
                valid = false;
            }
            else
            {
                Number.ToolTip = "";
                Number.Background = Brushes.Transparent;
            }
            if (GenderCour.SelectedItem == null)
            {
                GenderCour.ToolTip = "Виберіть стать!";
                GenderCour.Background = Brushes.Red;
                valid = false;
            }
            else
            {
                GenderCour.ToolTip = "";
                GenderCour.Background = Brushes.Transparent;
            }
        
            if (cityCour.SelectedItem == null)
            {
                cityCour.ToolTip = "Виберіть місто!";
                cityCour.Background = Brushes.Red;
                valid = false;
            }
            else
            {
                cityCour.ToolTip = "";
                cityCour.Background = Brushes.Transparent;
            }

            if (string.IsNullOrWhiteSpace(Photo1.Text))
            {
                Photo1.ToolTip = "Додайте фото!";
                Photo1.Background = Brushes.Red;
                valid = false;
            }
            else
            {
                Photo1.ToolTip = "";
                Photo1.Background = Brushes.Transparent;
            }
            if (!int.TryParse(ageCour.Text, out int age))
            {
                ageCour.ToolTip = "Введіть коректний вік!";
                ageCour.Background = Brushes.Red;
                valid = false;
            }
            else
            {
                couriers.ageCourier = age;

                if (!couriers.IsageCourier())
                {
                    ageCour.ToolTip = "Введіть вік кур’єра від 17 років!";
                    ageCour.Background = Brushes.Red;
                    valid = false;
                }
                else
                {
                    ageCour.ToolTip = "";
                    ageCour.Background = Brushes.Transparent;
                }
            }

            if (string.IsNullOrWhiteSpace(CodeCourier.Text))
            {
                CodeCourier.ToolTip = "Додайте код!";
                CodeCourier.Background = Brushes.Red;
                valid = false;
            }
            else
            {
                CodeCourier.ToolTip = "";
                CodeCourier.Background = Brushes.Transparent;
            }
            if (!valid)
            {
                return;
            }
         
            if (couriers.addCourier())
            {

                LoadCouriers();
                fmaim_nameCour.Clear();
                ageCour.Clear();
                Number.Clear();
                Photo1.Clear();
                CodeCourier.Clear();

                GenderCour.SelectedIndex = -1;
                cityCour.SelectedIndex = -1;
            }
        }

        private void PhotoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                Photo1.Text = openFileDialog.FileName;
        }
      

        private void GenerationCode_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            string code = "";

            for (int i = 0; i < 8; i++)
            {
                code += chars[rnd.Next(chars.Length)];
            }
            CodeCourier.Text = code;
            
        }
    }
}
