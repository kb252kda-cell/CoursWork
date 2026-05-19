using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPWPFProject
{

    public class Employee
    {
        public string nameEmployee { get; set; }
        public decimal salaryEmployee { get; set; }
    }
    internal class courier : Employee
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=Order;Username=postgres;Password=promomo999;";

        public int ageCourier { get; set; }
        public int numberCourier { get; set; }
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


    }
 
}
