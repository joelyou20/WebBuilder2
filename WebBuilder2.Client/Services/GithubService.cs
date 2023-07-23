using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Services
{
    public class GithubService : IGithubService
    {
        private IGithubClient _client;

        public GithubService(IGithubClient client)
        {
            _client = client;
        }

        public async Task<ValidationResponse<Repository>> GetRepositoriesAsync()
        {
            return await _client.GetRepositoriesAsync();
        }
        public async Task<ValidationResponse<GitIgnoreTemplateResponse>> GetGitIgnoreTemplatesAsync()
        {
            return await _client.GetGitIgnoreTemplatesAsync();
        }
        public async Task<ValidationResponse<GithubProjectLicense>> GetGithubProjectLicensesAsync()
        {
            return await _client.GetGithubProjectLicensesAsync();
        }
        public async Task<ValidationResponse> PostAuthenticateAsync(GithubAuthenticationRequest request)
        {
            return await _client.PostAuthenticateAsync(request);
        }
        public async Task<ValidationResponse<Repository>> PostCreateRepoAsync(GithubCreateRepoRequest request)
        {
            return await _client.PostCreateRepoAsync(request);
        }
    }
}
