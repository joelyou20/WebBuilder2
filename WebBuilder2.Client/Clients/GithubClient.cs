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
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients
{
    public class GithubClient : ClientBase, IGithubClient
    {
        public GithubClient(HttpClient httpClient) : base(httpClient, "github") { }

        public async Task<ValidationResponse<RepositoryModel>> GetRepositoriesAsync() => await GetAsync<RepositoryModel>("repos");
        public async Task<ValidationResponse<RepositoryModel>> PostCreateRepoAsync(RepositoryModel repository) => await PostAsync<RepositoryModel>("repos/create", JsonContent.Create(repository));
        public async Task<ValidationResponse<RepoContent>> PostRepositoryContentAsync(string userName, string repoName, string? path = null) => await PostAsync<RepoContent> ($"repos/{userName}/{repoName}", JsonContent.Create(path));
        public async Task<ValidationResponse> PostCopyRepoAsync(GithubCopyRepoRequest request) => await PostAsync("repos/copy", JsonContent.Create(request));
        public async Task<ValidationResponse<GitTreeItem>> GetGitTreeAsync(string userName, string repoName) => await GetAsync<GitTreeItem>($"git/tree/{userName}/{repoName}");
        public async Task<ValidationResponse> PostAuthenticateAsync() => await PostAsync("auth");
        public async Task<ValidationResponse<GitIgnoreTemplateResponse>> GetGitIgnoreTemplatesAsync() => await GetAsync<GitIgnoreTemplateResponse>("gitignore");
        public async Task<ValidationResponse<GithubProjectLicense>> GetGithubProjectLicensesAsync() => await GetAsync<GithubProjectLicense>("license");
        public async Task<ValidationResponse<GithubSecretResponse>> GetSecretsAsync(string userName, string repoName) => await GetAsync<GithubSecretResponse>($"secrets/{userName}/{repoName}");
        public async Task<ValidationResponse<GithubSecret>> CreateSecretAsync(GithubSecret secret, string userName, string repoName) => await CreateSecretAsync(new GithubSecret[] { secret }, userName, repoName);
        public async Task<ValidationResponse<GithubSecret>> CreateSecretAsync(IEnumerable<GithubSecret> secrets, string userName, string repoName) => await PutAsync<GithubSecret>($"secrets/{userName}/{repoName}", JsonContent.Create(secrets));
        public async Task<ValidationResponse<string>> GetUserAsync() => await GetAsync<string>("user");
        public async Task<ValidationResponse> CreateCommitAsync(GithubCreateCommitRequest request, string userName, string repoName) => await PutAsync($"commit/{userName}/{repoName}", JsonContent.Create(request));
    }
}
