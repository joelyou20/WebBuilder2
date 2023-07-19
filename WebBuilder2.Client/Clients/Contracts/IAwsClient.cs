using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Clients.Contracts;

public interface IAwsClient
{
    Task<IEnumerable<Bucket>> GetBucketsAsync();
    Task<string> GetForecastedCostAsync();
    Task<IEnumerable<HostedZone>> GetHostedZonesAsync();
}
