using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Services;

public class AwsService(IAwsClient client) : IAwsService
{
    private readonly IAwsClient _client = client;

    public async Task<Bucket> GetSingleBucketAsync(string name) => await _client.GetSingleBucketAsync(name);
    public async Task<IEnumerable<Bucket>> GetBucketsAsync() => await _client.GetBucketsAsync();
    public async Task CreateBucketsAsync(AwsCreateBucketRequest request) => await _client.CreateBucketsAsync(request);
    public async Task PostConfigureLoggingAsync(AwsConfigureLoggingRequest request) => await _client.PostConfigureLoggingAsync(request);
    public async Task PostBucketPolicyAsync(AwsAddBucketPolicyRequest request) => await _client.PostBucketPolicyAsync(request);
    public async Task PostConfigurePublicAccessBlockAsync(AwsPublicAccessBlockRequest request) => await _client.PostConfigurePublicAccessBlockAsync(request);
    public async Task<IEnumerable<HostedZone>> GetHostedZonesAsync() => await _client.GetHostedZonesAsync();

    public async Task<decimal> GetForecastedCostAsync()
    {
        string? response = await _client.GetForecastedCostAsync();

        decimal.TryParse(response, out decimal result);

        return result;
    }

    public async Task<IEnumerable<DomainInquiry>> GetSuggestedDomainNamesAsync(string domain) => await _client.GetSuggestedDomainNamesAsync(domain);
    public async Task<IEnumerable<Domain>> GetRegisteredDomainsAsync() => await _client.GetRegisteredDomainsAsync();
    public async Task PostRegisterDomainAsync(string domainName) => await _client.PostRegisterDomainAsync(domainName);
    public async Task<AwsNewSSLCertificateResponse> PostNewSSLCertificateAsync(AwsNewSSLCertificateRequest request) => await _client.PostNewSSLCertificateAsync(request);
}
