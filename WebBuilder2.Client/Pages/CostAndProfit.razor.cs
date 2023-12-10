using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Pages;

public partial class CostAndProfit
{
    [Inject] public IAwsService AwsService { get; set; } = default!;
    [Inject] public IGoogleService GoogleService { get; set; } = default!;

    private decimal? _forecastedMonthlyCost;
    private List<GoogleAdSenseAccount>? _accounts = new();
    private List<GooglePayment>? _payments = new();
    private List<GoogleAdClient>? _adClients = new();
    private bool _isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _forecastedMonthlyCost = await AwsService.GetForecastedCostAsync();

            _accounts = await GoogleService.GetAccountsAsync();
            _payments = await GoogleService.GetPaymentsAsync();
            _adClients = await GoogleService.GetAdClientsAsync();
        }
        finally
        {
            _isLoading = false;
            StateHasChanged();
        }
    }
}
