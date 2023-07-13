using System.Net.Http;
using System.Net.Http.Json;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Clients
{
    public class SiteClient : ISiteClient
    {
        private HttpClient _httpClient;

        public SiteClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Site>?> GetSitesAsync()
        {
            var path = "https://localhost:7137/site";
            HttpResponseMessage response = await _httpClient.GetAsync(path);
            if(!response.IsSuccessStatusCode)
            {
                // Handle error
            }

            var result = await response.Content.ReadAsStringAsync();
            return null;
        }
    }
}
