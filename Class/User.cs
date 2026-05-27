using GMap.NET.MapProviders;
using Npgsql;
using OOPWPFProject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;



namespace OOPWPFProject
{
    public abstract class User
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=Order;Username=postgres;Password=promomo999;";
        public string email { get; set; }
        public string Number { get; set; }
        public string Password { get; set; }
        public string Name_Firstname { get; set; }
        public bool Register()
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

                    return false;
                }
                else
                {


                    string query = "INSERT INTO \"Login\" (email, \"Number\", \"Name_Firstname\", \"Pass\") VALUES (@email, @number, @name, @pass)";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@number", Number);
                    cmd.Parameters.AddWithValue("@name", Name_Firstname);
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);
                    cmd.Parameters.AddWithValue("@pass", hashedPassword); cmd.ExecuteNonQuery();
                    return true;

                }

            }

        }
        public bool Login()
        {
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
                        return false;
                    }

                    string hash = reader["Pass"].ToString();

                    bool check = BCrypt.Net.BCrypt.Verify(Password, hash);

                    if (check)
                    {
                        Name_Firstname = reader["Name_Firstname"].ToString();
                        Number = reader["Number"].ToString();
                    }

                    return check;
                }
            }
        }
        public bool IsValidEmail()
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            return Regex.IsMatch(email, pattern);
        }
        public bool IsValidPassword()
        {
            return !string.IsNullOrWhiteSpace(Password) && Password.Length >= 6;
        }
        public bool IsValidNumber()
        {
            return Number.All(c => char.IsDigit(c) || c == '+');
        }


        public abstract DataTable ShowOrders(Order order);





    }
    public class RegularUser : User
    {
        public override DataTable ShowOrders(Order order)
        {
            return order.loadOrderUser();
        }
    }

    public class Admin : User
    {
        public override DataTable ShowOrders(Order order)
        {
            return order.loadOrderAdmin();
        }
        public bool IsAdminCredentials(string email, string password)
        {
            return email == "dtimasik@gmail.com" && password == "promomo999";
        }
    }
}
