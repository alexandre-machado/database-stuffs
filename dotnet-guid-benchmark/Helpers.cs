using Dapper;
using Microsoft.Data.SqlClient;

namespace dotnet_guid_benchmark
{
    public static class Sql
    {
        private static readonly string connectionString
            = "Server=localhost;Database=SampleDB;User Id=sa;Password=123456!@#Qq;TrustServerCertificate=true;";

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
        public decimal AvgFragmentationInPercent { get; set; }
        public string TableName { get; set; }
        public string IndexName { get; set; }
    }
}
