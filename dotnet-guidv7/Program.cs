// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
// connect on local sql server
string connectionString = "Server=localhost;Database=master;User Id=sa;Password=3uuiCaKxfbForrK;";
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    Console.WriteLine("Connected to SQL Server");

    // Create a sample database
    String sql = "CREATE DATABASE SampleDB";
    using (SqlCommand command = new SqlCommand(sql, connection))
    {
        command.ExecuteNonQuery();
        Console.WriteLine("Created Database");
    }
}