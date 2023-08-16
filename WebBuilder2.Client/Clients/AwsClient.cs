using Newtonsoft.Json;
using System.Net.Http.Json;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients;

public class AwsClient : IAwsClient
{
    private HttpClient _httpClient;

    public AwsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Bucket>?> GetBucketsAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}aws/buckets");
        if (!response.IsSuccessStatusCode)
        {
            // Handle error
        }

        var message = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<IEnumerable<Bucket>>(message);
        return result;
    }

    public async Task<ValidationResponse?> CreateBucketsAsync(AwsCreateBucketRequest request)
    {
        var content = JsonContent.Create(request);
        HttpResponseMessage response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}aws/buckets", content);
        if (!response.IsSuccessStatusCode)
        {
            // Handle error
        }

        var message = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ValidationResponse>(message);
        return result;
    }

    public async Task<string?> GetForecastedCostAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}aws/cost");
        if (!response.IsSuccessStatusCode)
        {
            // Handle error
        }

        var message = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<string>(message);
        return result;
    }

    public async Task<IEnumerable<HostedZone>?> GetHostedZonesAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}aws/hostedzones");
        if (!response.IsSuccessStatusCode)
        {
            // Handle error
        }

        var message = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<IEnumerable<HostedZone>>(message);
        return result;
    }

    public async Task<ValidationResponse?> PostAppAsync(RepositoryModel repo)
    {
        var content = JsonContent.Create(repo);
        HttpResponseMessage response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}aws/app", content);
        if (!response.IsSuccessStatusCode)
        {
            // Handle error
        }

        var message = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ValidationResponse>(message);
        return result;
    }

    public async Task<ValidationResponse<DomainInquiry>?> GetSuggestedDomainNamesAsync(string domain)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}aws/route53/domain/suggest/{domain}");
        if (!response.IsSuccessStatusCode)
        {
            // Handle error
        }

        var message = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ValidationResponse<DomainInquiry>>(message);
        return result;
    }

    public async Task<ValidationResponse<Domain>?> GetRegisteredDomainsAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}aws/route53/domain/");
        if (!response.IsSuccessStatusCode)
        {
            // Handle error
        }

        var message = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ValidationResponse<Domain>>(message);
        return result;
    }

    public async Task<ValidationResponse?> PostRegisterDomainAsync(string domainName)
    {
        var content = JsonContent.Create(domainName);
        HttpResponseMessage response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}aws/route53/domain/register", content);
        if (!response.IsSuccessStatusCode)
        {
            // Handle error
        }

        var message = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ValidationResponse>(message);
        return result;
    }
}
