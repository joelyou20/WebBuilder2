using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Services.Contracts;

public interface IDatabaseService
{
    Task PostCreateDatabaseAsync(string databaseName);
}
