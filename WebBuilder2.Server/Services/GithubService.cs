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

        foreach(var repo in repositories)
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
}
