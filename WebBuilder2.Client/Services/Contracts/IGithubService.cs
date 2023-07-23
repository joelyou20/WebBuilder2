using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Services.Contracts;

public interface IGithubService
{
    Task<ValidationResponse<Repository>> GetRepositoriesAsync();
    Task<ValidationResponse<GitIgnoreTemplateResponse>> GetGitIgnoreTemplatesAsync();
    Task<ValidationResponse<GithubProjectLicense>> GetGithubProjectLicensesAsync();
    Task<ValidationResponse> PostAuthenticateAsync(GithubAuthenticationRequest request);
    Task<ValidationResponse<Repository>> PostCreateRepoAsync(GithubCreateRepoRequest request);
}
