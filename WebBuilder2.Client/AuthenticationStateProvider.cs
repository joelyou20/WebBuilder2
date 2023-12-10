using Microsoft.JSInterop;
using System.Security.Claims;
using WebBuilder2.Client.Models;

namespace WebBuilder2.Client;

public class AuthenticationStateProvider : IAuthenticationStateProvider
{
    public User CurrentUser = new();

    [JSInvokable]
    public void GoogleLogin(GoogleResponse googleResponse)
    {
        var principal = new ClaimsPrincipal();
        var user = User.FromGoogleJwt(googleResponse.Credential);
        CurrentUser = user;

        if (user == null) throw new Exception("User is null");

        //principal = user.ToClaimsPrincipal();

        //NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }
}
