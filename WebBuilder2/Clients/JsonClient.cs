using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using WebBuilder2.Clients.Contracts;
using WebBuilder2.Models;

namespace WebBuilder2.Clients
{
    public class JsonClient : IDatabaseClient
    {
        private HttpClient _httpClient;

        public JsonClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Site>?> GetSitesAsync()
        {
            var response = await _httpClient.GetAsync("./test.json");
            var message = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<Site>>(message);
        }
    }
}
