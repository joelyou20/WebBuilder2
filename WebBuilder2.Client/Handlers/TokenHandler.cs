using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Handlers;

public class TokenHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;
    private readonly NavigationManager _navigationManager;

    public TokenHandler(ILocalStorageService localStorage, NavigationManager navigationManager)
    {
        _localStorage = localStorage;
        _navigationManager = navigationManager;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var user = await _localStorage.GetItemAsync<LoginUserResponse>("user");
        if (!string.IsNullOrEmpty(user?.Token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
        }

        var message = await base.SendAsync(request, cancellationToken);

        if (message.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            // Redirect to the login page
            _navigationManager.NavigateTo("/identity/account/login");
        }

        return message;
    }
}
