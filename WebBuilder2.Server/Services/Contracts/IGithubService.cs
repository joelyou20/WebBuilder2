using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Server.Services.Contracts
{
    public interface IGithubService
    {
        Task<RespositoryResponse> GetRespositoriesAsync();
        Task<GithubAuthenticationResponse> AuthenticateUserAsync(GithubAuthenticationRequest request);
        Task<GithubCreateRepoResponse> CreateRepoAsync(GithubCreateRepoRequest request);
        Task<IEnumerable<string>> GetGitIgnoreTemplatesAsync();
    }
}
