using Npgsql;
using OOPWPFProject.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace OOPWPFProject
{
    public class Tovar : IDbEntity
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=Order;Username=postgres;Password=promomo999;";
        public string nameTovar1 { get; set; }
        public int idProduct { get; set; }
        public string photoTovar1 { get; set; }
        public int amountTovar1 { get; set; }
        public decimal priceTovar1 { get; set; }
        public string categoriTovar1 { get; set; }
        public void Add() => addproduct();
        public void Delete() => deleteTovar();
        public DataTable Load() => loadTovary();
        public void Update()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                string sql = "UPDATE \"Tovar\" SET \"Amount\" = \"Amount\" - @count WHERE \"NameProduct\" = @name";

                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@count", amountTovar1);
                cmd.Parameters.AddWithValue("@name", nameTovar1);

                cmd.ExecuteNonQuery();
            }
        }
        public void  addproduct()
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
            }
        }
        public void editTovar()
        {
            using (var conn = new Npgsql.NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = "UPDATE \"Tovar\" SET \"NameProduct\" = @nameProduct,\"Categori\" = @categori, \"Price\" = @price, \"Amount\" = @amountproduct, \"PhotoProduct\" = @photoproduct  WHERE \"IdTovar\" = @idTovar";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@nameProduct", nameTovar1);
                cmd.Parameters.AddWithValue("@categori", categoriTovar1);
                cmd.Parameters.AddWithValue("@price", priceTovar1);
                cmd.Parameters.AddWithValue("@amountproduct", amountTovar1);
                cmd.Parameters.AddWithValue("@photoproduct", photoTovar1);
                cmd.Parameters.AddWithValue("@idTovar", idProduct);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Товар редаговано!");
            }
        }
        public DataTable searchTovarname()
        {
            using (var conn = new Npgsql.NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                string sql = "SELECT * FROM \"Tovar\" WHERE \"NameProduct\" ILIKE  @nameTovar1";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@nameTovar1", "%" + nameTovar1 + "%");
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        public DataTable searchCategori()
        {
            using (var conn = new Npgsql.NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                string sql = "SELECT * FROM \"Tovar\" WHERE \"Categori\" = @categori";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@categori",  categoriTovar1);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

    }
}
