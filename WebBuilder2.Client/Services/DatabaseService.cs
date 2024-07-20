using Amazon.Runtime.Internal;
using WebBuilder2.Client.Clients;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Observers.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Services;

public class DatabaseService(IDatabaseClient databaseClient, IErrorObserver errorObserver, ILogService logService) : ServiceBase(errorObserver, logService), IDatabaseService 
{
    private IDatabaseClient _client = databaseClient;

    public async Task PostCreateDatabaseAsync(string databaseName)
    {
        await ExecuteAsync(() => _client.PostCreateDatabaseAsync(databaseName));
    }
}
