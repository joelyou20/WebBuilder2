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

public class GoogleClient : IGoogleClient
{
    private HttpClient _httpClient;
    private IErrorObserver _errorObserver;
    private ILogService _logService;

    public GoogleClient(HttpClient httpClient, IErrorObserver errorObserver, ILogService logService)
    {
        _httpClient = httpClient;
        _errorObserver = errorObserver;
        _logService = logService;
    }

    public async Task<ValidationResponse<GoogleAdSenseAccount>> GetAccountsAsync(string? name = null)
    {
        StringBuilder sb = new();
        sb.Append($"{_httpClient.BaseAddress}google/accounts");
        if(name == null)
        {
            sb.Append($"/{name}");
        }
        HttpResponseMessage response = await _httpClient.GetAsync(sb.ToString());
        if (!response.IsSuccessStatusCode)
        {
            // Handle error
        }

        var message = await response.Content.ReadAsStringAsync();
        var result = ValidationResponse<GoogleAdSenseAccount>.ToResult(message);
        return result;
    }

    public async Task<ValidationResponse<GooglePayment>> GetPaymentsAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}google/payments");
        if (!response.IsSuccessStatusCode)
        {
            // Handle error
        }

        var message = await response.Content.ReadAsStringAsync();
        var result = ValidationResponse<GooglePayment>.ToResult(message);
        return result;
    }

    public async Task<ValidationResponse<GoogleAdClient>> GetAdClientsAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}google/adclients");
        if (!response.IsSuccessStatusCode)
        {
            // Handle error
        }

        var message = await response.Content.ReadAsStringAsync();
        var result = ValidationResponse<GoogleAdClient>.ToResult(message);
        return result;
    }

    private void OnError(object? sender, Newtonsoft.Json.Serialization.ErrorEventArgs e)
    {
        _errorObserver.AddError(e.ErrorContext.Error);
        _logService.AddLogAsync(e.ErrorContext.Error);
    }
}
