    using Npgsql;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
using static GMap.NET.Entity.OpenStreetMapGeocodeEntity;
using System.Windows.Threading;


namespace OOPWPFProject
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>

    public partial class Window2 : Window
    {
        courier courier1 = new courier();
        Delivery delivery = new Delivery();
        Tovar tov = new Tovar();
        Order order = new Order();
        private bool loggg = false;
        private string currentname = "";
        private bool IsAdminka = false;
        private string currentemail = "";
        private DataTable cart = new DataTable();
        private DataTable carts = new DataTable();
        private const string ConnectionString = "Host=localhost;Port=5432;Database=Order;Username=postgres;Password=promomo999;";
        DispatcherTimer timer = new DispatcherTimer();
        public Window2()
        {
            InitializeComponent();
            CreateCartTable();
            createorder();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Timer_Tick;
            
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            order.statusOrder = "Доставлено";

            order.changeStatus();
            StatusS.SelectedItem = "Доставлено";

            StatusS.Visibility = Visibility.Visible;
            OrdersList.ItemsSource = order.loadOrderCourier().DefaultView;

            MessageBox.Show("Замовлення доставлено!");

            timer.Stop();

        }
        private void createorder()
        {
            carts.Columns.Add("Дата");
            carts.Columns.Add("Сума");
            carts.Columns.Add("Ім'я");
        }

        public string NumberPhone;
        public string EmailAdres;
        public string Passwords;
        private bool isAdmin = false;
        private void CreateCartTable()
        {
            cart.Columns.Add("Назва");
            cart.Columns.Add("Ціна");
            cart.Columns.Add("Кількість");
            cart.Columns.Add("Сума");
            BasketGrid.ItemsSource = cart.DefaultView;
        }

        private void OrdList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            Tovary.Visibility = Visibility.Collapsed;
            Zamovlennya.Visibility = Visibility.Collapsed;
            basket.Visibility = Visibility.Collapsed;
            Login.Visibility = Visibility.Collapsed;
            Login.Visibility = Visibility.Collapsed;
            Profile.Visibility = Visibility.Collapsed;
            switch (OrdList.SelectedIndex)
            {
                case 0:
                    Tovary.Visibility = Visibility.Visible;
                    LoadTovar();
                    break;
                case 1:

                    Zamovlennya.Visibility = Visibility.Visible;
                    break;
                case 2:
                    basket.Visibility = Visibility.Visible;
                    break;
                case 3:
                    if (loggg)
                    {
                        Profile.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        Login.Visibility = Visibility.Visible;
                    }

                    break;
                case 4:
                    this.Close(); break;
            }


        }

        private void l_Checked(object sender, RoutedEventArgs e)
        {
            Log.Visibility = Visibility.Visible;
            Reg.Visibility = Visibility.Collapsed;
            Numbe.Visibility = Visibility.Collapsed;
            Email.Visibility = Visibility.Visible;
            EmailTxt.Visibility = Visibility.Visible;
            TxtPass.Visibility = Visibility.Visible;
            Password.Visibility = Visibility.Visible;
            NumTxt.Visibility = Visibility.Collapsed;
            Name_Fname.Visibility = Visibility.Collapsed;
            Name_FnameTxt.Visibility = Visibility.Collapsed;
            TovarCour.Visibility = Visibility.Visible;
            BasketCour.Visibility = Visibility.Visible;
            Code.Visibility = Visibility.Collapsed;
            CodeTxt.Visibility = Visibility.Collapsed;
            LogCourier.Visibility = Visibility.Collapsed;
        }

        private void r_Checked(object sender, RoutedEventArgs e)
        {
            Reg.Visibility = Visibility.Visible;
            Log.Visibility = Visibility.Collapsed;
            Numbe.Visibility = Visibility.Visible;
            NumTxt.Visibility = Visibility.Visible;
            Name_Fname.Visibility = Visibility.Visible;
            Name_FnameTxt.Visibility = Visibility.Visible;
            Email.Visibility = Visibility.Visible;
            EmailTxt.Visibility = Visibility.Visible;
            TxtPass.Visibility = Visibility.Visible;
            Password.Visibility = Visibility.Visible;
            TovarCour.Visibility = Visibility.Visible;
            BasketCour.Visibility = Visibility.Visible;
            Code.Visibility = Visibility.Collapsed;
            CodeTxt.Visibility = Visibility.Collapsed;
            LogCourier.Visibility = Visibility.Collapsed;

        }
        private void c_Checked(object sender, RoutedEventArgs e)
        {
            Log.Visibility = Visibility.Collapsed;
            Reg.Visibility = Visibility.Collapsed;
            Numbe.Visibility = Visibility.Collapsed;
            Email.Visibility = Visibility.Collapsed;
            EmailTxt.Visibility = Visibility.Collapsed;
            TxtPass.Visibility = Visibility.Collapsed;
            Password.Visibility = Visibility.Collapsed;
            NumTxt.Visibility = Visibility.Collapsed;
            Name_Fname.Visibility = Visibility.Collapsed;
            Name_FnameTxt.Visibility = Visibility.Collapsed;
            TovarCour.Visibility = Visibility.Collapsed;
            BasketCour.Visibility = Visibility.Collapsed;
            Code.Visibility = Visibility.Visible;
            CodeTxt.Visibility = Visibility.Visible;
            LogCourier.Visibility = Visibility.Visible;
        }
        private void Reg_Click(object sender, RoutedEventArgs e)
        {
            string email = Email.Text;
            string password = Password.Password;
            string numbers = Numbe.Text;
            bool valid = true;

            if (!email.Contains("@") || !email.Contains(".com"))
            {
                Email.ToolTip = "Введіть корректний email!";
                Email.Background = Brushes.Red;
                valid = false;
            }
            else

            {
                Email.ToolTip = "";
                Email.Background = Brushes.Transparent;
            }
            if (!Name_Fname.Text.All(c => char.IsLetter(c) || c == ' ') || string.IsNullOrWhiteSpace(Name_Fname.Text))
            {
                Name_Fname.ToolTip = "Введіть корректно прізвище або ім'я";
                Name_Fname.Background = Brushes.Red;
                valid = false;
            }
            else
            {
                Name_Fname.ToolTip = "";
                Name_Fname.Background = Brushes.Transparent;
            }
            if (!Numbe.Text.All(x => char.IsDigit(x) || x == '+') || Numbe.Text.Trim() == "")
            {
                Numbe.ToolTip = "Введіть корректний номер телефону!";
                Numbe.Background = Brushes.Red;
                valid = false;
            }
            else
            {
                Numbe.ToolTip = "";
                Numbe.Background = Brushes.Transparent;
            }
            if (password.Length < 6)
            {
                Password.ToolTip = "Пароль має бути мінімум 6 символів!";
                Password.Background = Brushes.Red;
                valid = false;
            }
            else
            {
                Password.ToolTip = "";
                Password.Background = Brushes.Transparent;
            }


            if (valid == true)
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    string checkQuery = "SELECT COUNT(*) FROM \"Login\" WHERE email=@email";
                    NpgsqlCommand checkCmd = new NpgsqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@email", email);
                    long exists = (long)checkCmd.ExecuteScalar();

                    if (exists > 0)
                    {
                        Email.Background = Brushes.Red;
                        Exit.Text = "Акаунт з таким email вже існує!";
                        Email.Clear();
                        Numbe.Clear();
                        Name_Fname.Clear();
                        Password.Clear();
                        return;
                    }
                    else
                    {


                        string query = "INSERT INTO \"Login\" (email, \"Number\", \"Name_Firstname\", \"Pass\") VALUES (@email, @number, @name, @pass)";
                        NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@number", Numbe.Text);
                        cmd.Parameters.AddWithValue("@name", Name_Fname.Text);
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password.Password);
                        cmd.Parameters.AddWithValue("@pass", hashedPassword); cmd.ExecuteNonQuery();
                        MessageBox.Show("Акаунт створено!");
                        AmountCombo.Visibility = Visibility.Visible;
                        AddOrder.Visibility = Visibility.Visible;
                        order.loadOrderUser();
                        Email.Clear();
                        Numbe.Clear();
                        Name_Fname.Clear();
                        Password.Clear();
                    }
                }
            }

        }

        private void selectAmount(object sender, SelectionChangedEventArgs e)
        {
            if (TovarGridOf.SelectedItem == null)
                return;

            DataRowView row = TovarGridOf.SelectedItem as DataRowView;

            if (row == null)
                return;

            AmountCombo.Items.Clear();

            int.TryParse(row["Amount"].ToString(), out int maxQuantity);
            if (maxQuantity == 0)
            {
                MessageBox.Show("Товару немає на складі");
                return;
            }

            for (int i = 1; i <= maxQuantity; i++)
            {
                AmountCombo.Items.Add(i);
            }

            AmountCombo.SelectedIndex = 0;
        }
        private void Log_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string email = Email.Text;
                string password = Password.Password;
                string name = Name_Fname.Text;

                if (email == "dtimasik@gmail.com" && password == "promomo999")
                {
                    loggg = true;
                    isAdmin = true;
                    currentname = "Адміністратор";
                    ProfileName.Text = "Ви увійшли як адміністратор";

                    loginpng.Source = new BitmapImage(new Uri("/profile.png", UriKind.Relative));
                    LoginTxt.Text = "Профіль";


                    addTovar.Visibility = Visibility.Visible;


                    Login.Visibility = Visibility.Collapsed;
                    Profile.Visibility = Visibility.Visible;
                    AmountCombo.Visibility = Visibility.Visible;
                    AddOrder.Visibility = Visibility.Visible;
                    Delete.Visibility = Visibility.Visible;
                    AddCourier.Visibility = Visibility.Visible;
                
                    return;
                }

                using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    string query = "SELECT \"Pass\", \"Name_Firstname\", \"Number\" FROM \"Login\" WHERE email=@email";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@email", email);
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            Exit.Text = "Невірний email або пароль";
                            return;
                        }

                        string Hash = reader["Pass"]?.ToString();
                        string nameFromDb = reader["Name_Firstname"]?.ToString();
                        NumberPhone = reader["Number"]?.ToString();

                        if (BCrypt.Net.BCrypt.Verify(password, Hash))
                        {
                            loggg = true;
                            currentname = nameFromDb;
                            currentemail = email;
                            ProfileName.Text = "Ваше ім'я та прізвище " + nameFromDb;

                            loginpng.Source = new BitmapImage(new Uri("/profile.png", UriKind.Relative));
                            LoginTxt.Text = "Профіль";

                            Login.Visibility = Visibility.Collapsed;
                            Profile.Visibility = Visibility.Visible;
                            AddOrder.Visibility = Visibility.Visible;
                            AmountCombo.Visibility = Visibility.Visible;
                            OrdersList.ItemsSource = order.loadOrderUser().DefaultView;
                           
                        }
                        else
                        {
                            Exit.Text = "Невірний email або пароль";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exit.Text = "Помилка:" + ex.Message;
            }
        }





        private void addTovar_Click(object sender, RoutedEventArgs e)
        {
            AddTovarWindow addt = new AddTovarWindow();
            addt.Show();
        }

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            if (TovarGridOf.SelectedItem == null)
            {
                MessageBox.Show("Виберіть товар");
                return;
            }

            if (AmountCombo.SelectedItem == null)
            {
                MessageBox.Show("Виберіть кількість");
                return;
            }


            DataRowView row = (DataRowView)TovarGridOf.SelectedItem;

            string name = row["NameProduct"].ToString();

            decimal.TryParse(row["Price"].ToString(), out decimal price);

            int.TryParse(AmountCombo.SelectedItem.ToString(), out int amount);

            decimal sum = price * amount;

            cart.Rows.Add(name, price, amount, sum);

            MessageBox.Show("Товар додано в кошик");


        }

        private void LoadTovar()
        {
            DataTable dt = tov.loadTovary();

            TovarGridOf.ItemsSource = dt.DefaultView;
        }




        private void exitAcc_Click(object sender, RoutedEventArgs e)
        {
            loggg = false;
            isAdmin = false;
            currentemail = "";
            currentname = "";
            LoginTxt.Text = "Увійти";
            loginpng.Source = new BitmapImage(new Uri("/login.png", UriKind.Relative));
            Profile.Visibility = Visibility.Collapsed;
            Login.Visibility = Visibility.Visible;
            AddOrder.Visibility = Visibility.Collapsed;
            Delete.Visibility = Visibility.Collapsed;
            addTovar.Visibility = Visibility.Collapsed;
            cart.Clear();
        }

        private void AddOrders_Click(object sender, RoutedEventArgs e)
        {

            bool valid = true;
            if (string.IsNullOrWhiteSpace(AdressDelivery.Text))
            {
                AdressDelivery.ToolTip = "Введіть адресу!";
                AdressDelivery.Background = Brushes.Red;
                valid = false;
            }
            else
            {
                AdressDelivery.ToolTip = "";
                AdressDelivery.Background = Brushes.White;

            }
            if (CityDelivery.SelectedItem == null)
            {
                CityDelivery.ToolTip = "Виберіть місто!";
                CityDelivery.Background = Brushes.Red;
                valid = false;
            }
            else
            {
                CityDelivery.ToolTip = "";
                CityDelivery.Background = Brushes.White;

            }
            if (!CartPayment.IsChecked.Value && !MoneyPayment.IsChecked.Value)
            {
                MessageBox.Show("Виберіть спосіб оплати!");
                valid = false;
            }

            if (!valid)
                return;


            if (CartPayment.IsChecked == true)
            {
                MessageBox.Show("Оплата картою");
            }
            if (MoneyPayment.IsChecked == true)
            {
                MessageBox.Show("Оплата готівкою");
            }
            if (valid)
            {
                delivery.addressDelivery = (AdressDelivery.SelectedItem as ComboBoxItem)?.Content.ToString();
                delivery.cityDelivery = (CityDelivery.SelectedItem as ComboBoxItem)?.Content.ToString();
                delivery.addDelivery();


                DataRowView row = (DataRowView)TovarGridOf.SelectedItem;

                order.nameProduct = row["NameProduct"].ToString();

                decimal.TryParse(row["Price"].ToString(), out decimal price);
                int.TryParse(AmountCombo.SelectedItem.ToString(), out int amount);

                using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    string sql = "UPDATE \"Tovar\" SET \"Amount\" = \"Amount\" - @count WHERE \"NameProduct\" = @name";

                    NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@count", amount);
                    cmd.Parameters.AddWithValue("@name", order.nameProduct);

                    cmd.ExecuteNonQuery();
                }
                order.numberClient = NumberPhone;
                order.totalPrice = price * amount;
                order.amountProduct = amount;

                order.dateOrder = DateTime.Now;
                order.clientName = currentname;
                order.addressOrder =
                (CityDelivery.SelectedItem as ComboBoxItem)?.Content.ToString()
                + ", " +
                AdressDelivery.Text;
                order.addorderAdmin();
                LoadTovar();
                OrdersList.ItemsSource = order.loadOrderUser().DefaultView;
            }
        }

        private void AddCourier_Click(object sender, RoutedEventArgs e)
        {
            RegistCourier courieropen = new RegistCourier();
            courieropen.Show();

        }

        private void CityDelivery_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AdressDelivery.Items.Clear();

            string city = (CityDelivery.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (city == "Київ")
            {
                AdressDelivery.Items.Add("Хрещатик");
                AdressDelivery.Items.Add("Оболонська");
                AdressDelivery.Items.Add("Саксаганського");
            }

            else if (city == "Львів")
            {
                AdressDelivery.Items.Add("Шевченка");
                AdressDelivery.Items.Add("Городоцька");
                AdressDelivery.Items.Add("Личаківська");
            }

            else if (city == "Житомир")
            {
                AdressDelivery.Items.Add("Київська");
                AdressDelivery.Items.Add("Перемоги");
                AdressDelivery.Items.Add("Велика Бердичівська");
            }
        }

        private void LogCourier_Click(object sender, RoutedEventArgs e)
        {
            courier1.codeLogin = Code.Text;

            DataTable dt = courier1.searchCode();

            if (dt.Rows.Count > 0)
            {
                loggg = true;

                currentname = "Кур'єр";
                ProfileName.Text = "Ви увійшли як Кур'єр";

                loginpng.Source = new BitmapImage(new Uri("/profile.png", UriKind.Relative));
                LoginTxt.Text = "Профіль";

                Login.Visibility = Visibility.Collapsed;
                Profile.Visibility = Visibility.Visible;

                OrdersList.ItemsSource = order.loadOrderCourier().DefaultView;

                MessageBox.Show("Успішний вхід!");
            }
            else
            {
                MessageBox.Show("Невірний код!");
            }
        
        }
        private void OrdersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrdersList.SelectedItem == null)
                return;

            DataRowView row = OrdersList.SelectedItem as DataRowView;

            if (row == null)
                return;

            string status = row["statusOrder"].ToString();

            if (status == "Доставлено")
            {
                StatusS.Visibility = Visibility.Collapsed;
                StatusSButton.Visibility = Visibility.Collapsed;
                return;
            }
            else
            {
                StatusS.Visibility = Visibility.Visible;
                StatusSButton.Visibility = Visibility.Visible;
            }

            StatusS.Items.Clear();

            StatusS.Items.Add("Очікується підтвердження");
            StatusS.Items.Add("В дорозі");
            StatusS.Items.Add("Доставлено");

            StatusS.SelectedItem = status;
        }

        private void StatusSButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersList.SelectedItem == null)
            {
                MessageBox.Show("Виберіть замовлення");
                return;
            }

            if (StatusS.SelectedItem == null)
            {
                MessageBox.Show("Виберіть статус");
                return;
            }
            DataRowView row = OrdersList.SelectedItem as DataRowView;

            int.TryParse(row["idOrder"].ToString(), out int id);
            order.idOrder = id;
            order.statusOrder = StatusS.SelectedItem.ToString();
            order.changeStatus();
            OrdersList.ItemsSource = order.loadOrderCourier().DefaultView;
            if (order.statusOrder == "В дорозі")
            {
                StatusS.Visibility = Visibility.Collapsed;
                order.idOrder = id;
                timer.Start();
            }
        }
    }
    }
