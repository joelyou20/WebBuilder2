using Amazon.Runtime.Internal;
using Newtonsoft.Json;
using System.Net.Http.Json;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Observers;
using WebBuilder2.Client.Observers.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients;

public class AwsClient : ClientBase, IAwsClient
{
    public AwsClient(HttpClient httpClient) : base(httpClient, "aws") { }

    public async Task<ValidationResponse<Bucket>> GetSingleBucketAsync(string name) => await GetAsync<Bucket>($"bucket/{name}");
    public async Task<ValidationResponse<Bucket>> GetBucketsAsync() => await GetAsync<Bucket>("buckets");
    public async Task<ValidationResponse> CreateBucketsAsync(AwsCreateBucketRequest request) => await PutAsync("buckets", JsonContent.Create(request));
    public async Task<ValidationResponse> PostConfigureLoggingAsync(AwsConfigureLoggingRequest request) => await PostAsync("buckets/logging", JsonContent.Create(request));
    public async Task<ValidationResponse> PostBucketPolicyAsync(AwsAddBucketPolicyRequest request) => await PostAsync("buckets/policy", JsonContent.Create(request));
    public async Task<ValidationResponse> PostConfigurePublicAccessBlockAsync(AwsPublicAccessBlockRequest request) => await PostAsync("buckets/access", JsonContent.Create(request));
    public async Task<ValidationResponse<string>> GetForecastedCostAsync() => await GetAsync<string>("cost");
    public async Task<ValidationResponse<HostedZone>> GetHostedZonesAsync() => await GetAsync<HostedZone>("hostedzones");
    public async Task<ValidationResponse> PostAppAsync(RepositoryModel repo) => await PostAsync("app", JsonContent.Create(repo));
    public async Task<ValidationResponse<DomainInquiry>> GetSuggestedDomainNamesAsync(string domain) => await GetAsync<DomainInquiry>($"route53/domain/suggest/{domain}");
    public async Task<ValidationResponse<Domain>> GetRegisteredDomainsAsync() => await GetAsync<Domain>("route53/domain");
    public async Task<ValidationResponse> PostRegisterDomainAsync(string domainName) => await PostAsync("route53/domain/register", JsonContent.Create(domainName));
    public async Task<ValidationResponse<AwsNewSSLCertificateResponse>> PostNewSSLCertificateAsync(AwsNewSSLCertificateRequest request) => await PostAsync<AwsNewSSLCertificateResponse>("cert", JsonContent.Create(request));
}
