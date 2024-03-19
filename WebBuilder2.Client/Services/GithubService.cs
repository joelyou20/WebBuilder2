using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Components;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Observers.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Services
{
    public class GithubService : ServiceBase, IGithubService
    {
        private IGithubClient _client;
        private NavigationManager _navigationManager;
        private IErrorObserver _errorObserver;

        public GithubService(IGithubClient client, NavigationManager navigationManager, IErrorObserver errorObserver, ILogService logService) : base(errorObserver, logService)
        {
            _client = client;
            _navigationManager = navigationManager;
            _errorObserver = errorObserver;
        }

        public async Task<string?> GetGithubUser()
        {
            await PostAuthenticateAsync();

            IEnumerable<string>? result = await ExecuteAsync<string>(_client.GetUserAsync);

            return result?.SingleOrDefault();
        }

        public async Task<List<RepositoryModel>?> GetRepositoriesAsync()
        {
            await PostAuthenticateAsync();

            IEnumerable<RepositoryModel>? result = await ExecuteAsync(_client.GetRepositoriesAsync);

            return result?.ToList();
        }

        public async Task<RepoContent?> GetRepositoryContentAsync(string repoName, string? reference = null)
        {
            var userName = await GetLoginAsync();

            if (userName == null) throw new Exception("Failed to login to Github");

            IEnumerable<RepoContent>? result = await ExecuteAsync(() => _client.PostRepositoryContentAsync(userName, repoName, reference));

            return result?.SingleOrDefault();
        }

        public async Task PostCopyRepoAsync(GithubCopyRepoRequest request)
        {
            await PostAuthenticateAsync();

            await ExecuteAsync(() => _client.PostCopyRepoAsync(request));
        }

        public async Task<List<GitTreeItem>?> GetGitTreeAsync(string repoName)
        {
            var userName = await GetLoginAsync();
            IEnumerable<GitTreeItem>? result = await ExecuteAsync(() => _client.GetGitTreeAsync(userName, repoName));

            return result?.ToList();
        }

        public async Task<GitIgnoreTemplateResponse?> GetGitIgnoreTemplatesAsync()
        {
            await PostAuthenticateAsync();

            IEnumerable<GitIgnoreTemplateResponse>? result = await ExecuteAsync(_client.GetGitIgnoreTemplatesAsync);

            return result?.SingleOrDefault();
        }

        public async Task<List<GithubProjectLicense>?> GetGithubProjectLicensesAsync()
        {
            await PostAuthenticateAsync();

            IEnumerable<GithubProjectLicense>? result = await ExecuteAsync(_client.GetGithubProjectLicensesAsync);

            return result?.ToList();
        }

        public async Task<GithubSecretResponse?> GetSecretsAsync(string repoName)
        {
            var userName = await GetLoginAsync();
            IEnumerable<GithubSecretResponse>? result = await ExecuteAsync(() => _client.GetSecretsAsync(userName, repoName));

            return result?.SingleOrDefault();
        }

        public async Task<List<GithubSecret>?> CreateSecretAsync(GithubSecret secret, string repoName) =>
            await CreateSecretAsync(new GithubSecret[] { secret }, repoName);

        public async Task<List<GithubSecret>?> CreateSecretAsync(IEnumerable<GithubSecret> secrets, string repoName)
        {
            var userName = await GetLoginAsync();
            IEnumerable<GithubSecret>? result = await ExecuteAsync(() => _client.CreateSecretAsync(secrets, userName, repoName));

            return result?.ToList();
        }

        public async Task CreateCommitAsync(GithubCreateCommitRequest request, string repoName)
        {
            string? userName = await GetLoginAsync();

            if (userName == null) throw new Exception("Failed to login to Github");

            await ExecuteAsync(() => _client.CreateCommitAsync(request, userName, repoName));
        }

        public async Task PostAuthenticateAsync()
        {
            await ExecuteAsync(() => _client.PostAuthenticateAsync());
        }

        public async Task<RepositoryModel?> PostCreateRepoAsync(RepositoryModel repository)
        {
            await PostAuthenticateAsync();

            var result = await ExecuteAsync(() => _client.PostCreateRepoAsync(repository));
            return result?.SingleOrDefault();
        }

        private async Task<string?> GetLoginAsync()
        {
            await PostAuthenticateAsync();

            IEnumerable<string>? result = await ExecuteAsync(_client.GetUserAsync);

            return result?.SingleOrDefault();
        }
    }
}
