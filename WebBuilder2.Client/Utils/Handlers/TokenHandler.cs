using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Utils.Handlers;

public class TokenHandler(ILocalStorageService localStorage, NavigationManager navigationManager) : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage = localStorage;
    private readonly NavigationManager _navigationManager = navigationManager;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var isLoginRequest = request.RequestUri != null && request.RequestUri.PathAndQuery.Contains("user/login");

        var user = await _localStorage.GetItemAsync<LoginUserResponse>("user");
        if (!isLoginRequest && (user != null && !string.IsNullOrEmpty(user.Token)))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
        }

        var message = await base.SendAsync(request, cancellationToken);

        if (message.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            // Redirect to the login page
            _navigationManager.NavigateTo("/identity/account/login");
            return null!;
        }

        return message;
    }
}
