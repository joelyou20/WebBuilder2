using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

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

    public async Task<ValidationResponse> CreateBucketsAsync(AwsCreateBucketRequest request)
    {
        return await _client.CreateBucketsAsync(request);
    }

    public async Task<ValidationResponse> PostConfigureLoggingAsync(AwsConfigureLoggingRequest request)
    {
        return await _client.PostConfigureLoggingAsync(request);
    }

    public async Task<ValidationResponse> PostBucketPolicyAsync(AwsAddBucketPolicyRequest request)
    {
        return await _client.PostBucketPolicyAsync(request);
    }

    public async Task<ValidationResponse> PostConfigurePublicAccessBlockAsync(AwsPublicAccessBlockRequest request)
    {
        return await _client.PostConfigurePublicAccessBlockAsync(request);
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

    public async Task<ValidationResponse> CreateAmplifyAppAsync(RepositoryModel repo)
    {
        return await _client.PostAppAsync(repo);
    }

    public async Task<ValidationResponse<DomainInquiry>> GetSuggestedDomainNamesAsync(string domain)
    {
        return await _client.GetSuggestedDomainNamesAsync(domain);
    }

    public async Task<ValidationResponse<Domain>> GetRegisteredDomainsAsync()
    {
        return await _client.GetRegisteredDomainsAsync();
    }

    public async Task<ValidationResponse> PostRegisterDomainAsync(string domainName)
    {
        return await _client.PostRegisterDomainAsync(domainName);
    }
}
