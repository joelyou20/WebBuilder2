using Microsoft.Extensions.Options;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Server.Options;
using WebBuilder2.Shared.Models;
using Google;
using WebBuilder2.Server.Services.Wrappers.Contracts;
using Google.Apis.Adsense.v2.Data;
using Google.Apis.Adsense.v2;

namespace WebBuilder2.Server.Services;

public class GoogleAdSenseService(IAdsenseServiceWrapper adsenseService) : IGoogleAdSenseService
{
    private readonly IAdsenseServiceWrapper _adsenseService = adsenseService;

    public async Task<GoogleAdSenseAccount> GetSingleAccountByNameAsync(string name)
    {
        Account account = await _adsenseService.GetSingleAccountAsync(name);

        if (account == null) throw new GoogleApiException("GoogleAdSenseService", $"Failed to get account with name: {name}.");

        var result = new GoogleAdSenseAccount
        {
            Name = account.Name,
            DisplayName = account.DisplayName,
            State = account.State,
        };

        return result;
    }

    public async Task<IEnumerable<GoogleAdSenseAccount>> GetAccountsAsync()
    {
        ListAccountsResponse response = await _adsenseService.GetAccountsAsync();

        if (response == null) throw new GoogleApiException("GoogleAdSenseService", "Failed to get accounts.");

        IEnumerable<GoogleAdSenseAccount> googleAdsenseAccounts = response.Accounts.Select(account => new GoogleAdSenseAccount
        {
            Name = account.Name,
            DisplayName = account.DisplayName,
            State = account.State,
        });

        return googleAdsenseAccounts;
    }

    public async Task<IEnumerable<GooglePayment>> GetPaymentsAsync()
    {
        ListPaymentsResponse response = await _adsenseService.GetPaymentsAsync();

        if (response == null) throw new GoogleApiException("GoogleAdSenseService", "Failed to get payments.");
        
        var result = response.Payments.Select(payment => new GooglePayment
        {
            Name = payment.Name,
            Date = payment.Date != null ? new DateTime(
                payment.Date.Year ?? DateTime.MinValue.Year,
                payment.Date.Month ?? DateTime.MinValue.Month,
                payment.Date.Day ?? DateTime.MinValue.Day) : null,
            Amount = payment.Amount,
        });

        return result;
    }

    public async Task<IEnumerable<GoogleAdClient>> GetClientsAsync()
    {
        ListAdClientsResponse response = await _adsenseService.GetClientsAsync();

        if (response == null) throw new GoogleApiException("GoogleAdSenseService", "Failed to get clients.");

        var result = response.AdClients.Select(client => new GoogleAdClient
        {
            Name = client.Name,
            ProductCode = client.ProductCode,
            State = client.State
        });

        return result;
    }
}
