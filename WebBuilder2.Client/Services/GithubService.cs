using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Services
{
    public class GithubService : IGithubService
    {
        private IGithubClient _client;

        public GithubService(IGithubClient client)
        {
            _client = client;
        }

        public async Task<RespositoryResponse> GetRepositoriesAsync()
        {
            return await _client.GetRepositoriesAsync();
        }
        public async Task<GithubTemplateResponse> GetTemplatesAsync()
        {
            return await _client.GetTemplatesAsync();
        }
        public async Task<IEnumerable<string>> GetGitIgnoreTemplatesAsync()
        {
            return await _client.GetGitIgnoreTemplatesAsync();
        }
        public async Task<GithubAuthenticationResponse> PostAuthenticateAsync(GithubAuthenticationRequest request)
        {
            return await _client.PostAuthenticateAsync(request);
        }
        public async Task<GithubCreateRepoResponse> PostCreateRepoAsync(GithubCreateRepoRequest request)
        {
            return await _client.PostCreateRepoAsync(request);
        }
    }
}
