using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Services.Contracts;

public interface IGithubService
{
    Task<ValidationResponse<string>> GetGithubUser();
    Task<ValidationResponse<RepositoryModel>> GetRepositoriesAsync();
    Task<ValidationResponse<GitIgnoreTemplateResponse>> GetGitIgnoreTemplatesAsync();
    Task<ValidationResponse<GithubProjectLicense>> GetGithubProjectLicensesAsync();
    Task<ValidationResponse> PostAuthenticateAsync(GithubAuthenticationRequest request);
    Task<ValidationResponse<RepositoryModel>> PostCreateRepoAsync(RepositoryModel repository);
    Task<ValidationResponse<GithubSecretResponse>> GetSecretsAsync(string repoName);
    Task<ValidationResponse<GithubSecret>> CreateSecretAsync(GithubSecret secret, string repoName);
}
