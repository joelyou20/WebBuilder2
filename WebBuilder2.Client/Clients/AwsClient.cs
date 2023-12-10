using Newtonsoft.Json;
using System.Net.Http.Json;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Observers;
using WebBuilder2.Client.Observers.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients;

public class AwsClient : IAwsClient
{
    private HttpClient _httpClient;
    private IErrorObserver _errorObserver;

    public AwsClient(HttpClient httpClient, IErrorObserver errorObserver)
    {
        _httpClient = httpClient;
        _errorObserver = errorObserver;
    }

    public async Task<ValidationResponse<Bucket>> GetSingleBucketAsync(string name)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}aws/bucket/{name}");
        if (!response.IsSuccessStatusCode)
        {
            // Handle error
        }

        var message = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ValidationResponse<Bucket>>(message, new JsonSerializerSettings
        {
            Error = OnError
        })!;
        return result;
    }

    public async Task<ValidationResponse<Bucket>> GetBucketsAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}aws/buckets");
        if (!response.IsSuccessStatusCode)
        {
            // Handle error
        }

        var message = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ValidationResponse<Bucket>>(message, new JsonSerializerSettings
        {
            Error = OnError
        })!;
        return result;
    }

    public async Task<ValidationResponse> CreateBucketsAsync(AwsCreateBucketRequest request)
    {
        var content = JsonContent.Create(request);
        HttpResponseMessage response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}aws/buckets", content);
        if (!response.IsSuccessStatusCode)
        {
            // Handle error
        }

        var message = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ValidationResponse>(message, new JsonSerializerSettings
        {
            Error = OnError
        })!;
        return result;
    }

    public async Task<ValidationResponse> PostConfigureLoggingAsync(AwsConfigureLoggingRequest request)
    {
        var content = JsonContent.Create(request);
        HttpResponseMessage response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}aws/buckets/logging", content);
        if (!response.IsSuccessStatusCode)
        {
            // Handle error
        }

        var message = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ValidationResponse>(message, new JsonSerializerSettings
        {
            Error = OnError
        })!;
        return result;
    }

    public async Task<ValidationResponse> PostBucketPolicyAsync(AwsAddBucketPolicyRequest request)
    {
        var content = JsonContent.Create(request);
        HttpResponseMessage response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}aws/buckets/policy", content);
        if (!response.IsSuccessStatusCode)
        {
            // Handle error
        }

        var message = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ValidationResponse>(message, new JsonSerializerSettings
        {
            Error = OnError
        })!;
        return result;
    }

    public async Task<ValidationResponse> PostConfigurePublicAccessBlockAsync(AwsPublicAccessBlockRequest request)
    {
        var content = JsonContent.Create(request);
        HttpResponseMessage response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}aws/buckets/access", content);
        if (!response.IsSuccessStatusCode)
        {
            // Handle error
        }

        var message = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ValidationResponse>(message, new JsonSerializerSettings
        {
            Error = OnError
        })!;
        return result;
    }

    public async Task<ValidationResponse<string>> GetForecastedCostAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}aws/cost");
        if (!response.IsSuccessStatusCode)
        {
            // Handle error
        }

        var message = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ValidationResponse<string>>(message, new JsonSerializerSettings
        {
            Error = OnError
        })!;
        return result;
    }

    public async Task<ValidationResponse<HostedZone>> GetHostedZonesAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}aws/hostedzones");
        if (!response.IsSuccessStatusCode)
        {
            // Handle error
        }

        var message = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ValidationResponse<HostedZone>>(message, new JsonSerializerSettings
        {
            Error = OnError
        })!;
        return result;
    }

    public async Task<ValidationResponse> PostAppAsync(RepositoryModel repo)
    {
        var content = JsonContent.Create(repo);
        HttpResponseMessage response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}aws/app", content);
        if (!response.IsSuccessStatusCode)
        {
            // Handle error
        }

        var message = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ValidationResponse>(message, new JsonSerializerSettings
        {
            Error = OnError
        })!;
        return result;
    }

    public async Task<ValidationResponse<DomainInquiry>> GetSuggestedDomainNamesAsync(string domain)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}aws/route53/domain/suggest/{domain}");
        if (!response.IsSuccessStatusCode)
        {
            // Handle error
        }

        var message = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ValidationResponse<DomainInquiry>>(message, new JsonSerializerSettings
        {
            Error = OnError
        })!;
        return result;
    }

    public async Task<ValidationResponse<Domain>> GetRegisteredDomainsAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}aws/route53/domain/");
        if (!response.IsSuccessStatusCode)
        {
            // Handle error
        }

        var message = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ValidationResponse<Domain>>(message, new JsonSerializerSettings
        {
            Error = OnError
        })!;
        return result;
    }

    public async Task<ValidationResponse> PostRegisterDomainAsync(string domainName)
    {
        var content = JsonContent.Create(domainName);
        HttpResponseMessage response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}aws/route53/domain/register", content);
        if (!response.IsSuccessStatusCode)
        {
            // Handle error
        }

        var message = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ValidationResponse>(message, new JsonSerializerSettings
        {
            Error = OnError
        })!;
        return result;
    }

    private void OnError(object? sender, Newtonsoft.Json.Serialization.ErrorEventArgs e)
    {
        _errorObserver.AddError(e.ErrorContext.Error);
    }
}
