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

        public async Task<GithubRespositoryResponse> GetRepositoriesAsync()
        {
            return await _client.GetRepositoriesAsync();
        }
    }
}
