using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OOPWPFProject
{
    internal class Payment
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=Order;Username=postgres;Password=promomo999;";
        public int idPayment { get; set; }
        public string typePayment { get; set; }
        public decimal amountPayment { get; set; }
        public string statusPayment { get; set; }
        
        public void addPayment()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                string query =
"INSERT INTO \"Payment\" (\"typePayment\", \"amountPayment\", \"statusPayment\") " +
"VALUES (@typePayment, @amountPayment, @statusPayment)";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@typePayment", typePayment);
                cmd.Parameters.AddWithValue("@amountPayment", amountPayment);
                cmd.Parameters.AddWithValue("@statusPayment", statusPayment);

                cmd.ExecuteNonQuery();
            }
        }
        public DataTable loadPayment()
        {
            using (var conn = new Npgsql.NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                string sql = "SELECT * FROM \"Payment\"";

                Npgsql.NpgsqlDataAdapter da = new Npgsql.NpgsqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }
        public void editPayment()
        {
            using (var conn = new Npgsql.NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = "UPDATE \"Payment\" SET \"typePayment\" = @typePayment,\"amountPayment\" = @amountPayment, \"statusPayment\" = @statusPayment  WHERE \"idPayment\" = @idPayment";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@typePayment", typePayment);
                cmd.Parameters.AddWithValue("@amountPayment", amountPayment);
                cmd.Parameters.AddWithValue("@statusPayment", statusPayment);
                cmd.Parameters.AddWithValue("@idPayment", idPayment);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Платіж редаговано!");
            }
        }
        public void deletePayment() {
            using (var conn = new Npgsql.NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = "DELETE FROM \"Payment\" WHERE \"idPayment\" = @idPayment";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@idPayment", idPayment);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Платіж видалено!");
            }
        }
        public DataTable searchPayment()
        {
            using (var conn = new Npgsql.NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                string sql = "SELECT * FROM \"Payment\" WHERE \"idPayment\" = @idPayment";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@idPayment", idPayment);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        public void changeStatusPayment()
        {
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                string sql = "UPDATE \"Payment\" SET \"statusPayment\" = @statusPayment WHERE \"idPayment\" = @idPayment";

                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@statusPayment", statusPayment);
                cmd.Parameters.AddWithValue("@idPayment", idPayment);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Статус платежу змінено!");
            }
        }
    }
}
