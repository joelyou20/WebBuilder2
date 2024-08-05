namespace WebBuilder2.Client.Services.Contracts;

public interface ILocalStorageService
{
    Task SetItemAsync(string key, string value);
    Task<T?> GetItemAsync<T>(string key);
    Task RemoveItemAsync(string key);
    Task ClearAsync();
}
