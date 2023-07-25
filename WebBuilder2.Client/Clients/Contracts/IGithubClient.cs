using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients.Contracts;

public interface IGithubClient
{
    Task<ValidationResponse<Repository>> GetRepositoriesAsync();
    Task<ValidationResponse> PostAuthenticateAsync(GithubAuthenticationRequest request);
    Task<ValidationResponse<Repository>> PostCreateRepoAsync(Repository repository);
    Task<ValidationResponse<GitIgnoreTemplateResponse>> GetGitIgnoreTemplatesAsync();
    Task<ValidationResponse<GithubProjectLicense>> GetGithubProjectLicensesAsync();
}
