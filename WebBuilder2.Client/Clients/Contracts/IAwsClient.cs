using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients.Contracts;

public interface IAwsClient
{
    Task<Bucket> GetSingleBucketAsync(string name);
    Task<IEnumerable<Bucket>> GetBucketsAsync();
    Task CreateBucketsAsync(AwsCreateBucketRequest request);
    Task PostConfigureLoggingAsync(AwsConfigureLoggingRequest request);
    Task PostBucketPolicyAsync(AwsAddBucketPolicyRequest request);
    Task PostConfigurePublicAccessBlockAsync(AwsPublicAccessBlockRequest request);
    Task<string> GetForecastedCostAsync();
    Task<IEnumerable<HostedZone>> GetHostedZonesAsync();
    Task PostAppAsync(RepositoryModel repo);
    Task<IEnumerable<DomainInquiry>> GetSuggestedDomainNamesAsync(string domain);
    Task<IEnumerable<Domain>> GetRegisteredDomainsAsync();
    Task PostRegisterDomainAsync(string domainName);
    Task<AwsNewSSLCertificateResponse> PostNewSSLCertificateAsync(AwsNewSSLCertificateRequest request);
}
