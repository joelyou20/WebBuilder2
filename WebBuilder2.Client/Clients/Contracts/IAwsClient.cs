using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients.Contracts;

public interface IAwsClient
{
    Task<IEnumerable<Bucket>?> GetBucketsAsync();
    Task<ValidationResponse?> CreateBucketsAsync(AwsCreateBucketRequest request);
    Task<ValidationResponse?> PostConfigureLoggingAsync(AwsConfigureLoggingRequest request);
    Task<ValidationResponse?> PostBucketPolicyAsync(AwsAddBucketPolicyRequest request);
    Task<ValidationResponse?> PostConfigurePublicAccessBlockAsync(AwsPublicAccessBlockRequest request);
    Task<string?> GetForecastedCostAsync();
    Task<IEnumerable<HostedZone>?> GetHostedZonesAsync();
    Task<ValidationResponse?> PostAppAsync(RepositoryModel repo);
    Task<ValidationResponse<DomainInquiry>?> GetSuggestedDomainNamesAsync(string domain);
    Task<ValidationResponse<Domain>?> GetRegisteredDomainsAsync();
    Task<ValidationResponse?> PostRegisterDomainAsync(string domainName);
}
