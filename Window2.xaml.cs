    using Npgsql;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
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
    /// Логика взаимодействия для Window2.xaml
    /// </summary>

    public partial class Window2 : Window
    {



        private bool loggg = false;
        private string currentname = "";
        private bool IsAdminka = false;
        private string currentemail = "";
        private DataTable cart = new DataTable();
        private DataTable carts = new DataTable();
        public Window2()
        {
            InitializeComponent();
            CreateCartTable();
            createorder();
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
                    LoadTovary();
                    break;
                case 1:

                    Zamovlennya.Visibility = Visibility.Visible;
                    LoadOrders();
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

        }

        private void Reg_Click(object sender, RoutedEventArgs e)
        {
            string email = Email.Text;
            string password = Password.Password;
            string numbers = Numbe.Text;
            bool valid = true;

            string connectionString = "Host=localhost;Port=5432;Database=Order;Username=postgres;Password=promomo999;";

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
            if (!Name_Fname.Text.All(c => char.IsLetter(c) || c == ' '))
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
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
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
                    }
                }
            }

        }
        private void LoadTovary()
        {
            string connectionString = "Host=localhost;Port=5432;Database=Order;Username=postgres;Password=promomo999;";

            using (var conn = new Npgsql.NpgsqlConnection(connectionString))
            {
                conn.Open();

                string sql = "SELECT * FROM \"Tovar\"";

                Npgsql.NpgsqlDataAdapter da = new Npgsql.NpgsqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                TovarGrid.ItemsSource = dt.DefaultView;
            }
        }
        private void selectAmount(object sender, SelectionChangedEventArgs e)
        {
            if (TovarGrid.SelectedItem == null)
                return;

            DataRowView row = TovarGrid.SelectedItem as DataRowView;

            if (row == null)
                return;

            AmountCombo.Items.Clear();

            int maxQuantity = Convert.ToInt32(row["Кількість"]);

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
                    return;
                }
                string connectionString = "Host=localhost;Port=5432;Database=Order;Username=postgres;Password=promomo999;";

                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT \"Pass\", \"Name_Firstname\" FROM \"Login\" WHERE email=@email";

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
            int currentQty;
            int currentpri;
            int addanm;
            if (!int.TryParse(AmountCombo.SelectedItem?.ToString(), out addanm))
            {
                MessageBox.Show("Некоректна кількість");
                return;
            }
            if (AmountCombo.SelectedItem != null && TovarGrid.SelectedItem != null)
            {

                string selectedName = ((DataRowView)TovarGrid.SelectedItem)["Ім'я"].ToString();

                bool found = false;

                foreach (DataRow row in cart.Rows)
                {
                    if (row["Назва"].ToString() == selectedName)
                    {

                        int.TryParse(row["Ціна"]?.ToString(), out currentpri);


                        if (!int.TryParse(row["Кількість"]?.ToString(), out currentQty))
                        {
                            MessageBox.Show("Некоректна кількість у кошику");
                            return;
                        }


                        int totalQty = currentQty + addanm;
                        row["Кількість"] = totalQty;
                        int summ = currentpri * totalQty;
                        row["Сума"] = summ;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    int.TryParse(((DataRowView)TovarGrid.SelectedItem)["Ціна"]?.ToString(), out int price);

                    DataRow newRow = cart.NewRow();
                    newRow["Назва"] = selectedName;
                    newRow["Ціна"] = ((DataRowView)TovarGrid.SelectedItem)["Ціна"];
                    newRow["Кількість"] = addanm;
                    newRow["Сума"] = price * addanm;


                    cart.Rows.Add(newRow);
                }
            }
        }

        private void AddOrders_Click(object sender, RoutedEventArgs e)
        {
            if (cart.Rows.Count == 0)
            {
                MessageBox.Show("Кошик порожній");
                return;
            }

            if (!loggg)
            {
                MessageBox.Show("Спочатку увійдіть в акаунт");
                return;
            }

            string connectionString = "Host=localhost;Port=5432;Database=Order;Username=postgres;Password=promomo999;";
            bool chek = true;

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();


                foreach (DataRow row in cart.Rows)
                {
                    string name = row["Назва"].ToString();
                    int.TryParse(row["Кількість"]?.ToString(), out int amount);

                    string sql = "SELECT \"Кількість\" FROM \"Tovar\" WHERE \"Ім'я\" = @name";
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", name);
                    object result = cmd.ExecuteScalar();

                    if (result == null)
                    {
                        MessageBox.Show($"Товар {name} не знайдено в базі");
                        chek = false;
                        continue;
                    }

                    int.TryParse(result.ToString(), out int dbamount);
                    if (dbamount < amount)
                    {
                        MessageBox.Show($"Недостатньо товару: {name}. На складі {dbamount}, потрібно {amount}");
                        chek = false;
                    }
                }

                if (!chek)
                {
                    MessageBox.Show("Замовлення неможливо оформити");
                    return;
                }

                int totalSum = 0;
                foreach (DataRow row in cart.Rows)
                {
                    int.TryParse(row["Сума"]?.ToString(), out int s);
                    totalSum += s;
                }


                string insertOrder = "INSERT INTO \"Order\" (\"Email\", \"Дата\", \"Сума\") VALUES (@emal, @date, @sum)";
                using (NpgsqlCommand cmd = new NpgsqlCommand(insertOrder, conn))
                {
                    cmd.Parameters.AddWithValue("@emal", currentemail);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@sum", totalSum);
                    cmd.ExecuteNonQuery();
                }


                foreach (DataRow row in cart.Rows)
                {
                    string name = row["Назва"].ToString();
                    int.TryParse(row["Кількість"]?.ToString(), out int amount);

                    string updateSql = "UPDATE \"Tovar\" SET \"Кількість\" = \"Кількість\" - @amount WHERE \"Ім'я\" = @name";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(updateSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@amount", amount);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.ExecuteNonQuery();
                    }
                }


                cart.Clear();
                MessageBox.Show("Замовлення оформлено успішно!");
            }
        }

        private void LoadOrders()
        {
            string connectionString = "Host=localhost;Port=5432;Database=Order;Username=postgres;Password=promomo999;";

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                string sql;

                if (isAdmin)
                {
                    sql = "SELECT * FROM \"Order\"";
                }
                else
                {
                    sql = "SELECT * FROM \"Order\" WHERE \"Email\" = @email";
                }

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);

                if (!isAdmin)
                    da.SelectCommand.Parameters.AddWithValue("@email", currentemail);

                DataTable dt = new DataTable();
                da.Fill(dt);
                OrdersList.ItemsSource = dt.DefaultView;
            }
        }

        private void exitAcc_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (TovarGrid.SelectedItem is not DataRowView selectedUser)
            {
                MessageBox.Show("Оберіть користувача для видалення.",
                    "Увага", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string connectionString = "Host=localhost;Port=5432;Database=Order;Username=postgres;Password=promomo999;";
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                string sql = "DELETE FROM \"Tovar\" WHERE \"Ім'я\" = @name";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    string name = selectedUser["Ім'я"].ToString();
                    cmd.Parameters.AddWithValue("@name",name);
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                      
                        MessageBox.Show($"Товар «{name}» видалено.", "Готово",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            

                    }
                }
    }
}
