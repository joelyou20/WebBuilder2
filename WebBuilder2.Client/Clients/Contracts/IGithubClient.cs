using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Clients.Contracts;

public interface IGithubClient
{
    Task<RespositoryResponse> GetRepositoriesAsync();
    Task<GithubTemplateResponse> GetTemplatesAsync();
    Task<GithubAuthenticationResponse> PostAuthenticateAsync(GithubAuthenticationRequest request);
    Task<GithubCreateRepoResponse> PostCreateRepoAsync(GithubCreateRepoRequest request);
    Task<IEnumerable<string>> GetGitIgnoreTemplatesAsync();
}
