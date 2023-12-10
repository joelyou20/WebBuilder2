using Newtonsoft.Json;
using System.Net.Http.Json;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients;

public class ClientBase<T> where T : AuditableEntity
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

        response.EnsureSuccessStatusCode();

        return await ParseResponseAsync(response);
    }

    private async Task<ValidationResponse<T>> PostAsync(string? path = null, JsonContent? content = null)
    {
        HttpResponseMessage response = await _httpClient.PostAsync(path == null ? 
            $"{_httpClient.BaseAddress}{_endpoint}" : 
            $"{_httpClient.BaseAddress}{_endpoint}/{path}", content);
        response.EnsureSuccessStatusCode();

        return await ParseResponseAsync(response);
    }

    private async Task<ValidationResponse<T>> PutAsync(string? path = null, JsonContent? content = null)
    {
        HttpResponseMessage response = await _httpClient.PutAsync(path == null ?
            $"{_httpClient.BaseAddress}{_endpoint}" :
            $"{_httpClient.BaseAddress}{_endpoint}/{path}", content);
        response.EnsureSuccessStatusCode();

        return await ParseResponseAsync(response);
    }

    private async Task<ValidationResponse<T>> ParseResponseAsync(HttpResponseMessage response)
    {
        var message = await response.Content.ReadAsStringAsync();
        var result = ValidationResponse<T>.ToResult(message)!;
        return result;
    }
}
