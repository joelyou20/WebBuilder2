using Newtonsoft.Json;
using System.Text;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Observers;
using WebBuilder2.Client.Observers.Contracts;
using WebBuilder2.Client.Services;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients;

public class GoogleClient : ClientBase, IGoogleClient
{
    public GoogleClient(HttpClient httpClient) : base(httpClient, "google") { }

    public async Task<IEnumerable<GoogleAdSenseAccount>> GetAccountsAsync() => await GetAsync<IEnumerable<GoogleAdSenseAccount>>("accounts");
    public async Task<IEnumerable<GooglePayment>> GetPaymentsAsync() => await GetAsync<IEnumerable<GooglePayment>>("payments");
    public async Task<IEnumerable<GoogleAdClient>> GetAdClientsAsync() => await GetAsync<IEnumerable<GoogleAdClient>>("adclients");
}
