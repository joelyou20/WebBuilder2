using Google.Apis.Adsense.v2;
using Google.Apis.Adsense.v2.Data;
using Microsoft.Extensions.Options;
using WebBuilder2.Server.Options;
using WebBuilder2.Server.Services.Wrappers.Contracts;

namespace WebBuilder2.Server.Services.Wrappers;

// The google api is not unit test friendly. This wrapper only exists to allow for
//  unit testing.
public class AdsenseServiceWrapper(AdsenseService adSenseService, IOptions<GoogleOptions> googleSettings) : IAdsenseServiceWrapper
{
    private readonly AdsenseService _adSenseService = adSenseService;
    private readonly GoogleOptions _googleSettings = googleSettings.Value;

    public async Task<ListAccountsResponse> GetAccountsAsync()
    {
        AccountsResource.ListRequest accountsListRequest = _adSenseService.Accounts.List();
        return await accountsListRequest.ExecuteAsync();
    }

    public async Task<Account> GetSingleAccountAsync(string name)
    {
        AccountsResource.GetRequest accountGetRequest = _adSenseService.Accounts.Get(name);
        return await accountGetRequest.ExecuteAsync();
    }

    public async Task<ListPaymentsResponse> GetPaymentsAsync()
    {
        AccountsResource.PaymentsResource.ListRequest payments = _adSenseService.Accounts.Payments.List(_googleSettings.AdsenseAccountId);
        return await payments.ExecuteAsync();
    }

    public async Task<ListAdClientsResponse> GetClientsAsync()
    {
        AccountsResource.AdclientsResource.ListRequest clients = _adSenseService.Accounts.Adclients.List(_googleSettings.AdsenseAccountId);
        return await clients.ExecuteAsync();
    }
}
