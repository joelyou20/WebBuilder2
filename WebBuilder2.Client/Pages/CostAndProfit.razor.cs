using Microsoft.AspNetCore.Components;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Pages;

public partial class CostAndProfit
{
    [Inject] public IAwsService AwsService { get; set; } = default!;
    [Inject] public IGoogleService GoogleService { get; set; } = default!;

    private decimal _forecastedMonthlyCost;
    private List<GoogleAdSenseAccount> _accounts = [];
    private List<GooglePayment> _payments = [];
    private List<GoogleAdClient> _adClients = [];
    private bool _isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _forecastedMonthlyCost = await AwsService.GetForecastedCostAsync();

            _accounts = (await GoogleService.GetAccountsAsync()).ToList();
            _payments = (await GoogleService.GetPaymentsAsync()).ToList();
            _adClients = (await GoogleService.GetAdClientsAsync()).ToList();
        }
        finally
        {
            _isLoading = false;
            StateHasChanged();
        }
    }
}
