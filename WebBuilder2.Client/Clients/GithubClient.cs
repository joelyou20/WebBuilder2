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

        public async Task<RespositoryResponse> GetRepositoriesAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}github/repos");
            if (!response.IsSuccessStatusCode)
            {
                // Handle error
            }

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<RespositoryResponse>(message);
            return result;
        }

        public async Task<GithubTemplateResponse> GetTemplatesAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}github/templates");
            if (!response.IsSuccessStatusCode)
            {
                // Handle error
            }

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GithubTemplateResponse>(message);
            return result;
        }

        public async Task<GithubAuthenticationResponse> PostAuthenticateAsync(GithubAuthenticationRequest request)
        {
            var content = JsonContent.Create(request);
            HttpResponseMessage response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}github/auth", content);
            if (!response.IsSuccessStatusCode)
            {
                // Handle error
            }

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GithubAuthenticationResponse>(message);
            return result;
        }

        public async Task<GithubCreateRepoResponse> PostCreateRepoAsync(GithubCreateRepoRequest request)
        {
            var content = JsonContent.Create(request);
            HttpResponseMessage response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}github/repos/create", content);
            if (!response.IsSuccessStatusCode)
            {
                // Handle error
            }

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GithubCreateRepoResponse>(message);
            return result;
        }

        public async Task<IEnumerable<string>> GetGitIgnoreTemplatesAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}github/gitignore");
            if (!response.IsSuccessStatusCode)
            {
                // Handle error
            }

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<string>>(message);
            return result;
        }

        public async Task<IEnumerable<GithubProjectLicense>> GetGithubProjectLicensesAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}github/license");
            if (!response.IsSuccessStatusCode)
            {
                // Handle error
            }

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<GithubProjectLicense>>(message);
            return result;
        }
    }
}
