using Microsoft.IdentityModel.Tokens;
using Octokit;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Server.Services;

public class GithubService : IGithubService
{
    private GitHubClient _client;

    public GithubService(GitHubClient client)
    {
        _client = client;
    }

    public async Task<RespositoryResponse> GetRespositoriesAsync()
    {
        List<Shared.Models.Repository> repos = new();

        var repositories = await _client.Repository.GetAllForCurrent();

        foreach (var repo in repositories)
        {
            repos.Add(new Shared.Models.Repository
            {
                Id = repo.Id,
                Name = repo.Name,
            });
        }

        var response = new RespositoryResponse
        {
            Repositories = repos
        };

        return response;
    }

    public async Task<GithubAuthenticationResponse> AuthenticateUserAsync(GithubAuthenticationRequest request)
    {
        try
        {
            // Check if client is already authenticated. _client.User.Current() will throw an AuthorizationException if the client is not authenticated
            User user = await _client.User.Current();
            return new GithubAuthenticationResponse
            {
                IsAuthenticated = true,
                Message = "Success"
            };
        }
        catch (AuthorizationException)
        {
            // IF client is not authenticated and there is no PAT in the request, throw an error
            // ELSE attempt to authorize client with PAT
            if (request == null || request.PersonalAccessToken.IsNullOrEmpty()) {
                return new GithubAuthenticationResponse
                {
                    IsAuthenticated = false,
                    Message = "Failed to Authenticate"
                };
            }
            else
            {
                _client.Credentials = new Credentials(request.PersonalAccessToken);
                User user = await _client.User.Current();
                return new GithubAuthenticationResponse
                {
                    IsAuthenticated = user != null,
                    Message = user == null ? "Failed to Authenticate" : $"Success: User {user.Email} is authenticated"
                };
            }
        }
    }
}
