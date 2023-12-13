using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Services.Contracts;

public interface IGoogleService
{
    Task<List<GoogleAdSenseAccount>?> GetAccountsAsync();
    Task<List<GooglePayment>?> GetPaymentsAsync();
    Task<List<GoogleAdClient>?> GetAdClientsAsync();
}
