using Newtonsoft.Json;
using System.Net.Http.Json;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients;

public class ClientBase<T> where T : class
{
    private HttpClient _httpClient;
    private string _endpoint;

    public ClientBase(HttpClient httpClient, string endpoint)
    {
        _httpClient = httpClient;
        _endpoint = endpoint;
    }

    public async Task<ValidationResponse<T>> AddAsync(T value) => await AddRangeAsync(new T[] { value });
    public async Task<ValidationResponse<T>> AddRangeAsync(IEnumerable<T> values) => await PutAsync(content: JsonContent.Create(values));
    public async Task<ValidationResponse<T>> GetSingleAsync(long id) => await GetAsync(path: id.ToString());
    public async Task<ValidationResponse<T>> SoftDeleteAsync(T value) => await SoftDeleteRangeAsync(new T[] { value });
    public async Task<ValidationResponse<T>> SoftDeleteRangeAsync(IEnumerable<T> values) => await PostAsync("delete", JsonContent.Create(values));
    public async Task<ValidationResponse<T>> UpdateAsync(T value) => await UpdateRangeAsync(new T[] { value });
    public async Task<ValidationResponse<T>> UpdateRangeAsync(IEnumerable<T> values) => await PostAsync("update", JsonContent.Create(values));

    public async Task<ValidationResponse<T>> GetAsync(string? path = null, Dictionary<string, string>? filter = null)
    {
        var url = $"{_httpClient.BaseAddress}{_endpoint}";
        if (path != null) url = $"{url}/{path}";
        if (filter != null) url = $"{url}?{string.Join('&', filter.Select(kv => $"{kv.Key}={kv.Value}"))}";

        HttpResponseMessage response = await _httpClient.GetAsync(url);

        return await ValidationResponse<T>.ParseResponseAsync(response);
    }

    public async Task<ValidationResponse<T>> PostAsync(string? path = null, JsonContent? content = null)
    {
        HttpResponseMessage response = await _httpClient.PostAsync(path == null ? 
            $"{_httpClient.BaseAddress}{_endpoint}" : 
            $"{_httpClient.BaseAddress}{_endpoint}/{path}", content);

        return await ValidationResponse<T>.ParseResponseAsync(response);
    }

    private async Task<ValidationResponse<T>> PutAsync(string? path = null, JsonContent? content = null)
    {
        HttpResponseMessage response = await _httpClient.PutAsync(path == null ?
            $"{_httpClient.BaseAddress}{_endpoint}" :
            $"{_httpClient.BaseAddress}{_endpoint}/{path}", content);

        return await ValidationResponse<T>.ParseResponseAsync(response);
    }
}

public class ClientBase
{
    private HttpClient _httpClient;
    private string _endpoint;

    public ClientBase(HttpClient httpClient, string endpoint)
    {
        _httpClient = httpClient;
        _endpoint = endpoint;
    }

    public async Task<ValidationResponse<T>> AddAsync<T>(T value) where T : class => await AddRangeAsync(new T[] { value });
    public async Task<ValidationResponse<T>> AddRangeAsync<T>(IEnumerable<T> values) where T : class => await PutAsync<T>(content: JsonContent.Create(values));
    public async Task<ValidationResponse<T>> GetSingleAsync<T>(long id) where T : class => await GetAsync<T>(path: id.ToString());
    public async Task<ValidationResponse<T>> SoftDeleteAsync<T>(T value) where T : class => await SoftDeleteRangeAsync(new T[] { value });
    public async Task<ValidationResponse<T>> SoftDeleteRangeAsync<T>(IEnumerable<T> values) where T : class => await PostAsync<T>("delete", JsonContent.Create(values));
    public async Task<ValidationResponse<T>> UpdateAsync<T>(T value) where T : class => await UpdateRangeAsync(new T[] { value });
    public async Task<ValidationResponse<T>> UpdateRangeAsync<T>(IEnumerable<T> values) where T : class => await PostAsync<T>("update", JsonContent.Create(values));

    public async Task<ValidationResponse<T>> GetAsync<T>(string? path = null, Dictionary<string, string>? filter = null) where T : class
    {
        var url = $"{_httpClient.BaseAddress}{_endpoint}";
        if (path != null) url = $"{url}/{path}";
        if (filter != null) url = $"{url}?{string.Join('&', filter.Select(kv => $"{kv.Key}={kv.Value}"))}";

        HttpResponseMessage response = await _httpClient.GetAsync(url);

        return await ValidationResponse<T>.ParseResponseAsync(response);
    }

    public async Task<ValidationResponse<T>> PostAsync<T>(string? path = null, JsonContent? content = null) where T : class
    {
        HttpResponseMessage response = await _httpClient.PostAsync(path == null ?
            $"{_httpClient.BaseAddress}{_endpoint}" :
            $"{_httpClient.BaseAddress}{_endpoint}/{path}", content);

        return await ValidationResponse<T>.ParseResponseAsync(response);
    }

    public async Task<ValidationResponse> PostAsync(string? path = null, JsonContent? content = null)
    {
        HttpResponseMessage response = await _httpClient.PostAsync(path == null ?
            $"{_httpClient.BaseAddress}{_endpoint}" :
            $"{_httpClient.BaseAddress}{_endpoint}/{path}", content);

        return await ValidationResponse.ParseResponseAsync(response);
    }

    public async Task<ValidationResponse<T>> PutAsync<T>(string? path = null, JsonContent? content = null) where T : class
    {
        HttpResponseMessage response = await _httpClient.PutAsync(path == null ?
            $"{_httpClient.BaseAddress}{_endpoint}" :
            $"{_httpClient.BaseAddress}{_endpoint}/{path}", content);

        return await ValidationResponse<T>.ParseResponseAsync(response);
    }

    public async Task<ValidationResponse> PutAsync(string? path = null, JsonContent? content = null)
    {
        HttpResponseMessage response = await _httpClient.PutAsync(path == null ?
            $"{_httpClient.BaseAddress}{_endpoint}" :
            $"{_httpClient.BaseAddress}{_endpoint}/{path}", content);

        return await ValidationResponse.ParseResponseAsync(response);
    }
}
