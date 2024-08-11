using Newtonsoft.Json;
using System.Net.Http.Json;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients;

public class ClientBase<T>(HttpClient httpClient, string endpoint) where T : class
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly string _endpoint = endpoint;

    public async Task<T> AddAsync(T value) => (await AddRangeAsync(new T[] { value })).Single();
    public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> values) => await PutAsync(content: JsonContent.Create(values));
    public async Task<T> GetSingleAsync(long id) => (await GetAsync(path: id.ToString())).Single();
    public async Task<T> SoftDeleteAsync(T value) => (await SoftDeleteRangeAsync(new T[] { value })).Single();
    public async Task<IEnumerable<T>> SoftDeleteRangeAsync(IEnumerable<T> values) => await PostAsync("delete", JsonContent.Create(values));
    public async Task<T> UpdateAsync(T value) => (await UpdateRangeAsync(new T[] { value })).Single();
    public async Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> values) => await PostAsync("update", JsonContent.Create(values));

    public async Task<IEnumerable<T>> GetAsync(string? path = null, Dictionary<string, string>? filter = null)
    {
        var url = $"{_httpClient.BaseAddress}{_endpoint}";
        if (path != null) url = $"{url}/{path}";
        if (filter != null) url = $"{url}?{string.Join('&', filter.Select(kv => $"{kv.Key}={kv.Value}"))}";

        HttpResponseMessage response = await _httpClient.GetAsync(url);

        return await ParseResponseAsync(response);
    }

    public async Task<IEnumerable<T>> PostAsync(string? path = null, JsonContent? content = null)
    {
        HttpResponseMessage response = await _httpClient.PostAsync(path == null ? 
            $"{_httpClient.BaseAddress}{_endpoint}" : 
            $"{_httpClient.BaseAddress}{_endpoint}/{path}", content);

        return await ParseResponseAsync(response);
    }

    private async Task<IEnumerable<T>> PutAsync(string? path = null, JsonContent? content = null)
    {
        HttpResponseMessage response = await _httpClient.PutAsync(path == null ?
            $"{_httpClient.BaseAddress}{_endpoint}" :
            $"{_httpClient.BaseAddress}{_endpoint}/{path}", content);

        return await ParseResponseAsync(response);
    }

    public static async Task<IEnumerable<T>> ParseResponseAsync(HttpResponseMessage response)
    {
        var message = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<IEnumerable<T>>(message)!;
        return result;
    }
}

public class ClientBase(HttpClient httpClient, string endpoint)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly string _endpoint = endpoint;

    public async Task<T> AddAsync<T>(T value) where T : class => await AddRangeAsync(new T[] { value });
    public async Task<T> AddRangeAsync<T>(IEnumerable<T> values) where T : class => await PutAsync<T>(content: JsonContent.Create(values));
    public async Task<T> GetSingleAsync<T>(long id) where T : class => await GetAsync<T>(path: id.ToString());
    public async Task<T> SoftDeleteAsync<T>(T value) where T : class => await SoftDeleteRangeAsync(new T[] { value });
    public async Task<T> SoftDeleteRangeAsync<T>(IEnumerable<T> values) where T : class => await PostAsync<T>("delete", JsonContent.Create(values));
    public async Task<T> UpdateAsync<T>(T value) where T : class => await UpdateRangeAsync(new T[] { value });
    public async Task<T> UpdateRangeAsync<T>(IEnumerable<T> values) where T : class => await PostAsync<T>("update", JsonContent.Create(values));

    public async Task<T> GetAsync<T>(string? path = null, Dictionary<string, string>? filter = null) where T : class
    {
        var url = $"{_httpClient.BaseAddress}{_endpoint}";
        if (path != null) url = $"{url}/{path}";
        if (filter != null) url = $"{url}?{string.Join('&', filter.Select(kv => $"{kv.Key}={kv.Value}"))}";

        HttpResponseMessage response = await _httpClient.GetAsync(url);

        response.EnsureSuccessStatusCode();

        return await ParseResponseAsync<T>(response);
    }

    public async Task<T> PostAsync<T>(string? path = null, JsonContent? content = null) where T : class
    {
        HttpResponseMessage response = await _httpClient.PostAsync(path == null ?
            $"{_httpClient.BaseAddress}{_endpoint}" :
            $"{_httpClient.BaseAddress}{_endpoint}/{path}", content);

        response.EnsureSuccessStatusCode();

        return await ParseResponseAsync<T>(response);
    }

    public async Task PostAsync(string? path = null, JsonContent? content = null)
    {
        var response = await _httpClient.PostAsync(path == null ?
            $"{_httpClient.BaseAddress}{_endpoint}" :
            $"{_httpClient.BaseAddress}{_endpoint}/{path}", content);

        response.EnsureSuccessStatusCode();
    }

    public async Task<T> PutAsync<T>(string? path = null, JsonContent? content = null) where T : class
    {
        HttpResponseMessage response = await _httpClient.PutAsync(path == null ?
            $"{_httpClient.BaseAddress}{_endpoint}" :
            $"{_httpClient.BaseAddress}{_endpoint}/{path}", content);

        response.EnsureSuccessStatusCode();

        return await ParseResponseAsync<T>(response);
    }

    public async Task PutAsync(string? path = null, JsonContent? content = null)
    {
        var response = await _httpClient.PutAsync(path == null ?
            $"{_httpClient.BaseAddress}{_endpoint}" :
            $"{_httpClient.BaseAddress}{_endpoint}/{path}", content);

        response.EnsureSuccessStatusCode();
    }

    public static async Task<T> ParseResponseAsync<T>(HttpResponseMessage response) where T : class
    {
        var message = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<T>(message)!;
        return result;
    }
}
