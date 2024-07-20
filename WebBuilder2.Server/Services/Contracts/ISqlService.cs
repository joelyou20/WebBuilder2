using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services.Contracts;

public interface ISqlService
{
    Task<ValidationResponse> CreateDatabaseAsync(string databaseName, string connectionStringName = "default");
    Task GetDatabaseListAsync(string serverName, string databaseName, string connectionStringName);
}
