using MySql.Data.MySqlClient;
using System;

namespace BrewAndBiteCafe
{
    /// <summary>
    /// Public class for MySQL database connectivity.
    /// All forms use this class to obtain a connection.
    /// </summary>
    public class DatabaseConnection
    {
        private const string Server   = "localhost";
        private const string Database = "sales_inventory_db";
        private const string User     = "root";
        private const string Password = "";         
        private const int    Port     = 3306;

        public static string ConnectionString =>
            $"Server={Server};Port={Port};Database={Database};Uid={User};Pwd={Password};CharSet=utf8mb4;";

        /// <summary>Returns an open MySqlConnection. Caller must dispose it.</summary>
        public static MySqlConnection GetConnection()
        {
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();
            return conn;
        }

        /// <summary>Tests connectivity. Returns true if successful.</summary>
        public static bool TestConnection()
        {
            try
            {
                using var conn = GetConnection();
                return conn.State == System.Data.ConnectionState.Open;
            }
            catch
            {
                return false;
            }
        }
    }
}
