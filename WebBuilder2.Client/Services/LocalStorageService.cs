using Microsoft.JSInterop;
using Newtonsoft.Json;
using WebBuilder2.Client.Observers.Contracts;
using WebBuilder2.Client.Services.Contracts;

namespace WebBuilder2.Client.Services;

public class LocalStorageService(IJSRuntime jsRuntime, IErrorObserver errorObserver) : ILocalStorageService
{
    private readonly IJSRuntime _jsRuntime = jsRuntime;
    private readonly IErrorObserver _errorObserver = errorObserver;

    public async Task SetItemAsync(string key, string value)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
    }

    public async Task<T?> GetItemAsync<T>(string key)
    {
        try
        {
            var result = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
            return JsonConvert.DeserializeObject<T>(result);
        }
        catch
        {
            // Too many annoying error messages. Just going to fail silently for now
            return default;
        }
    }

    public async Task RemoveItemAsync(string key)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
    }

    public async Task ClearAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.clear");
    }
}
