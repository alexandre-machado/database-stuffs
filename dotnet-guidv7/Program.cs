using Microsoft.Data.SqlClient;

Console.WriteLine("Hello, World!");

var connectionString = "Server=localhost;Database=master;User Id=sa;Password=3uuiCaKxfbForrK;TrustServerCertificate=true;";
using (var connection = new SqlConnection(connectionString))
{
    connection.Open();
    Console.WriteLine("Connected to SQL Server");
    // drop the database if it exists
    var sql = "DROP DATABASE IF EXISTS SampleDB";
    using var command = new SqlCommand(sql, connection);
    command.ExecuteNonQuery();
    Console.WriteLine("Dropped Database");

    // Create a sample database
    sql = "CREATE DATABASE SampleDB";
    command.CommandText = sql;
    command.ExecuteNonQuery();
    Console.WriteLine("Created Database");

    // create a table
    sql = @"
        USE SampleDB;
        CREATE TABLE Inventory (
            Id INT PRIMARY KEY IDENTITY,
            Name NVARCHAR(50),
            Quantity INT
        );
    ";
    command.CommandText = sql;
    command.ExecuteNonQuery();
    Console.WriteLine("Created Table");
}
// docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=3uuiCaKxfbForrK" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
