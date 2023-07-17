using Newtonsoft.Json;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Clients;

public class AwsClient : IAwsClient
{
    private HttpClient _httpClient;

    public AwsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Bucket>> GetBucketsAsync()
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

    public async Task<IEnumerable<HostedZone>> GetHostedZonesAsync()
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
}
