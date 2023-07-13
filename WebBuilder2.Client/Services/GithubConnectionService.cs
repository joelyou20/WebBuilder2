using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Services.Contracts;

namespace WebBuilder2.Client.Services
{
    public class GithubConnectionService : IGithubConnectionService
    {
        private IGithubClient _client;

        public GithubConnectionService(IGithubClient client)
        {
            _client = client;
        }

        public async Task ConnectAsync()
        {
            await _client.PostConnectionRequestAsync();
        }
    }
}
