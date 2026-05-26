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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Collections.ObjectModel;
using OOPWPFProject.Class;

namespace OOPWPFProject
{ 
    public partial class DepositWindow : Window
    {
        DepositBalance deposit = new DepositBalance();
        public ObservableCollection<string> Cards { get; set; }
        public Dictionary<string, string> CvvCode { get; set; }

        public async Task<bool> AddDepositWithDelay(string number, decimal amount)
        {
            Random rnd = new Random();
            int delay = rnd.Next(3000, 10000);
            await Task.Delay(delay);

            decimal currentBalance = deposit.GetBalance(number);
            if (currentBalance >= 60)
                return false;

            deposit.numberDeposit = number;
            deposit.depositMoney = amount;
            deposit.balance = currentBalance + amount;
            return deposit.addDeposit();
        }
        public DepositWindow(string phoneNumber)
        {
            InitializeComponent();

            CvvCode = new Dictionary<string, string>
        {
            { "4766 5555 4334 3345", "123" },
            { "5245 3342 3424 5324", "456" },
            { "3432 4324 2442 4244", "789" },
            { "3344 4433 5555 5553", "321" },
            { "4233 4434 3323 3333", "654" },
            { "4432 4423 4424 2233", "987" },
            { "5123 4567 8901 2346", "111" },
            { "4244 3334 4434 3344", "222" },
            { "4422 3222 3322 4443", "333" },
            { "4111 1111 1111 1111", "444" },
            { "5344 5433 5543 4442", "555" },
            { "5245 4432 4444 1342", "666" },
            { "6544 4443 5553 5533", "777" },
            { "4533 3343 3234 3234", "888" },
            { "6543 5322 4435 3343", "999" },
            { "4343 2342 5532 1232", "112" },
            { "5333 5533 3344 3333", "223" },
            { "4453 3223 4533 5532", "334" },
            { "5333 4443 5553 3324", "445" },
            { "4342 4224 3242 4133", "556" },
            { "5564 3334 4444 3334", "667" },
            { "5245 3245 7737 2462", "778" },
            { "5533 5533 4434 3342", "889" },
            { "4454 3453 5543 3434", "990" },
            { "5534 5433 5533 4345", "101" },
        };
            Cards = new ObservableCollection<string>(CvvCode.Keys);

            DataContext = this;
        }

        private void ButtonCheak_Click(object sender, RoutedEventArgs e)
        {
            string inputCard = NumberCart.Text.Trim();
            string inputCvv = CvvBox.Text.Trim();

            var found = CvvCode.FirstOrDefault(x => x.Key.Replace(" ", "") == inputCard.Replace(" ", ""));

            if (found.Key == null)
            {
                MessageBox.Show("Карта не знайдена!");
                return;
            }

            if (found.Value != inputCvv)
            {
                MessageBox.Show(" Невірний CVV!");
                return;
            }

            MessageBox.Show(" Карта і CVV вірні!");
            StackDepodit.Visibility = Visibility.Visible;
            StackCheck.Visibility = Visibility.Collapsed;
            ButtonCheak.Visibility = Visibility.Collapsed;

        }

        private async void ButtonDeposit_Click(object sender, RoutedEventArgs e)
        {
            string inputNumber = Number.Text.Trim();

            if (!deposit.CheckNumberInLogin(inputNumber))
            {
                MessageBox.Show("Номер не знайдено в системі!");
                return;
            }

            if (!decimal.TryParse(DepositSum.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Введіть коректну суму!");
                return;
            }

            decimal currentBalance = deposit.GetBalance(inputNumber);
            if (currentBalance >= 60)
            {
                BalanceText.Text = $"Баланс: {currentBalance} грн";
                MessageBox.Show("Баланс вже достатній, поповнення не потрібне!");
                return;
            }

            ButtonDeposit.IsEnabled = false;
            BalanceText.Text = "Обробка платежу";

            bool success = await AddDepositWithDelay(inputNumber, amount);

            if (success)
            {
                decimal newBalance = deposit.GetBalance(inputNumber);
                BalanceText.Text = $"Баланс: {newBalance} грн";
                MessageBox.Show("Поповнення успішне!");
            }
            else
            {
                decimal newBalance = deposit.GetBalance(inputNumber);
                BalanceText.Text = $"Баланс: {newBalance} грн";
                MessageBox.Show("оповнення не виконано — баланс вже > 60 грн");
            }

            ButtonDeposit.IsEnabled = true;
        }
    
}
}