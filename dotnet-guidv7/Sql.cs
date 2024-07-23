using Dapper;
using Microsoft.Data.SqlClient;

namespace dotnet_guidv7
{
    public static class Sql
    {
        private static readonly string connectionString
            = "Server=localhost;Database=SampleDB;User Id=sa;Password=123456;TrustServerCertificate=true;";

        public static void Query(string sql, object? param = default)
        {
            using var connection = GetConnection();
            connection.Execute(sql, param);
        }

        public static IEnumerable<T> Query<T>(string sql, object? param = default)
        {
            using var connection = GetConnection();
            return connection.Query<T>(sql, param);
        }

        private static SqlConnection GetConnection()
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            // Console.WriteLine("Connected to SQL Server");
            return connection;
        }
    }
    public class FragResult
    {
        public decimal avg_fragmentation_in_percent { get; set; }
        public string table_name { get; set; }
        public string index_name { get; set; }
    }
}
