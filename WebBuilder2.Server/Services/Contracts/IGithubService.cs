using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services.Contracts
{
    public interface IGithubService
    {
        Task<ValidationResponse<RepositoryModel>> GetRepositoriesAsync();
        Task<ValidationResponse> AuthenticateUserAsync(GithubAuthenticationRequest request);
        Task<ValidationResponse<RepositoryModel>> CreateRepoAsync(RepositoryModel repository);
        Task<ValidationResponse<GitIgnoreTemplateResponse>> GetGitIgnoreTemplatesAsync();
        Task<ValidationResponse<GithubProjectLicense>> GetLicenseTemplatesAsync();
        Task<ValidationResponse<GithubSecretResponse>> GetSecretsAsync(string userName, string repoName);
        Task<ValidationResponse<GithubSecret>> CreateSecretAsync(GithubSecret secret, string userName, string repoName);
        Task<ValidationResponse<string>> GetUserAsync();
        Task<ValidationResponse> CreateCommitAsync(string owner, string repoName, GithubCreateCommitRequest request);
    }
}
