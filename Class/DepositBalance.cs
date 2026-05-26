using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPWPFProject.Class
{
    public class DepositBalance
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=Order;Username=postgres;Password=promomo999;";
        public decimal balance {  get; set; }
        public string numberDeposit { get; set; }
        public decimal depositMoney { get; set; }
        public bool addDeposit()
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    string query =
                    "INSERT INTO \"Deposit\" " +
                    "(\"balance\", \"numberDeposit\", \"depositMoney\") " +
                    "VALUES (@balance, @numberDeposit, @depositMoney)";

                    NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@balance", balance);
                    cmd.Parameters.AddWithValue("@numberDeposit", numberDeposit);
                    cmd.Parameters.AddWithValue("@depositMoney", depositMoney);

                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public decimal GetBalance(string cardNumber)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    string query = "SELECT \"balance\" FROM \"Deposit\" WHERE \"numberDeposit\" = @number";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@number", cardNumber);

                    var result = cmd.ExecuteScalar();
                    return decimal.TryParse(result?.ToString(), out decimal balance) ? balance : 0;
                }
            }
            catch
            {
                return 0;
            }
        }
        public bool CheckNumberInLogin(string number)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM \"Login\" WHERE \"Number\" = @number";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@number", number);
                    long count = (long)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            catch
            {
                return false;
            }
        }
        public bool DeductBalance(string number, decimal amount)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    decimal currentBalance = GetBalance(number);

                    if (currentBalance < amount)
                        return false;
                    string query = "UPDATE \"Deposit\" SET \"balance\" = \"balance\" - @amount WHERE \"numberDeposit\" = @number";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@number", number);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public bool AddCourierPayment(string courierCode, decimal amount)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    string checkQuery = "SELECT COUNT(*) FROM \"Deposit\" WHERE \"numberDeposit\" = @number";
                    NpgsqlCommand checkCmd = new NpgsqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@number", courierCode);
                    long count = (long)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        string updateQuery = "UPDATE \"Deposit\" SET \"balance\" = \"balance\" + @amount WHERE \"numberDeposit\" = @number";
                        NpgsqlCommand updateCmd = new NpgsqlCommand(updateQuery, conn);
                        updateCmd.Parameters.AddWithValue("@amount", amount);
                        updateCmd.Parameters.AddWithValue("@number", courierCode);
                        updateCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        string insertQuery = "INSERT INTO \"Deposit\" (\"numberDeposit\", \"balance\", \"depositMoney\") VALUES (@number, @amount, @amount)";
                        NpgsqlCommand insertCmd = new NpgsqlCommand(insertQuery, conn);
                        insertCmd.Parameters.AddWithValue("@number", courierCode);
                        insertCmd.Parameters.AddWithValue("@amount", amount);
                        insertCmd.ExecuteNonQuery();
                    }
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
