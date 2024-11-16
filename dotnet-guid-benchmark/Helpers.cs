using Dapper;
using Microsoft.Data.SqlClient;

namespace dotnet_guid_benchmark
{
    public static class Sql
    {
        private static readonly string connectionString
            = "Server=localhost;Database=SampleDB;User Id=sa;Password=123456!@#Qq;TrustServerCertificate=true;";

        public static async void Task<Query>(string sql, object? param = default)
        {
            using var connection = await GetConnection();
            await connection.ExecuteAsync(sql, param);
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = default)
        {
            using var connection = await GetConnection();
            return await connection.QueryAsync<T>(sql, param);
        }

        public static async Task QueryAsync(string sql, object? param = default)
        {
            using var connection = await GetConnection();
            await connection.QueryAsync(sql, param);
        }

        private static async Task<SqlConnection> GetConnection()
        {
            var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
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
