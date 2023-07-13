using Octokit;
using WebBuilder2.Server.Services.Contracts;

namespace WebBuilder2.Server.Services;

public class GithubConnectionService : IGithubConnectionService
{
    private GitHubClient _client;

    public GithubConnectionService(GitHubClient client)
    {
        _client = client;
    }

    public async Task<bool> ConnectAsync()
    {
        var x = _client.User.Email;

        return true;
    }

    public async Task AuthenticateUserAsync()
    {
        //var oauthLoginRequest = new OauthLoginRequest(clientId); 
        //oauthLoginRequest.Scopes.Add("user"); 
        //oauthLoginRequest.Scopes.Add("public_repo"); 
        //var loginUrl = _client.Oauth.GetGitHubLoginUrl(oauthLoginRequest);  
        //var oauthTokenRequest = new OauthTokenRequest(clientId,clientSecret,code); 
        //var oauthToken = await _client.Oauth.CreateAccessToken(oauthTokenRequest);
        //_client.Credentials = new Credentials(oauthToken.AccessToken);
    }
}
