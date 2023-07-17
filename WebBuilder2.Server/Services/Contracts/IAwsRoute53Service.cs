using WebBuilder2.Shared.Models;

namespace WebBuilder2.Server.Services.Contracts;

public interface IAwsRoute53Service
{
    Task<IEnumerable<HostedZone>> GetHostedZonesAsync();
}
