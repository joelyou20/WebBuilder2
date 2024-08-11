using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Services.Contracts;

public interface IAwsService
{
    Task<Bucket?> GetSingleBucketAsync(string name);
    Task<IEnumerable<Bucket>?> GetBucketsAsync();
    Task CreateBucketsAsync(AwsCreateBucketRequest request);
    Task PostConfigureLoggingAsync(AwsConfigureLoggingRequest request);
    Task PostBucketPolicyAsync(AwsAddBucketPolicyRequest request);
    Task PostConfigurePublicAccessBlockAsync(AwsPublicAccessBlockRequest request);
    Task<IEnumerable<HostedZone>?> GetHostedZonesAsync();
    Task<decimal?> GetForecastedCostAsync();
    Task CreateAmplifyAppAsync(RepositoryModel repo);
    Task<IEnumerable<DomainInquiry>?> GetSuggestedDomainNamesAsync(string domain);
    Task<IEnumerable<Domain>?> GetRegisteredDomainsAsync();
    Task PostRegisterDomainAsync(string domainName);
    Task<AwsNewSSLCertificateResponse?> PostNewSSLCertificateAsync(AwsNewSSLCertificateRequest request);
}
