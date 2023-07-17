using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Services.Contracts;

public interface IAwsService
{
    Task<IEnumerable<Bucket>> GetBucketsAsync();
    Task<IEnumerable<HostedZone>> GetHostedZonesAsync();
}
