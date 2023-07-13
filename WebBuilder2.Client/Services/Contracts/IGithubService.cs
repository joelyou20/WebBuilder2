using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Services.Contracts;

public interface IGithubService
{
    Task<GithubRespositoryResponse> GetRepositoriesAsync();
}
