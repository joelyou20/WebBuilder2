using Amazon.Runtime.Internal;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Observers;
using WebBuilder2.Client.Observers.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients
{
    public class GithubClient(HttpClient httpClient) : ClientBase(httpClient, "github"), IGithubClient
    {
        public async Task<IEnumerable<RepositoryModel>> GetRepositoriesAsync() => await GetAsync<IEnumerable<RepositoryModel>>("repos");
        public async Task<RepositoryModel> PostCreateRepoAsync(RepositoryModel repository) => await PostAsync<RepositoryModel>("repos/create", JsonContent.Create(repository));
        public async Task<IEnumerable<RepoContent>> PostRepositoryContentAsync(string userName, string repoName, string? path = null) => 
            await PostAsync<IEnumerable<RepoContent>> ($"repos/{userName}/{repoName}", JsonContent.Create(path));
        public async Task PostCopyRepoAsync(GithubCopyRepoRequest request) => await PostAsync("repos/copy", JsonContent.Create(request));
        public async Task<IEnumerable<GitTreeItem>> GetGitTreeAsync(string userName, string repoName) => await GetAsync<IEnumerable<GitTreeItem>>($"git/tree/{userName}/{repoName}");
        public async Task PostAuthenticateAsync() => await PostAsync("auth");
        public async Task<IEnumerable<GitIgnoreTemplateResponse>> GetGitIgnoreTemplatesAsync() => await GetAsync<IEnumerable<GitIgnoreTemplateResponse>>("gitignore");
        public async Task<IEnumerable<GithubProjectLicense>> GetGithubProjectLicensesAsync() => await GetAsync<IEnumerable<GithubProjectLicense>>("license");
        public async Task<IEnumerable<GithubSecret>> GetSecretsAsync(string userName, string repoName) => await GetAsync<IEnumerable<GithubSecret>>($"secrets/{userName}/{repoName}");
        public async Task<IEnumerable<GithubSecret>> CreateSecretAsync(GithubSecret secret, string userName, string repoName) => await CreateSecretAsync(new GithubSecret[] { secret }, userName, repoName);
        public async Task<IEnumerable<GithubSecret>> CreateSecretAsync(IEnumerable<GithubSecret> secrets, string userName, string repoName) => 
            await PutAsync<IEnumerable<GithubSecret>>($"secrets/{userName}/{repoName}", JsonContent.Create(secrets));
        public async Task<string> GetUserAsync() => await GetAsync<string>("user");
        public async Task CreateCommitAsync(GithubCreateCommitRequest request, string userName, long repoId) => await PutAsync($"commit/{userName}/{repoId}", JsonContent.Create(request));
    }
}
