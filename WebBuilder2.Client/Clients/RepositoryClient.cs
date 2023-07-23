using Newtonsoft.Json;
using System.Net.Http.Json;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients
{
    public class RepositoryClient : IRepositoryClient
    {
        private HttpClient _httpClient;

        public RepositoryClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ValidationResponse<Repository>> GetRepositoriesAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}repository");
            if (!response.IsSuccessStatusCode)
            {
                // Handle error
            }

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse<Repository>>(message);
            return result;
        }

        public async Task<ValidationResponse<Repository>> GetSingleRepositoryAsync(long id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}repository/{id}");
            if (!response.IsSuccessStatusCode)
            {
                // Handle error
            }

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse<Repository>>(message);
            return result;
        }

        public async Task<ValidationResponse<Repository>> SoftDeleteRepositoryAsync(Repository repository)
        {
            var content = JsonContent.Create(repository);
            HttpResponseMessage response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}site/delete", content);
            if (!response.IsSuccessStatusCode)
            {
                // Handle error
            }

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse<Repository>>(message);
            return result;
        }

        public async Task<ValidationResponse<Repository>> AddRepositoryAsync(Repository repository)
        {
            var content = JsonContent.Create(repository);
            HttpResponseMessage response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}repository", content);
            if (!response.IsSuccessStatusCode)
            {
                // Handle error
            }

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse<Repository>>(message);
            return result;
        }
    }
}
