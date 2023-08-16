using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Services.Contracts;

public interface IAwsService
{
    Task<IEnumerable<Bucket>> GetBucketsAsync();
    Task<ValidationResponse?> CreateBucketsAsync(AwsCreateBucketRequest request);
    Task<IEnumerable<HostedZone>> GetHostedZonesAsync();
    Task<decimal> GetForecastedCostAsync();
    Task<ValidationResponse> CreateAmplifyAppAsync(RepositoryModel repo);
    Task<ValidationResponse<DomainInquiry>> GetSuggestedDomainNamesAsync(string domain);
    Task<ValidationResponse<Domain>> GetRegisteredDomainsAsync();
    Task<ValidationResponse> PostRegisterDomainAsync(string domainName);
}
