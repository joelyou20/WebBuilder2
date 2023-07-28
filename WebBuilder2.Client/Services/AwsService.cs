using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Services;

public class AwsService : IAwsService
{
    private IAwsClient _client;

    public AwsService(IAwsClient client)
    {
        _client = client;
    }

    public async Task<IEnumerable<Bucket>> GetBucketsAsync()
    {
        return await _client.GetBucketsAsync();
    }

    public async Task<IEnumerable<HostedZone>> GetHostedZonesAsync()
    {
        return await _client.GetHostedZonesAsync();
    }

    public async Task<decimal> GetForecastedCostAsync()
    {
        var response = await _client.GetForecastedCostAsync();
        decimal.TryParse(response, out decimal result);

        return result;
    }
}
