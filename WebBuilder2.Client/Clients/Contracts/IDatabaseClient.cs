using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients.Contracts;

public interface IDatabaseClient
{
    Task<ValidationResponse> PostCreateDatabaseAsync(string databaseName);
}
