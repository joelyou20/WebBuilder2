using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients.Contracts;

public interface IGithubClient
{
    Task<ValidationResponse<string>?> GetUserAsync();
    Task<ValidationResponse<RepositoryModel>?> GetRepositoriesAsync();
    Task<ValidationResponse<RepositoryModel>?> PostCreateRepoAsync(RepositoryModel repository);
    Task<ValidationResponse<RepoContent>?> PostRepositoryContentAsync(string userName, string repoName, string? path = null);
    Task<ValidationResponse?> PostCopyRepoAsync(GithubCopyRepoRequest request);
    Task<ValidationResponse<GitTreeItem>?> GetGitTreeAsync(string userName, string repoName);
    Task<ValidationResponse?> PostAuthenticateAsync(GithubAuthenticationRequest request);
    Task<ValidationResponse<GitIgnoreTemplateResponse>?> GetGitIgnoreTemplatesAsync();
    Task<ValidationResponse<GithubProjectLicense>?> GetGithubProjectLicensesAsync();
    Task<ValidationResponse<GithubSecretResponse>?> GetSecretsAsync(string userName, string repoName);
    Task<ValidationResponse<GithubSecret>?> CreateSecretAsync(GithubSecret secret, string userName, string repoName);
    Task<ValidationResponse<GithubSecret>?> CreateSecretAsync(IEnumerable<GithubSecret> secrets, string userName, string repoName);
    Task<ValidationResponse?> CreateCommitAsync(GithubCreateCommitRequest request, string userName, string repoName);
}
