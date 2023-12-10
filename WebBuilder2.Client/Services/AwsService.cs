using Amazon.Runtime.Internal;
using System.Xml.Linq;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Observers.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Services;

public class AwsService : ServiceBase, IAwsService
{
    private IAwsClient _client;

    public AwsService(IAwsClient client, IErrorObserver errorObserver, ILogService logService) : base(errorObserver, logService)
    {
        _client = client;
    }

    public async Task<Bucket?> GetSingleBucketAsync(string name)
    {
        IEnumerable<Bucket>? result = await ExecuteAsync(() => _client.GetSingleBucketAsync(name));

        return result?.SingleOrDefault();
    }

    public async Task<List<Bucket>?> GetBucketsAsync()
    {
        IEnumerable<Bucket>? result = await ExecuteAsync(_client.GetBucketsAsync);
        return result?.ToList();
    }

    public async Task CreateBucketsAsync(AwsCreateBucketRequest request)
    {
        await ExecuteAsync(() => _client.CreateBucketsAsync(request));
    }

    public async Task PostConfigureLoggingAsync(AwsConfigureLoggingRequest request)
    {
        await ExecuteAsync(() => _client.PostConfigureLoggingAsync(request));
    }

    public async Task PostBucketPolicyAsync(AwsAddBucketPolicyRequest request)
    {
        await ExecuteAsync(() => _client.PostBucketPolicyAsync(request));
    }

    public async Task PostConfigurePublicAccessBlockAsync(AwsPublicAccessBlockRequest request)
    {
        await ExecuteAsync(() => _client.PostConfigurePublicAccessBlockAsync(request));
    }

    public async Task<List<HostedZone>?> GetHostedZonesAsync()
    {
        IEnumerable<HostedZone>? result = await ExecuteAsync(_client.GetHostedZonesAsync);

        return result?.ToList();
    }

    public async Task<decimal?> GetForecastedCostAsync()
    {
        IEnumerable<string>? response = await ExecuteAsync(_client.GetForecastedCostAsync);

        string? cost = response?.SingleOrDefault();

        decimal.TryParse(cost, out decimal result);

        return result;
    }

    public async Task CreateAmplifyAppAsync(RepositoryModel repo)
    {
        await ExecuteAsync(() => _client.PostAppAsync(repo));
    }

    public async Task<List<DomainInquiry>?> GetSuggestedDomainNamesAsync(string domain)
    {
        IEnumerable<DomainInquiry>? result = await ExecuteAsync(() => _client.GetSuggestedDomainNamesAsync(domain));

        return result?.ToList();
    }

    public async Task<List<Domain>?> GetRegisteredDomainsAsync()
    {
        IEnumerable<Domain>? result = await ExecuteAsync(_client.GetRegisteredDomainsAsync);

        return result?.ToList();
    }

    public async Task PostRegisterDomainAsync(string domainName)
    {
        await ExecuteAsync(() => _client.PostRegisterDomainAsync(domainName));
    }
}
