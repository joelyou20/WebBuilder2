using Google.Apis.Adsense.v2;
using Google.Apis.Adsense.v2.Data;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services.Contracts;

public interface IGoogleAdSenseService
{
    Task<ValidationResponse<GoogleAdSenseAccount>> GetAccountsAsync(string? name = null);
    Task<ValidationResponse<GooglePayment>> GetPaymentsAsync();
    Task<ValidationResponse<GoogleAdClient>> GetClientsAsync();
}
