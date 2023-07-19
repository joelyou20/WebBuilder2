using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Xml.Linq;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients
{
    public class SiteClient : ISiteClient
    {
        private HttpClient _httpClient;

        public SiteClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AddSiteAsync(Site site)
        {
            var content = JsonContent.Create(site);
            HttpResponseMessage response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}site", content);
            if (!response.IsSuccessStatusCode)
            {
                // Handle error
            }
        }

        public async Task<ValidationResponse<Site>?> GetSingleSiteAsync(long id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}site/{id}");
            if (!response.IsSuccessStatusCode)
            {
                // Handle error
            }

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse<Site>>(message);
            return result;
        }

        public async Task<ValidationResponse<Site>?> GetSitesAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}site");
            if(!response.IsSuccessStatusCode)
            {
                // Handle error
            }

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse<Site>>(message);
            return result;
        }

        public async Task<ValidationResponse<Site>?> SoftDeleteSiteAsync(Site site)
        {
            var content = JsonContent.Create(site);
            HttpResponseMessage response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}site/delete", content);
            if (!response.IsSuccessStatusCode)
            {
                // Handle error
            }

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse<Site>>(message);
            return result;
        }
    }
}
