using Google.Apis.Adsense.v2;
using Google.Apis.Adsense.v2.Data;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services.Contracts;

public interface IGoogleAdSenseService
{
    Task<GoogleAdSenseAccount> GetSingleAccountByNameAsync(string name);
    Task<IEnumerable<GoogleAdSenseAccount>> GetAccountsAsync();
    Task<IEnumerable<GooglePayment>> GetPaymentsAsync();
    Task<IEnumerable<GoogleAdClient>> GetClientsAsync();
}
