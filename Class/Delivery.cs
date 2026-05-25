using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static GMap.NET.Entity.OpenStreetMapGeocodeEntity;
namespace OOPWPFProject
{
    internal class Delivery
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=Order;Username=postgres;Password=promomo999;";
        public int idDelivery { get; set; }
        public string cityDelivery { get; set; }
        public string addressDelivery { get; set; }
        public string statusDelivery { get; set; } = "Очікується підтвердження";
        public void addDelivery()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                string query =
"INSERT INTO \"Delivery\" (\"cityDelivery\", \"addressDelivery\", \"statusDelivery\") " +
"VALUES (@cityDelivery, @addressDelivery, @statusDelivery)";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@cityDelivery", cityDelivery);
                cmd.Parameters.AddWithValue("@addressDelivery", addressDelivery);
                cmd.Parameters.AddWithValue("@statusDelivery", statusDelivery);

                cmd.ExecuteNonQuery();
            }


        }
        public void CreateDelivery(string city, string address)
        {
            this.addressDelivery = address;
           this.cityDelivery = city;

            addDelivery();
        }
        public DataTable loadDelivery()
        {
            using (var conn = new Npgsql.NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                string sql = "SELECT * FROM \"Delivery\"";

                Npgsql.NpgsqlDataAdapter da = new Npgsql.NpgsqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }
        public void editDelivery()
        {
            using (var conn = new Npgsql.NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = "UPDATE \"Delivery\" SET \"cityDelivery\" = @cityDelivery,\"addressDelivery\" = @addressDelivery, \"statusDelivery\" = @statusDelivery  WHERE \"idDelivery\" = @idDelivery";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@cityDelivery", cityDelivery);
                cmd.Parameters.AddWithValue("@addressDelivery", addressDelivery);
                cmd.Parameters.AddWithValue("@statusDelivery", statusDelivery);
                cmd.Parameters.AddWithValue("@idDelivery", idDelivery);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Замовлення редаговано!");
            }
        }
        public void deleteDelivery()
        {
            using (var conn = new Npgsql.NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = "DELETE FROM \"Delivery\" WHERE \"idDelivery\" = @idDelivery";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@idDelivery", idDelivery);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Замовлення видалено!");
            }
        }
        public DataTable searchDelivery()
        {
            using (var conn = new Npgsql.NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                string sql = "SELECT * FROM \"Delivery\" WHERE \"idDelivery\" = @idDelivery";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@idDelivery", idDelivery);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        public void changeStatusDelivery(){
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                string sql = "UPDATE \"Delivery\" SET \"statusDelivery\" = @status WHERE \"idDelivery\" = @idDelivery";

                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@status", statusDelivery);
                cmd.Parameters.AddWithValue("@idDelivery", idDelivery);

                cmd.ExecuteNonQuery();
            }
        }

    }

}
