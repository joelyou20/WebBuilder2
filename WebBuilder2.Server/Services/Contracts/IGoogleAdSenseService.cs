using Google.Apis.Adsense.v2;
using Google.Apis.Adsense.v2.Data;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services.Contracts;

public interface IGoogleAdSenseService
{
    Task<IEnumerable<GoogleAdSenseAccount>> GetAccountsAsync(string? name = null);
    Task<IEnumerable<GooglePayment>> GetPaymentsAsync();
    Task<IEnumerable<GoogleAdClient>> GetClientsAsync();
}
