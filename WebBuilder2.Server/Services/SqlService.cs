using System.Data.SqlClient;
using System.Drawing;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services;

public class SqlService : ISqlService
{
    public async Task<ValidationResponse> CreateDatabaseAsync(string databaseName, string connectionStringName = "default")
    {
        string? connectionString = GetConnectionString(connectionStringName) ?? throw new Exception("Connection string not found.");
        string sqlCreateDBQuery = $"CREATE DATABASE {databaseName}";

        using SqlConnection connection = new(connectionString);
        SqlCommand command = new(sqlCreateDBQuery, connection);

        connection.Open();
        int result = await command.ExecuteNonQueryAsync();
        return result >= 0 ? ValidationResponse.Success() : ValidationResponse.Failure();
    }

    public async Task GetDatabaseListAsync(string serverName, string databaseName, string connectionStringName)
    {
        string? connectionString = GetConnectionString(connectionStringName) ?? throw new Exception("Connection string not found.");
        using SqlConnection connection = new(connectionString);
        connection.Open();

        SqlCommand command = new("SELECT name from sys.databases", connection);

        using SqlDataReader dr = await command.ExecuteReaderAsync();

    }

    private string? GetConnectionString(string connectionStringName)
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())  // Path to where appsettings.json is located
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Example of retrieving a setting from appsettings.json
        string? connectionString = config.GetConnectionString(connectionStringName);

        return connectionString;
    }
}
