using Amazon.Runtime.Internal;
using Newtonsoft.Json;
using System.Net.Http.Json;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Observers;
using WebBuilder2.Client.Observers.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients;

public class AwsClient(HttpClient httpClient) : ClientBase(httpClient, "aws"), IAwsClient
{
    public async Task<Bucket> GetSingleBucketAsync(string name) => await GetAsync<Bucket>($"bucket/{name}");
    public async Task<IEnumerable<Bucket>> GetBucketsAsync() => await GetAsync<IEnumerable<Bucket>>("buckets");
    public async Task CreateBucketsAsync(AwsCreateBucketRequest request) => await PutAsync("buckets", JsonContent.Create(request));
    public async Task PostConfigureLoggingAsync(AwsConfigureLoggingRequest request) => await PostAsync("buckets/logging", JsonContent.Create(request));
    public async Task PostBucketPolicyAsync(AwsAddBucketPolicyRequest request) => await PostAsync("buckets/policy", JsonContent.Create(request));
    public async Task PostConfigurePublicAccessBlockAsync(AwsPublicAccessBlockRequest request) => await PostAsync("buckets/access", JsonContent.Create(request));
    public async Task<string> GetForecastedCostAsync() => await GetAsync<string>("cost");
    public async Task<IEnumerable<HostedZone>> GetHostedZonesAsync() => await GetAsync<IEnumerable<HostedZone>>("hostedzones");
    public async Task PostAppAsync(RepositoryModel repo) => await PostAsync("app", JsonContent.Create(repo));
    public async Task<IEnumerable<DomainInquiry>> GetSuggestedDomainNamesAsync(string domain) => await GetAsync<IEnumerable<DomainInquiry>>($"route53/domain/suggest/{domain}");
    public async Task<IEnumerable<Domain>> GetRegisteredDomainsAsync() => await GetAsync<IEnumerable<Domain>>("route53/domain");
    public async Task PostRegisterDomainAsync(string domainName) => await PostAsync("route53/domain/register", JsonContent.Create(domainName));
    public async Task<AwsNewSSLCertificateResponse> PostNewSSLCertificateAsync(AwsNewSSLCertificateRequest request) => await PostAsync<AwsNewSSLCertificateResponse>("cert", JsonContent.Create(request));
}
