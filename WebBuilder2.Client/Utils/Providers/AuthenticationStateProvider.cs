using Microsoft.JSInterop;
using System.Security.Claims;
using WebBuilder2.Client.Models;
using WebBuilder2.Client.Utils.Providers.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Utils.Providers;

public class AuthenticationStateProvider() : IAuthenticationStateProvider
{
    public UserModel? CurrentUser = new();

    [JSInvokable]
    public void GoogleLogin(GoogleResponse googleResponse)
    {
        var principal = new ClaimsPrincipal();
        var user = JwtHelper.FromGoogleJwt(googleResponse.Credential);
        CurrentUser = user;

        if (user == null) throw new Exception("User is null");

        //principal = user.ToClaimsPrincipal();

        //NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }
}
