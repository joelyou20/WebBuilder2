using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients.Contracts;

public interface IGithubClient
{
    Task<string> GetUserAsync();
    Task<IEnumerable<RepositoryModel>> GetRepositoriesAsync();
    Task<RepositoryModel> PostCreateRepoAsync(RepositoryModel repository);
    Task<IEnumerable<RepoContent>> PostRepositoryContentAsync(string userName, string repoName, string? path = null);
    Task PostCopyRepoAsync(GithubCopyRepoRequest request);
    Task<IEnumerable<GitTreeItem>> GetGitTreeAsync(string userName, string repoName);
    Task PostAuthenticateAsync();
    Task<IEnumerable<GitIgnoreTemplateResponse>> GetGitIgnoreTemplatesAsync();
    Task<IEnumerable<GithubProjectLicense>> GetGithubProjectLicensesAsync();
    Task<IEnumerable<GithubSecretResponse>> GetSecretsAsync(string userName, string repoName);
    Task<IEnumerable<GithubSecret>> CreateSecretAsync(GithubSecret secret, string userName, string repoName);
    Task<IEnumerable<GithubSecret>> CreateSecretAsync(IEnumerable<GithubSecret> secrets, string userName, string repoName);
    Task CreateCommitAsync(GithubCreateCommitRequest request, string userName, long repoId);
}
