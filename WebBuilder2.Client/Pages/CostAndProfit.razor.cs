using Microsoft.AspNetCore.Components;
using WebBuilder2.Client.Services.Contracts;

namespace WebBuilder2.Client.Pages;

public partial class CostAndProfit
{
    [Inject] public IAwsService AwsService { get; set; } = default!;

    private decimal _forecastedMonthlyCost = 0m;

    protected override async Task OnInitializedAsync()
    {
        _forecastedMonthlyCost = await AwsService.GetForecastedCostAsync();
    }
}
