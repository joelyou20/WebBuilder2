using Newtonsoft.Json;
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

        public async Task<Site?> GetSingleSiteAsync(string name)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}site/{name}");
            if (!response.IsSuccessStatusCode)
            {
                // Handle error
            }

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Site>(message);
            return result;
        }

        public async Task<IEnumerable<Site>?> GetSitesAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}site");
            if(!response.IsSuccessStatusCode)
            {
                // Handle error
            }

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<Site>>(message);
            return result;
        }
    }
}
