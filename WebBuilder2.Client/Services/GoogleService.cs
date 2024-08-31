using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Observers.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Services;

public class GoogleService(IGoogleClient client) : IGoogleService
{
    private readonly IGoogleClient _client = client;

    public async Task<GoogleAdSenseAccount> GetSingleAccountByNameAsync(string name)
    {
        GoogleAdSenseAccount result = await _client.GetSingleAccountByNameAsync(name);
        return result;
    }
    public async Task<IEnumerable<GoogleAdSenseAccount>> GetAccountsAsync()
    {
        IEnumerable<GoogleAdSenseAccount> result = await _client.GetAccountsAsync();
        return result;
    }

    public async Task<IEnumerable<GooglePayment>> GetPaymentsAsync()
    {
        IEnumerable<GooglePayment> result = await _client.GetPaymentsAsync();

        return result;
    }

    public async Task<IEnumerable<GoogleAdClient>> GetAdClientsAsync()
    {
        IEnumerable<GoogleAdClient> result = await _client.GetAdClientsAsync();

        return result;
    }
}
