    using Npgsql;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Data;
using OOPWPFProject.Class;

namespace OOPWPFProject
    {
        public class Order : IDbEntity
        {
            Courier couriers = new Courier();
            private const string ConnectionString = "Host=localhost;Port=5432;Database=Order;Username=postgres;Password=promomo999;";
            public int idOrder { get; set; }
            public string nameProduct { get; set; }
            public decimal totalPrice { get; set; }
            public int amountProduct { get; set; }
            public DateTime dateOrder { get; set; } = DateTime.Today;
            public string clientName { get; set; }
            public string addressOrder { get; set; }
            public string cityOrder { get; set; }
            public string numberClient { get; set; }
            public string statusOrder { get; set; } = "Очікується підтвердження";
            public string commentOrder { get; set; } = "";
            public string courierName { get; set; }
        public void Add() => addorderAdmin();
        public void Delete() => deleteorder();
        public DataTable Load() => loadOrderAdmin();

        public void addorderAdmin()
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    courierName = couriers.GetRandomCourier(cityOrder);
                    string query =
    "INSERT INTO \"OrderAdmin\" (\"nameProduct\", \"totalPrice\", \"amountProduct\", \"dateOrder\", \"clientName\",\"addressOrder\", \"numberClient\", \"statusOrder\", \"commentOrder\",\"CourierName\") " +
    "VALUES (@nameProduct, @totalPrice, @amountProduct, @dateOrder, " +
    "@clientName, @addressOrder, @numberClient, @statusOrder, @commentOrder, @courier)";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nameProduct", nameProduct);
                    cmd.Parameters.AddWithValue("@totalPrice", totalPrice);
                    cmd.Parameters.AddWithValue("@amountProduct", amountProduct);
                    cmd.Parameters.AddWithValue("@dateOrder", dateOrder);
                    cmd.Parameters.AddWithValue("@clientName", clientName);
                    cmd.Parameters.AddWithValue("@addressOrder", addressOrder);
                    cmd.Parameters.AddWithValue("@numberClient", numberClient);
                    cmd.Parameters.AddWithValue("@statusOrder", statusOrder);
                    cmd.Parameters.AddWithValue("@commentOrder", commentOrder);
                    cmd.Parameters.AddWithValue("@courier", courierName);
                    cmd.ExecuteNonQuery();

                }
            }
        
            public DataTable loadOrderUser()
            {
                using (var conn = new Npgsql.NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    string sql = @"SELECT   ""idOrder"", ""nameProduct"", ""totalPrice"", ""amountProduct"", ""dateOrder"", ""addressOrder"", ""statusOrder""  FROM ""OrderAdmin"" WHERE ""clientName"" = @clientName";
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@clientName", clientName);
    
                    Npgsql.NpgsqlDataAdapter da = new Npgsql.NpgsqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    return dt;
                }
            }
            public DataTable loadOrderCourier()
            {
                using (var conn = new Npgsql.NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    string sql = @"SELECT ""idOrder"", ""nameProduct"", ""totalPrice"", ""amountProduct"", ""clientName"", ""addressOrder"", ""numberClient"", ""statusOrder"" FROM ""OrderAdmin""";
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
    
                    cmd.Parameters.AddWithValue("@statusOrder", statusOrder);
                    Npgsql.NpgsqlDataAdapter da = new Npgsql.NpgsqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    return dt;
                }
            }
            public DataTable loadOrderAdmin()
            {
                using (var conn = new Npgsql.NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    string sql = "SELECT * FROM \"OrderAdmin\"";

                    Npgsql.NpgsqlDataAdapter da = new Npgsql.NpgsqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    return dt;
                }
            }
            public void deleteorder()
            {
                using (var conn = new Npgsql.NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    string sql = "DELETE FROM \"OrderAdmin\" WHERE \"idOrder\" = @idOrder";
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                    cmd = new NpgsqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@idOrder", idOrder);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Замовлення видалено!");
                }
            }
           
            public DataTable searchOrder()
            {
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                string sql = "SELECT * FROM \"OrderAdmin\" WHERE \"clientName\" = @clientName AND LOWER(\"nameProduct\") LIKE LOWER(@nameProduct)";

                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@clientName", clientName);
                cmd.Parameters.AddWithValue("@nameProduct", "%" + nameProduct + "%");

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                da.Fill(dt);

                return dt;
            }
        }
      

            public void changeStatus()
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    string sql = "UPDATE \"OrderAdmin\" SET \"statusOrder\" = @status WHERE \"idOrder\" = @idOrder";

                    NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@status", statusOrder);
                    cmd.Parameters.AddWithValue("@idOrder", idOrder);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Статус змінено!");
                
                }
            }

            public void CreateOrder(
         string clientName,
         string numberPhone,
         string city,
         decimal price,
         int amount
      
     )
            {
                this.clientName = clientName;
                this.numberClient = numberPhone;

                this.cityOrder = city;

                this.amountProduct = amount;
                this.totalPrice = price * amount;

                this.dateOrder = DateTime.Today;

                addorderAdmin();
            }
        }
    }

