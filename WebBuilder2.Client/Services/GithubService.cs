using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Components;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Observers.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Services
{
    public class GithubService(IGithubClient client, NavigationManager navigationManager) : IGithubService
    {
        private readonly IGithubClient _client = client;
        private readonly NavigationManager _navigationManager = navigationManager;

        public async Task<string> GetGithubUser()
        {
            await PostAuthenticateAsync();

            string result = await _client.GetUserAsync();

            return result;
        }

        public async Task<List<RepositoryModel>> GetRepositoriesAsync()
        {
            await PostAuthenticateAsync();

            IEnumerable<RepositoryModel> result = await _client.GetRepositoriesAsync();

            return result.ToList();
        }

        public async Task<RepoContent> GetRepositoryContentAsync(string repoName, string? reference = null)
        {
            var userName = await GetLoginAsync();

            if (userName == null) throw new Exception("Failed to login to Github");

            IEnumerable<RepoContent> result = await _client.PostRepositoryContentAsync(userName, repoName, reference);

            return result.Single();
        }

        public async Task PostCopyRepoAsync(GithubCopyRepoRequest request)
        {
            await PostAuthenticateAsync();

            await _client.PostCopyRepoAsync(request);
        }

        public async Task<List<GitTreeItem>> GetGitTreeAsync(string repoName)
        {
            var userName = await GetLoginAsync();

            if (userName == null) throw new ArgumentNullException(nameof(userName));

            IEnumerable<GitTreeItem> result = await _client.GetGitTreeAsync(userName, repoName);

            return result.ToList();
        }

        public async Task<GitIgnoreTemplateResponse> GetGitIgnoreTemplatesAsync()
        {
            await PostAuthenticateAsync();

            IEnumerable<GitIgnoreTemplateResponse> result = await _client.GetGitIgnoreTemplatesAsync();

            return result.Single();
        }

        public async Task<List<GithubProjectLicense>> GetGithubProjectLicensesAsync()
        {
            await PostAuthenticateAsync();

            IEnumerable<GithubProjectLicense>? result = await _client.GetGithubProjectLicensesAsync();

            return result.ToList();
        }

        public async Task<IEnumerable<GithubSecret>> GetSecretsAsync(string repoName)
        {
            var userName = await GetLoginAsync();

            if (userName == null) throw new ArgumentNullException(nameof(userName));

            IEnumerable<GithubSecret> result = await _client.GetSecretsAsync(userName, repoName);

            return result;
        }

        public async Task<List<GithubSecret>> CreateSecretAsync(GithubSecret secret, string repoName) =>
            await CreateSecretAsync(new GithubSecret[] { secret }, repoName);

        public async Task<List<GithubSecret>> CreateSecretAsync(IEnumerable<GithubSecret> secrets, string repoName)
        {
            var userName = await GetLoginAsync();

            if (userName == null) throw new ArgumentNullException(nameof(userName));

            IEnumerable<GithubSecret> result = await _client.CreateSecretAsync(secrets, userName, repoName);

            return result.ToList();
        }

        public async Task CreateCommitAsync(GithubCreateCommitRequest request, long repoId)
        {
            string? userName = await GetLoginAsync() ?? throw new Exception("Failed to login to Github");
            var repos = (await _client.GetRepositoriesAsync());
            var repoName = repos?.FirstOrDefault(x => x.ExternalId == repoId)?.Name ?? 
                throw new Exception($"Cannot find Github Repository with ID = {repoId}");
            await _client.CreateCommitAsync(request, userName, repoId);
        }

        public async Task PostAuthenticateAsync()
        {
            await _client.PostAuthenticateAsync();
        }

        public async Task<RepositoryModel> PostCreateRepoAsync(RepositoryModel repository)
        {
            await PostAuthenticateAsync();

            var result = await _client.PostCreateRepoAsync(repository);
            return result;
        }

        private async Task<string> GetLoginAsync()
        {
            await PostAuthenticateAsync();

            string result = await _client.GetUserAsync();

            return result;
        }
    }
}
