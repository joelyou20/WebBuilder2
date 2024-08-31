using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Services.Contracts;

public interface IGoogleService
{
    Task<GoogleAdSenseAccount> GetSingleAccountByNameAsync(string name);
    Task<IEnumerable<GoogleAdSenseAccount>> GetAccountsAsync();
    Task<IEnumerable<GooglePayment>> GetPaymentsAsync();
    Task<IEnumerable<GoogleAdClient>> GetAdClientsAsync();
}
