using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services.Contracts;

public interface IAwsRoute53Service
{
    Task<ValidationResponse<HostedZone>> GetHostedZonesAsync();
}
