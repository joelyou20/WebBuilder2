using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Clients
{
    public class GithubClient : IGithubClient
    {
        private HttpClient _httpClient;

        public GithubClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> PostConnectionRequestAsync()
        {
            var request = new GithubConnectionRequest();
            var content = JsonContent.Create(request);

            HttpResponseMessage response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}github", content);

            return response.IsSuccessStatusCode;
        }
    }
}
