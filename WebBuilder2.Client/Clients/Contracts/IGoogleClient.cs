using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients.Contracts;

public interface IGoogleClient
{
    Task<ValidationResponse<GoogleAdSenseAccount>> GetAccountsAsync(string? name = null);
    Task<ValidationResponse<GooglePayment>> GetPaymentsAsync();
    Task<ValidationResponse<GoogleAdClient>> GetAdClientsAsync();
}
