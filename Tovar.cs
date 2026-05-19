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
    public class Tovar
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=Order;Username=postgres;Password=promomo999;";
        public string nameTovar1 { get; set; }
        public int idProduct {  get; set; }
        public string photoTovar1 { get; set; }
        public int amountTovar1 { get; set; }
        public decimal priceTovar1 { get; set; }
        public string categoriTovar1 { get; set; }
        public void addproduct()
        {

            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                string query =
"INSERT INTO \"Tovar\" (\"NameProduct\", \"Categori\", \"Price\", \"Amount\", \"PhotoProduct\") VALUES (@NameProduct, @Categori, @Price, @Amount, @PhotoProduct)";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@NameProduct", nameTovar1);
                cmd.Parameters.AddWithValue("@Categori", categoriTovar1);
                cmd.Parameters.AddWithValue("@Price", priceTovar1);
                cmd.Parameters.AddWithValue("@Amount", amountTovar1);
                cmd.Parameters.AddWithValue("@PhotoProduct", photoTovar1);
               
                cmd.ExecuteNonQuery();

                MessageBox.Show("Товар додано!");

            }
        }
        public DataTable loadTovary()
        {
            using (var conn = new Npgsql.NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                string sql = "SELECT * FROM \"Tovar\"";

                Npgsql.NpgsqlDataAdapter da = new Npgsql.NpgsqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
            
        }
        public void deleteTovar()
        {
            using (var conn = new Npgsql.NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = "DELETE FROM \"Tovar\" WHERE \"IdTovar\" = @idTovar";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@idTovar", idProduct);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Товар видалено!");
            }
        }
        public void editTovar() {
            using (var conn = new Npgsql.NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = "UPDATE \"Tovar\" SET \"NameProduct\" = @nameProduct,\"Categori\" = @categori, \"Price\" = @price, \"Amount\" = @amountproduct, \"PhotoProduct\" = @photoproduct  WHERE \"IdTovar\" = @idTovar";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@NameProduct", nameTovar1);
                cmd.Parameters.AddWithValue("@categori", categoriTovar1);
                cmd.Parameters.AddWithValue("@price", priceTovar1);
                cmd.Parameters.AddWithValue("@amountproduct", amountTovar1);
                cmd.Parameters.AddWithValue("@photoproduct", photoTovar1);
                cmd.Parameters.AddWithValue("@idTovar", idProduct);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Товар редаговано!");
            }
        }

    }
}
