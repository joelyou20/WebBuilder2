using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Observers.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Services;

public class GoogleService(IGoogleClient client) : IGoogleService
{
    private readonly IGoogleClient _client = client;

    public async Task<List<GoogleAdSenseAccount>?> GetAccountsAsync()
    {
        IEnumerable<GoogleAdSenseAccount>? result = await _client.GetAccountsAsync();
        return result?.ToList();
    }

    public async Task<List<GooglePayment>?> GetPaymentsAsync()
    {
        IEnumerable<GooglePayment>? result = await _client.GetPaymentsAsync();

        return result?.ToList();
    }

    public async Task<List<GoogleAdClient>?> GetAdClientsAsync()
    {
        IEnumerable<GoogleAdClient>? result = await _client.GetAdClientsAsync();

        return result?.ToList();
    }
}
