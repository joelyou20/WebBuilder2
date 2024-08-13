using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Server.Services.Contracts
{
    public interface IGithubService
    {
        Task<IEnumerable<RepositoryModel>> GetRepositoriesAsync();
        Task AuthenticateUserAsync();
        Task<RepositoryModel> CreateRepoAsync(RepositoryModel repository);
        Task<GitIgnoreTemplateResponse> GetGitIgnoreTemplatesAsync();
        Task<IEnumerable<GithubProjectLicense>> GetLicenseTemplatesAsync();
        Task<IEnumerable<GithubSecret>> GetSecretsAsync(string userName, string repoName);
        Task CreateSecretAsync(IEnumerable<GithubSecret> secrets, string userName, string repoName);
        Task<string> GetUserAsync();
        Task CreateCommitAsync(string owner, long repoId, GithubCreateCommitRequest request);
        Task<IEnumerable<RepoContent>> GetRepositoryContentAsync(string owner, string repoName, string? path = null);
        Task<IEnumerable<GitTreeItem>> GetGitTreeAsync(string owner, string repoName);
        Task<bool> CopyRepoAsync(string clonedRepoName, string newRepoName, string path = ".");
    }
}
