    using Microsoft.Win32;
    using System.Data;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    namespace OOPWPFProject
        {
            /// <summary>
            /// Логика взаимодействия для AddTovarWindow.xaml
            /// </summary>
            public partial class AddTovarWindow : Window
            {
                Tovar tovar = new Tovar();
                public AddTovarWindow()
                {
                    InitializeComponent();
                    DataContext = this;
                LoadTovar();

                }
        
                private void AddTovar_Click(object sender, RoutedEventArgs e)
                {
                bool valid = true;
            
                if (string.IsNullOrWhiteSpace(nameTovar.Text))
                {
                    nameTovar.ToolTip = "Введіть назву товару";
                    nameTovar.Background = Brushes.Red;
                    valid = false;
                }
                else {
                    nameTovar.ToolTip = "";
                    nameTovar.Background = Brushes.Transparent;
                }
                if (!int.TryParse(amountTovar.Text, out int amount) || amount <= 0)
                {
                    amountTovar.ToolTip = "Введіть кількість товару!";
                    amountTovar.Background = Brushes.Red;
                    valid = false;

                }
                else
                {
                    amountTovar.ToolTip = "";
                    amountTovar.Background = Brushes.Transparent;
                }
                if (!decimal.TryParse(priceTovar.Text, out decimal price) || price <= 0)
                {
                    priceTovar.ToolTip = "Ведіть ціну!";
                    priceTovar.Background = Brushes.Red;
                    valid = false;

                }
                else
                {
                    priceTovar.ToolTip = "";
                    priceTovar.Background = Brushes.Transparent;
                }
                if (categoriTovar.SelectedItem == null)
                {
                    categoriTovar.ToolTip = "Виберіть категорію!";
                    categoriTovar.Background = Brushes.Red;
                    valid = false;
                }
                else
                {
                    categoriTovar.ToolTip = "";
                    categoriTovar.Background = Brushes.Transparent;
                }
                if (string.IsNullOrWhiteSpace(photoTovar.Text))
                {
                    photoTovar.ToolTip = "Виберіть фото!";
                    photoTovar.Background = Brushes.Red;
                    valid = false;
                }
                else
                {
                    photoTovar.ToolTip = "";
                    photoTovar.Background = Brushes.Transparent;
                }
                if (valid)
                {
                    tovar.nameTovar1 = nameTovar.Text;

                    tovar.amountTovar1 = amount;
                    tovar.priceTovar1 = price;
                    tovar.categoriTovar1 = categoriTovar.Text;
                    tovar.photoTovar1 = photoTovar.Text;
                    tovar.addproduct();
                    MessageBox.Show("Товари додано!");
                    LoadTovar();
                    nameTovar.Clear();
                    amountTovar.Clear();
                    priceTovar.Clear();
                    photoTovar.Clear();
                    categoriTovar.SelectedIndex = -1;
                }

                }

                private void PhotoButton_Click(object sender, RoutedEventArgs e)
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    if (openFileDialog.ShowDialog() == true)
                        photoTovar.Text = openFileDialog.FileName;
                }
            private void LoadTovar()
            {
                TovarGrid.ItemsSource = tovar.loadTovary().DefaultView;
            }

            private void Delete_tovar_Click(object sender, RoutedEventArgs e)
            {
                if (TovarGrid.SelectedItem != null)
                {
                    DataRowView row = (DataRowView)TovarGrid.SelectedItem;

                    bool success = int.TryParse(row["IdTovar"].ToString(), out int id);

                    tovar.idProduct = id;

                    tovar.deleteTovar();
                
                    TovarGrid.ItemsSource = tovar.loadTovary().DefaultView;
                }
            }
            private void TovarGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                if (TovarGrid.SelectedItem != null)
                {
                    DataRowView row = (DataRowView)TovarGrid.SelectedItem;

                    nameTovar.Text = row["NameProduct"].ToString();
                    priceTovar.Text = row["Price"].ToString();
                    amountTovar.Text = row["Amount"].ToString();
                    photoTovar.Text = row["PhotoProduct"].ToString();
                    categoriTovar.Text = row["Categori"].ToString();
                }
            }
            private void Edit_tovar_Click(object sender, RoutedEventArgs e)
            {
                if (TovarGrid.SelectedItem != null)
                {
                    DataRowView row = (DataRowView)TovarGrid.SelectedItem;
                int.TryParse(row["IdTovar"].ToString(), out int id); tovar.idProduct = id;
                    tovar.nameTovar1 = nameTovar.Text;
                    bool valid = true;

                    if (string.IsNullOrWhiteSpace(nameTovar.Text))
                    {
                        nameTovar.ToolTip = "Введіть назву товару";
                        nameTovar.Background = Brushes.LightPink;
                        valid = false;
                    }
                    else
                    {
                        nameTovar.ToolTip = "";
                        nameTovar.Background = Brushes.Transparent;
                    }
                    if (!int.TryParse(amountTovar.Text, out int amount) || amount <= 0)
                    {
                        amountTovar.ToolTip = "Введіть кількість товару!";
                        amountTovar.Background = Brushes.LightPink;
                        valid = false;

                    }
                    else
                    {
                        amountTovar.ToolTip = "";
                        amountTovar.Background = Brushes.Transparent;
                    }
                    if (!decimal.TryParse(priceTovar.Text, out decimal price) || price <= 0)
                    {
                        priceTovar.ToolTip = "Ведіть ціну!";
                        priceTovar.Background = Brushes.LightPink;
                        valid = false;

                    }
                    else
                    {
                        priceTovar.ToolTip = "";
                        priceTovar.Background = Brushes.Transparent;
                    }
                    if (categoriTovar.SelectedItem == null)
                    {
                        categoriTovar.ToolTip = "Виберіть категорію!";
                        categoriTovar.Background = Brushes.LightPink;
                        valid = false;
                    }
                    else
                    {
                        categoriTovar.ToolTip = "";
                        categoriTovar.Background = Brushes.Transparent;
                    }
                    if (string.IsNullOrWhiteSpace(photoTovar.Text))
                    {
                        photoTovar.ToolTip = "Виберіть фото!";
                        photoTovar.Background = Brushes.LightPink;
                        valid = false;
                    }
                    else
                    {
                        photoTovar.ToolTip = "";
                        photoTovar.Background = Brushes.Transparent;
                    }
                    if (valid)
                    {
                        tovar.amountTovar1 = amount;
                        tovar.priceTovar1 = price;
                        tovar.categoriTovar1 = categoriTovar.Text;
                        tovar.photoTovar1 = photoTovar.Text;
                        tovar.editTovar();
                        TovarGrid.ItemsSource = tovar.loadTovary().DefaultView;

                    }
                }
            }
        }
        }

