using Npgsql;
using OOPWPFProject.Class;
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
    internal class Delivery : IDbEntity
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=Order;Username=postgres;Password=promomo999;";
        public int idDelivery { get; set; }
        public string cityDelivery { get; set; }
        public string addressDelivery { get; set; }
        public string statusDelivery { get; set; } = "Очікується підтвердження";
        public void Add() => addDelivery();
        public void Delete() { } 
        public DataTable Load() { return null; } 
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
      
        
    

    }

}
