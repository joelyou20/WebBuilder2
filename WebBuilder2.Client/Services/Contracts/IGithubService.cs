using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Services.Contracts;

public interface IGithubService
{
    Task<string?> GetGithubUser();
    Task<List<RepositoryModel>?> GetRepositoriesAsync();
    Task<RepoContent?> GetRepositoryContentAsync(string repoName, string? path = null);
    Task PostCopyRepoAsync(GithubCopyRepoRequest request);
    Task<List<GitTreeItem>?> GetGitTreeAsync(string repoName);
    Task<GitIgnoreTemplateResponse?> GetGitIgnoreTemplatesAsync();
    Task<List<GithubProjectLicense>?> GetGithubProjectLicensesAsync();
    Task<ValidationResponse> PostAuthenticateAsync(GithubAuthenticationRequest request);
    Task<RepositoryModel?> PostCreateRepoAsync(RepositoryModel repository);
    Task<GithubSecretResponse?> GetSecretsAsync(string repoName);
    Task<List<GithubSecret>?> CreateSecretAsync(GithubSecret secret, string repoName);
    Task<List<GithubSecret>?> CreateSecretAsync(IEnumerable<GithubSecret> secrets, string repoName);
    Task CreateCommitAsync(GithubCreateCommitRequest request, string repoName);
}
