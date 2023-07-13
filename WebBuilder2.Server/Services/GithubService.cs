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

    public async Task<GithubRespositoryResponse> GetRespositoriesAsync()
    {
        List<GithubRepository> repos = new();

        var repositories = await _client.Repository.GetAllForCurrent();

        foreach(var repo in repositories)
        {
            repos.Add(new GithubRepository
            {
                Name = repo.Name
            });
        }

        var response = new GithubRespositoryResponse
        {
            Repositories = repos
        };

        return response;
    }
}
