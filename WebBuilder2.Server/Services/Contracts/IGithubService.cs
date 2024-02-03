using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services.Contracts
{
    public interface IGithubService
    {
        Task<ValidationResponse<RepositoryModel>> GetRepositoriesAsync();
        Task<ValidationResponse> AuthenticateUserAsync();
        Task<ValidationResponse<RepositoryModel>> CreateRepoAsync(RepositoryModel repository);
        Task<ValidationResponse<GitIgnoreTemplateResponse>> GetGitIgnoreTemplatesAsync();
        Task<ValidationResponse<GithubProjectLicense>> GetLicenseTemplatesAsync();
        Task<ValidationResponse<GithubSecretResponse>> GetSecretsAsync(string userName, string repoName);
        Task<ValidationResponse<GithubSecret>> CreateSecretAsync(IEnumerable<GithubSecret> secrets, string userName, string repoName);
        Task<ValidationResponse<string>> GetUserAsync();
        Task<ValidationResponse> CreateCommitAsync(string owner, string repoName, GithubCreateCommitRequest request);
        Task<ValidationResponse<RepoContent>> GetRepositoryContentAsync(string owner, string repoName, string? path = null);
        Task<ValidationResponse<GitTreeItem>> GetGitTreeAsync(string owner, string repoName);
        Task<ValidationResponse> CopyRepoAsync(string clonedRepoName, string newRepoName, string path = ".");
    }
}
