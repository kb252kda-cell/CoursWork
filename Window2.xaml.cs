    using Npgsql;
using OOPWPFProject.Class;
using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;
using static GMap.NET.Entity.OpenStreetMapGeocodeEntity;


namespace OOPWPFProject
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>

    public partial class Window2 : Window
    {
        Courier courier1 = new Courier();
        Delivery delivery = new Delivery();
        Tovar tov = new Tovar();
        Order order = new Order();
        User currentUser;
        RegularUser users = new RegularUser();
        private bool isLoggedIn = false;
        private int currentOrderId;
        private string currentname = "";
        private string currentemail = "";
        private DataTable cart = new DataTable();
        private DataTable carts = new DataTable();
        DispatcherTimer timer = new DispatcherTimer();
        private bool isCourier = false;
        private void UpdateBalanceUI()
        {
            DepositBalance db = new DepositBalance();
            decimal balance = db.GetBalance(NumberPhone);
            BalanceText.Text = $"Баланс: {balance} грн";

            Deposit.Visibility = balance < 60 ? Visibility.Visible : Visibility.Collapsed;
        }
        public Window2()
        {
            InitializeComponent();
            CreateCartTable();
            createorder();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Timer_Tick;
            
        }
        private void LoadCourierOrders()
        {
            DataTable dt = order.loadOrderCourier();

            var filtered = dt.AsEnumerable()
                             .Where(row => row["statusOrder"].ToString() != "Доставлено");

            if (filtered.Any())
            {
                OrdersList.ItemsSource =
                    filtered.CopyToDataTable().DefaultView;
            }
            else
            {
                OrdersList.ItemsSource = null;
            }
        }
        private void UpdateTotal()
        {
            if (cart.Rows.Count == 0)
            {
                TotalText.Text = "Загальна сума: 0 грн";
                return;
            }

            decimal total = cart.AsEnumerable()
                .Sum(r => Convert.ToDecimal(r["Сума"]));

            TotalText.Text = "Загальна сума: " + total + " грн";
        }
        private void Timer_Tick(object sender, EventArgs e)
        {

            order.idOrder = currentOrderId;
            order.statusOrder = "Доставлено";
            order.changeStatus();

            decimal courierPayment = 60; 
            DepositBalance db = new DepositBalance();
            bool paid = db.AddCourierPayment(courier1.codeLogin, courierPayment);

            if (paid)
                MessageBox.Show($"Замовлення доставлено! Вам нараховано {courierPayment} грн");
            else
                MessageBox.Show(" Замовлення доставлено!");

            StatusS.Visibility = Visibility.Visible;
            LoadCourierOrders();
            timer.Stop();


        }
        private void ResetControl(Control control)
        {
            control.ToolTip = "";
            control.Background = Brushes.Transparent;
        }
        private void createorder()
        {
            carts.Columns.Add("Дата");
            carts.Columns.Add("Сума");
            carts.Columns.Add("Ім'я");
        }
        private void HideAllPanels()
        {
            Tovary.Visibility = Visibility.Collapsed;
            Zamovlennya.Visibility = Visibility.Collapsed;
            basket.Visibility = Visibility.Collapsed;
            Login.Visibility = Visibility.Collapsed;
            Profile.Visibility = Visibility.Collapsed;
        }
        public string NumberPhone;

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


            HideAllPanels();
            if (!isLoggedIn && OrdList.SelectedIndex != 3)
            {
                Login.Visibility = Visibility.Visible;

                Tovary.Visibility = Visibility.Collapsed;
                Zamovlennya.Visibility = Visibility.Collapsed;
                basket.Visibility = Visibility.Collapsed;
                Profile.Visibility = Visibility.Collapsed;


                return;
            }
                switch (OrdList.SelectedIndex)
            {
                case 0:
                    Tovary.Visibility = Visibility.Visible;
                    LoadTovar();
                    break;
                case 1:
                    Zamovlennya.Visibility = Visibility.Visible;

                    if (isLoggedIn && currentUser != null)
                    {
                        OrdersList.ItemsSource =
                            currentUser.ShowOrders(order).DefaultView;

                    }
                    break;
                case 2:
                    basket.Visibility = Visibility.Visible;
                    if (isLoggedIn)
                    {
                        DeliveryStace.Visibility = Visibility.Visible;
                        ButtonInfoCourier.Visibility = Visibility.Visible;
                        paymnetStack.Visibility = Visibility.Visible;
                        AddOrders.Visibility = Visibility.Visible;
                        CityDelivery.Visibility = Visibility.Visible;
                        DeleteOrder.Visibility = Visibility.Visible;
                        SearchNameOrder.Visibility = Visibility.Visible;
                        SearchButtonName.Visibility = Visibility.Visible;
                        SortPrice.Visibility = Visibility.Visible;
                        BuyMenu.Visibility = Visibility.Visible;
                        OrdersPhoto.Visibility = Visibility.Visible;
                        BusketPhoto.Visibility = Visibility.Visible;
                        UpdateBalanceUI();

                    }
                    else
                    {
                        SortPrice.Visibility = Visibility.Collapsed;
                        SearchButtonName.Visibility = Visibility.Collapsed;
                        AddOrder.Visibility = Visibility.Collapsed;
                        Deposit.Visibility = Visibility.Collapsed;
                        SearchNameOrder.Visibility = Visibility.Collapsed;
                        CityDelivery.Visibility = Visibility.Collapsed;
                        ButtonInfoCourier.Visibility = Visibility.Collapsed;
                        DeliveryStace.Visibility = Visibility.Collapsed;
                        paymnetStack.Visibility = Visibility.Collapsed;
                        DeleteOrder.Visibility = Visibility.Collapsed;
                      
                    }
                    break;
                case 3:
                    if (isLoggedIn)
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
        private void ShowProfile()
        {
            Login.Visibility = Visibility.Collapsed;
            Profile.Visibility = Visibility.Visible;
        }
        private void l_Checked(object sender, RoutedEventArgs e)
        {
            Log.Visibility = Visibility.Visible;
            Reg.Visibility = Visibility.Collapsed;
            Numbe.Visibility = Visibility.Collapsed;
            Email.Visibility = Visibility.Visible;
            
            Password.Visibility = Visibility.Visible;
            Name_Fname.Visibility = Visibility.Collapsed;
            TovarCour.Visibility = Visibility.Visible;
            BasketCour.Visibility = Visibility.Visible;
            Code.Visibility = Visibility.Collapsed;
            LogCourier.Visibility = Visibility.Collapsed;
        }

        private void r_Checked(object sender, RoutedEventArgs e)
        {
            Reg.Visibility = Visibility.Visible;
            Log.Visibility = Visibility.Collapsed;
            Numbe.Visibility = Visibility.Visible;
            Name_Fname.Visibility = Visibility.Visible;
            Email.Visibility = Visibility.Visible;
            Password.Visibility = Visibility.Visible;
            TovarCour.Visibility = Visibility.Visible;
            BasketCour.Visibility = Visibility.Visible;
            Code.Visibility = Visibility.Collapsed;
            LogCourier.Visibility = Visibility.Collapsed;

        }
        private void c_Checked(object sender, RoutedEventArgs e)
        {
            Log.Visibility = Visibility.Collapsed;
            Reg.Visibility = Visibility.Collapsed;
            Numbe.Visibility = Visibility.Collapsed;
            Email.Visibility = Visibility.Collapsed;
            Password.Visibility = Visibility.Collapsed;
            Name_Fname.Visibility = Visibility.Collapsed;
            TovarCour.Visibility = Visibility.Collapsed;
            BasketCour.Visibility = Visibility.Collapsed;
            Code.Visibility = Visibility.Visible;
            LogCourier.Visibility = Visibility.Visible;
        }
        private void Reg_Click(object sender, RoutedEventArgs e)
        {
            users.email = Email.Text;
            users.Number = Numbe.Text;
            users.Password = Password.Password;
            users.Name_Firstname = Name_Fname.Text;
            bool valid = true;
            currentUser = new RegularUser();

            
            

            if (!users.IsValidEmail())
                {
                Email.ToolTip = "Введіть коректний email!";
                Email.Background = Brushes.Red;
                valid = false;
            }
            else
            {
                ResetControl(Email);
            }
            if (!Name_Fname.Text.All(c => char.IsLetter(c) || c == ' ') || string.IsNullOrWhiteSpace(Name_Fname.Text))
            {
                Name_Fname.ToolTip = "Введіть корректно прізвище або ім'я";
                Name_Fname.Background = Brushes.Red;
                valid = false;
            }
            else
            {
                ResetControl(Name_Fname);
            }
            if (!users.IsValidNumber() || Numbe.Text.Trim() == "")
            {
                Numbe.ToolTip = "Введіть корректний номер телефону!";
                Numbe.Background = Brushes.Red;
                valid = false;
            }
            else
            {
                ResetControl(Numbe);
            }
            if (!users.IsValidPassword())
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
            if (!valid)
            {
                return;
            }

            if (users.Register())
            {

                MessageBox.Show("Акаунт створено!");

                AmountCombo.Visibility = Visibility.Visible;
                AddOrder.Visibility = Visibility.Visible;
                AddCourier.Visibility = Visibility.Visible;
                AdressDelivery.Visibility = Visibility.Visible;
                CartPayment.Visibility = Visibility.Visible;
                MoneyPayment.Visibility = Visibility.Visible;
                SearchCategori.Visibility = Visibility.Visible;

                Email.Clear();
                Numbe.Clear();
                Name_Fname.Clear();
                Password.Clear();

            }
            else {
                Email.Background = Brushes.Red;
                Exit.Text = "Акаунт з таким email вже існує!";
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
                Admin admin = new Admin();
                if (admin.IsAdminCredentials(email, password))
                {
                    currentUser = new Admin();

                    isLoggedIn = true;
                    isCourier = false;
                    currentname = "Адміністратор";

                    ProfileName.Text = "Ви увійшли як адміністратор";

                    loginpng.Source = new BitmapImage(new Uri("/Photos/profile.png", UriKind.Relative));
                    LoginTxt.Text = "Профіль";

                    addTovar.Visibility = Visibility.Visible;

                    ShowProfile();

                    AmountCombo.Visibility = Visibility.Visible;
                    AddOrder.Visibility = Visibility.Visible;
                    Delete.Visibility = Visibility.Visible;
                    AddCourier.Visibility = Visibility.Visible;
                    ((ListBoxItem)OrdList.ItemContainerGenerator.ContainerFromIndex(2)).Visibility = Visibility.Collapsed;
                    OrdersList.ItemsSource = order.loadOrderAdmin().DefaultView;

                    return;
                }

                users.email = Email.Text;
                users.Password = Password.Password;
                currentUser = users;

                        if (users.Login())
                        {
                    isCourier = false;
                    isLoggedIn = true;
                   
                    currentname = users.Name_Firstname;
                    order.clientName = currentname;
                    NumberPhone = users.Number;
                    NumberPhone = users.Number;
                    UpdateBalanceUI();

                       currentemail = email;
                            ProfileName.Text = "Ваше ім'я та прізвище " + users.Name_Firstname;

                            loginpng.Source = new BitmapImage(new Uri("/Photos/profile.png", UriKind.Relative));
                            LoginTxt.Text = "Профіль";

                            ShowProfile();
                            AddOrder.Visibility = Visibility.Visible;
                            AmountCombo.Visibility = Visibility.Visible;
                            OrdersList.ItemsSource = order.loadOrderUser().DefaultView;
                            AddCourier.Visibility = Visibility.Collapsed;
                            AddOrders.Visibility = Visibility.Visible;
                            basket.Visibility = Visibility.Visible;
                            SearchCategori.Visibility = Visibility.Visible;
                            StackSearch.Visibility = Visibility.Visible;
                    TovarCour.Visibility = Visibility.Visible;
                    BasketCour.Visibility = Visibility.Visible;
                    BuyMenu.Visibility = Visibility.Visible;
                    OrdersPhoto.Visibility = Visibility.Visible;
                    BusketPhoto.Visibility = Visibility.Visible;
                    Email.Clear();
                    Password.Clear();

                }
                        else
                        {
                            Exit.Text = "Невірний email або пароль";
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
            UpdateTotal();

            MessageBox.Show("Товар додано в кошик");


        }

        private void LoadTovar()
        {
            DataTable dt = tov.loadTovary();

            TovarGridOf.ItemsSource = dt.DefaultView;
        }




        private void exitAcc_Click(object sender, RoutedEventArgs e)
        {
            isLoggedIn = false;
            isCourier = false;
            isAdmin = false;
            currentemail = "";
            currentname = "";
            LoginTxt.Text = "Увійти";
            loginpng.Source = new BitmapImage(new Uri("/Photos/login.png", UriKind.Relative));
            Profile.Visibility = Visibility.Collapsed;
            Login.Visibility = Visibility.Visible;
            AddOrder.Visibility = Visibility.Collapsed;
            Delete.Visibility = Visibility.Collapsed;
            addTovar.Visibility = Visibility.Collapsed;
            StatusS.Visibility = Visibility.Collapsed;
            StatusSButton.Visibility = Visibility.Collapsed;
            AddCourier.Visibility = Visibility.Collapsed;
            SearchCategori.Visibility = Visibility.Collapsed;
            StackSearch.Visibility = Visibility.Collapsed;
            AddOrders.Visibility = Visibility.Collapsed;
            BuyMenu.Visibility = Visibility.Collapsed;
            OrdersPhoto.Visibility = Visibility.Collapsed;
            BusketPhoto.Visibility = Visibility.Collapsed;
            ((ListBoxItem)OrdList.ItemContainerGenerator.ContainerFromIndex(2)).Visibility = Visibility.Visible;
            cart.Clear();
            OrdersList.ItemsSource = null;

        }

        private void AddOrders_Click(object sender, RoutedEventArgs e)
        {
            

            bool valid = true;
            if (cart.Rows.Count == 0)
            {
                MessageBox.Show("Виберіть замовлення");
                valid = false;
            }
            if (string.IsNullOrWhiteSpace(AdressDelivery.Text?.Trim()))
            {
                AdressDelivery.ToolTip = "Введіть адресу!";
                AdressDelivery.Background = Brushes.Red;
                valid = false;
            }
            else
            {
                ResetControl(AdressDelivery);

            }
            if (CityDelivery.SelectedItem == null)
            {
                CityDelivery.ToolTip = "Виберіть місто!";
                CityDelivery.Background = Brushes.Red;
                valid = false;
            }
            else
            {
                ResetControl(CityDelivery);

            }
            if (CartPayment.IsChecked != true && MoneyPayment.IsChecked != true)
            {
                MessageBox.Show("Виберіть спосіб оплати!");
                valid = false;
            }

            if (!valid)
            { return; 
            }

            if (CartPayment.IsChecked == true)
            {
                decimal total = cart.AsEnumerable().Sum(r => Convert.ToDecimal(r["Сума"]));

                DepositBalance db = new DepositBalance();
                decimal currentBalance = db.GetBalance(NumberPhone);

                if (currentBalance < total)
                {
                    MessageBox.Show($"Недостатньо коштів!\nБаланс: {currentBalance} грн\nСума: {total} грн");
                    return;
                }

                bool deducted = db.DeductBalance(NumberPhone, total);
                if (!deducted)
                {
                    MessageBox.Show("Помилка списання коштів!");
                    return;
                }

                UpdateBalanceUI(); 
            }


            if (valid)
            {
                string city = (CityDelivery.SelectedItem as ComboBoxItem)?.Content.ToString();

                string address = AdressDelivery.Text?.Trim();
                delivery.addressDelivery = address;
                delivery.cityDelivery = city;

                order.addressOrder = city + ", " + address;

                string courierName = courier1.GetRandomCourier(city);
                if (string.IsNullOrEmpty(courierName))
                {
                    MessageBox.Show("Немає доступних кур'єрів у цьому місті");
                    return;
                }

                delivery.CreateDelivery(city, address);



           

                foreach (DataRow row in cart.Rows)
                {
                    string name = row["Назва"].ToString();

                    decimal.TryParse(row["Ціна"].ToString(), out decimal price);

                    int.TryParse(row["Кількість"].ToString(), out int amount);

                    order.nameProduct = name;

                    tov.nameTovar1 = name;
                    tov.amountTovar1 = amount;

                    tov.Update();

                    order.CreateOrder(currentname, NumberPhone, city, price, amount);
                }
                cart.Clear();
                UpdateTotal();
                MessageBox.Show("Замовлення оформлено!");
                AdressDelivery.Items.Clear();
                AdressDelivery.SelectedItem = null;
                CityDelivery.SelectedItem = null;
                CartPayment.IsChecked = false;
                MoneyPayment.IsChecked = false;
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
                isLoggedIn  = true;
                isCourier = true;

                currentname = "Кур'єр";
                ProfileName.Text = "Ви увійшли як Кур'єр";

                loginpng.Source = new BitmapImage(new Uri("/Photos/profile.png", UriKind.Relative));
                LoginTxt.Text = "Профіль";
                NumberPhone = courier1.codeLogin;
                ShowProfile();
                StatusS.Visibility = Visibility.Visible;
                StatusSButton.Visibility = Visibility.Visible;
                LoadCourierOrders();
                Code.Clear();
                HideAllPanels();
                Zamovlennya.Visibility = Visibility.Visible;
                LoadCourierOrders();

                OrdList.SelectedIndex = 1;
                OrdersPhoto.Visibility = Visibility.Visible;
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
            if (!isCourier)
            {
                StatusS.Visibility = Visibility.Collapsed;
                StatusSButton.Visibility = Visibility.Collapsed;
                return;
            }
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
            LoadCourierOrders();
            if (order.statusOrder == "В дорозі")
            {
                StatusS.Visibility = Visibility.Collapsed;

                currentOrderId = id;
                timer.Start();
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            tov.nameTovar1 = SearchName.Text;

            DataTable dt = tov.searchTovarname();

            TovarGridOf.ItemsSource = dt.DefaultView;
        }

        private void SearchCategori_Selected(object sender, RoutedEventArgs e)
        {
            if (SearchCategori.SelectedItem == null)
            {
                return;
            }
            ComboBoxItem item = SearchCategori.SelectedItem as ComboBoxItem;

            string category = item.Content.ToString();

            if (category == "Всі")
            {
                TovarGridOf.ItemsSource = tov.loadTovary().DefaultView;
            }
            else
            {
                tov.categoriTovar1 = category;
                TovarGridOf.ItemsSource = tov.searchCategori().DefaultView;
                
                }

        }

        private void SortPrice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SortPrice.SelectedItem == null)
                return;

            ComboBoxItem item = SortPrice.SelectedItem as ComboBoxItem;

            string sort = item.Content.ToString();

            DataTable dt = order.loadOrderUser();

            if (sort == "Спочатку дорожчі")
            {
                var sorted = dt.AsEnumerable()
                               .OrderByDescending(row => Convert.ToDecimal(row["totalPrice"]));

                OrdersList.ItemsSource = sorted.CopyToDataTable().DefaultView;
            }
            else if (sort == "Спочатку дешевші")
            {
                var sorted = dt.AsEnumerable()
                               .OrderBy(row => Convert.ToDecimal(row["totalPrice"]));

                OrdersList.ItemsSource = sorted.CopyToDataTable().DefaultView;
            }
            else if (sort == "Всі")
            {
                OrdersList.ItemsSource = dt.DefaultView;
            }
        }

        private void ButtonInfoCourier_Click(object sender, RoutedEventArgs e)
        {
            CourierInfo courierwindow = new CourierInfo();
            courierwindow.Show();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (TovarGridOf.SelectedItem == null)
            {
                MessageBox.Show("Виберіть товар для видалення!");
                return;
            }

            DataRowView row = TovarGridOf.SelectedItem as DataRowView;

            if (row == null)
                return;

            if (int.TryParse(row["IdTovar"].ToString(), out int id))
            {
                tov.idProduct = id;

                tov.deleteTovar();

                LoadTovar();
            }
            else
            {
                MessageBox.Show("Помилка ID товару!");
            }
        
    }
        private void RefreshOrders()
        {
            order.clientName = currentname;

            DataTable dt;

            if (isCourier)
            {
                dt = order.loadOrderCourier();
            }
            else if (currentUser is Admin)
            {
                dt = order.loadOrderAdmin();
            }
            else
            {
                dt = order.loadOrderUser();
            }

            OrdersList.ItemsSource = null;
            OrdersList.ItemsSource = dt.DefaultView;

            OrdersList.Items.Refresh();
        }

        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersList.SelectedItem == null)
            {
                MessageBox.Show("Виберіть товар для видалення!");
                return;
            }
            DataRowView row = OrdersList.SelectedItem as DataRowView;

            if (int.TryParse(row["idOrder"].ToString(), out int id))
            {
                order.idOrder = id;

                order.deleteorder();

                RefreshOrders();
            }
            else
            {
                MessageBox.Show("Помилка ID товару!");
            }
        }

        private void SearchButtonName_Click(object sender, RoutedEventArgs e)
        {
            string search = SearchNameOrder.Text.Trim();

            if (string.IsNullOrWhiteSpace(search))
            {
                RefreshOrders();
                return;
            }

            order.clientName = currentname;
            order.nameProduct = search;

            DataTable dt = order.searchOrder();

            OrdersList.ItemsSource = null;
            OrdersList.ItemsSource = dt.DefaultView;

        }
       
        private void Deposit_Click(object sender, RoutedEventArgs e)
        {
            DepositWindow depositWindow = new DepositWindow(NumberPhone);
            depositWindow.Closed += (s, args) =>
            {
                UpdateBalanceUI();

            };
            depositWindow.Show();
        }
    }
    }
