using Google;
using Google.Apis.Adsense.v2;
using Google.Apis.Adsense.v2.Data;
using Google.Apis.Http;
using Google.Apis.Services;
using Microsoft.Extensions.Options;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Server.Settings;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services;

public class GoogleAdSenseService : IGoogleAdSenseService
{
    private AdsenseService _adSenseService;
    private GoogleSettings _googleSettings;

    public GoogleAdSenseService(AdsenseService adsenseService, IOptions<GoogleSettings> googleSettings)
    {
        _adSenseService = adsenseService;
        _googleSettings = googleSettings.Value;
    }

    public async Task<ValidationResponse<GoogleAdSenseAccount>> GetAccountsAsync(string? name = null)
    {
        try
        {
            List<GoogleAdSenseAccount> googleAdsenseAccounts = new();
            if (string.IsNullOrEmpty(name))
            {
                var accountsListRequest = _adSenseService.Accounts.List();
                var accounts = await accountsListRequest.ExecuteAsync();
                googleAdsenseAccounts.AddRange(accounts.Accounts.Select(account => new GoogleAdSenseAccount
                {
                    Name = account.Name,
                    DisplayName = account.DisplayName,
                    State = account.State,
                }));
            }
            else
            {
                var accountGetRequest = _adSenseService.Accounts.Get(name);
                var account = await accountGetRequest.ExecuteAsync();
                googleAdsenseAccounts.Add(new GoogleAdSenseAccount
                {
                    Name = account.Name,
                    DisplayName = account.DisplayName,
                    State = account.State,
                });
            }

            return ValidationResponse<GoogleAdSenseAccount>.Success(googleAdsenseAccounts);
        }
        catch (Exception ex)
        {
            return ValidationResponse<GoogleAdSenseAccount>.Failure(ex);
        }
    }

    public async Task<ValidationResponse<GooglePayment>> GetPaymentsAsync()
    {
        var features = _adSenseService.Features;
        var payments = _adSenseService.Accounts.Payments.List(_googleSettings.AdsenseAccountId);
        var response = await payments.ExecuteAsync();

        var result = response.Payments.Select(payment => new GooglePayment
        {
            Name = payment.Name,
            Date = payment.Date != null ? new DateTime(
                payment.Date.Year ?? DateTime.MinValue.Year,
                payment.Date.Month ?? DateTime.MinValue.Month,
                payment.Date.Day ?? DateTime.MinValue.Day) : null,
            Amount = payment.Amount,
        });

        return ValidationResponse<GooglePayment>.Success(result);
    }

    public async Task<ValidationResponse<GoogleAdClient>> GetClientsAsync()
    {
        try
        {
            var clients = _adSenseService.Accounts.Adclients.List(_googleSettings.AdsenseAccountId);
            var response = await clients.ExecuteAsync();

            var result = response.AdClients.Select(client => new GoogleAdClient
            {
                Name = client.Name,
                ProductCode = client.ProductCode,
                State = client.State
            });

            return ValidationResponse<GoogleAdClient>.Success(result);
        }
        catch (Exception ex)
        {
            return ValidationResponse<GoogleAdClient>.Failure(ex);
        }
    }
}
