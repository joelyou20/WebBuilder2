using Google.Apis.Adsense.v2.Data;

namespace WebBuilder2.Server.Services.Wrappers.Contracts;

public interface IAdsenseServiceWrapper
{
    Task<ListAccountsResponse> GetAccountsAsync();
    Task<Account> GetSingleAccountAsync(string name);
    Task<ListPaymentsResponse> GetPaymentsAsync();
    Task<ListAdClientsResponse> GetClientsAsync();
}
