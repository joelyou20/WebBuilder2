using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Observers.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Services;

public class GoogleService : ServiceBase, IGoogleService
{
    private IGoogleClient _client;

    public GoogleService(IGoogleClient client, IErrorObserver errorObserver, ILogService logService) : base(errorObserver, logService) 
    {
        _client = client;
    }
    
    public async Task<List<GoogleAdSenseAccount>?> GetAccountsAsync()
    {
        IEnumerable<GoogleAdSenseAccount>? result = await ExecuteAsync(() => _client.GetAccountsAsync());
        return result?.ToList();
    }

    public async Task<List<GooglePayment>?> GetPaymentsAsync()
    {
        IEnumerable<GooglePayment>? result = await ExecuteAsync(_client.GetPaymentsAsync);

        return result?.ToList();
    }

    public async Task<List<GoogleAdClient>?> GetAdClientsAsync()
    {
        IEnumerable<GoogleAdClient>? result = await ExecuteAsync(_client.GetAdClientsAsync);

        return result?.ToList();
    }
}
