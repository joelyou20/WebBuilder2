using Amazon.CostExplorer;
using Amazon.CostExplorer.Model;
using System.Net;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Server.Utils;
using WebBuilder2.Server.Utils.Extensions;

namespace WebBuilder2.Server.Services;

public class AwsCostExplorerService(AmazonCostExplorerClient client) : IAwsCostExplorerService
{
    private readonly AmazonCostExplorerClient _client = client;

    public async Task<string> GetForecastedCostAsync()
    {
        var request = new GetCostForecastRequest
        {
            Granularity = Granularity.DAILY,
            Metric = Metric.BLENDED_COST,
            TimePeriod = new DateInterval
            {
                Start = DateTime.UtcNow.ToShortDateString(),
                End = DateTime.UtcNow.AddDays(14).ToShortDateString()
            }
        };

        GetCostForecastResponse response = await _client.GetCostForecastAsync(request);

        AmazonServiceResponseValidator<AmazonCostExplorerException>.Validate(response, "Failed to get forecasted cost.");

        return response.Total.Amount;
    }
}
