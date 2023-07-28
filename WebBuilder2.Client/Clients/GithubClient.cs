using Amazon.Runtime.Internal;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients
{
    public class GithubClient : IGithubClient
    {
        private HttpClient _httpClient;

        public GithubClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ValidationResponse<RepositoryModel>> GetRepositoriesAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}github/repos");
            if (!response.IsSuccessStatusCode)
            {
                // Handle error
            }

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse<RepositoryModel>>(message);
            return result;
        }

        public async Task<ValidationResponse> PostAuthenticateAsync(GithubAuthenticationRequest request)
        {
            var content = JsonContent.Create(request);
            HttpResponseMessage response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}github/auth", content);
            if (!response.IsSuccessStatusCode)
            {
                // Handle error
            }

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse>(message);
            return result;
        }

        public async Task<ValidationResponse<RepositoryModel>> PostCreateRepoAsync(RepositoryModel repository)
        {
            var content = JsonContent.Create(repository);
            HttpResponseMessage response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}github/repos/create", content);
            if (!response.IsSuccessStatusCode)
            {
                // Handle error
            }

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse<RepositoryModel>>(message);
            return result;
        }

        public async Task<ValidationResponse<GitIgnoreTemplateResponse>> GetGitIgnoreTemplatesAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}github/gitignore");
            if (!response.IsSuccessStatusCode)
            {
                // Handle error
            }

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse<GitIgnoreTemplateResponse>>(message);
            return result;
        }

        public async Task<ValidationResponse<GithubProjectLicense>> GetGithubProjectLicensesAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}github/license");
            if (!response.IsSuccessStatusCode)
            {
                // Handle error
            }

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse<GithubProjectLicense>>(message);
            return result;
        }

        public async Task<ValidationResponse<GithubSecretResponse>> GetSecretsAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}github/secrets");
            if (!response.IsSuccessStatusCode)
            {
                // Handle error
            }

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse<GithubSecretResponse>>(message);
            return result;
        }
    }
}
