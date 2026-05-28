using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OOPWPFProject
{


    public  class Courier 
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=Order;Username=postgres;Password=promomo999;";

        public int ageCourier { get; set; }
        public string numberCourier { get; set; }
        public string nameEmployee { get; set; }
        public string GenderCourier { get; set; }
        public string photoCourier { get; set; }
        public string cityCourier { get; set; }
        public string codeLogin { get; set; }
        public bool IsageCourier()
        {
            return ageCourier >= 17; 
        }
        
        public string GetinfoCourier()
        {
            return $"{ageCourier}, {numberCourier}, {GenderCourier}, {photoCourier}";
        }
       
        
        public string Getfullinfo()
        {
            return $"Ім'я: {nameEmployee}, Вік: {ageCourier}, Стать: {GenderCourier}, Номер: {numberCourier}, Місто: {cityCourier}";

        }
        public DataTable searchCode()
        {
            using (var conn = new Npgsql.NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                string sql = "SELECT * FROM \"Courier\" WHERE \"codeLogin\" = @codeLogin";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@codeLogin", codeLogin);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        public DataTable loadCourier()
        {
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                string sql = "SELECT * FROM \"Courier\"";

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);

                DataTable dt = new DataTable();

                da.Fill(dt);

                return dt;
            }
        }
 public string GetRandomCourier(string city)
        {
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                string sql =
                "SELECT \"ElementName\" " +
                "FROM \"Courier\" " +
                "WHERE \"cityCourier\" = @city " +
                "ORDER BY RANDOM() " +
                "LIMIT 1";

                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@city", city);

                object result = cmd.ExecuteScalar();

                return result?.ToString();
            }
        }
        public bool addCourier()
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    string query =
                    "INSERT INTO \"Courier\" " +
                    "(\"ElementName\", \"ageCourier\", \"GenderCourier\", \"cityCourier\", \"photoCourier\", \"numberCourier\", \"codeLogin\") " +
                    "VALUES (@fnameandname, @ageCourier, @genderCourier, @cityCour, @photo, @number, @CodeCourier)";

                    NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@fnameandname", nameEmployee);
                    cmd.Parameters.AddWithValue("@ageCourier", ageCourier);
                    cmd.Parameters.AddWithValue("@genderCourier", GenderCourier);
                    cmd.Parameters.AddWithValue("@CodeCourier", codeLogin);
                    cmd.Parameters.AddWithValue("@cityCour", cityCourier);
                    cmd.Parameters.AddWithValue("@photo", photoCourier);
                    cmd.Parameters.AddWithValue("@number", numberCourier);

                    cmd.ExecuteNonQuery();

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
