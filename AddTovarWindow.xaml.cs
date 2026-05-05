using Npgsql;
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
using System.Xml.Linq;


namespace OOPWPFProject
{
    /// <summary>
    /// Логика взаимодействия для AddTovarWindow.xaml
    /// </summary>
    public partial class AddTovarWindow : Window
    {
        public AddTovarWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
        private void AddMot_Click(object sender, RoutedEventArgs e)
        {
            int amount;
            if (!int.TryParse(AmountTxt.Text, out amount))
            {
                MessageBox.Show("Некоректна кількість!");
                return;
            }
            decimal price;
            if (decimal.TryParse(PriceTovar.Text, out price)){
                string connectionString = "Host=localhost;Port=5432;Database=Order;Username=postgres;Password=promomo999;";

                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();



                    string query = "INSERT INTO \"Tovar\" (\"Ім'я\", \"Категорія\", \"Ціна\",\"Кількість\") VALUES (@namep, @categ, @price,@amount)";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@namep", nameTovar.Text);
                    cmd.Parameters.AddWithValue("@categ", categoryProduct.Text);
                    cmd.Parameters.AddWithValue("@price", PriceTovar.Text);
                    cmd.Parameters.AddWithValue("@amount", amount);
                    

                    nameTovar.Clear();
                    PriceTovar.Clear();
                    cmd.ExecuteNonQuery();

                }
            }
            else
            {
                MessageBox.Show("Некоректна ціна!");
            }

        }
    }
}

