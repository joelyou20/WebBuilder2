using Amazon.Runtime.Internal;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Sockets;
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

        public async Task<ValidationResponse<RepositoryModel>?> GetRepositoriesAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}github/repos");
            response.EnsureSuccessStatusCode();

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse<RepositoryModel>>(message);
            return result;
        }

        public async Task<ValidationResponse?> PostAuthenticateAsync(GithubAuthenticationRequest request)
        {
            var content = JsonContent.Create(request);
            HttpResponseMessage response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}github/auth", content);
            response.EnsureSuccessStatusCode();

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse>(message);
            return result;
        }

        public async Task<ValidationResponse<RepositoryModel>?> PostCreateRepoAsync(RepositoryModel repository)
        {
            var content = JsonContent.Create(repository);
            HttpResponseMessage response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}github/repos/create", content);
            response.EnsureSuccessStatusCode();

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse<RepositoryModel>>(message);
            return result;
        }

        public async Task<ValidationResponse<GitIgnoreTemplateResponse>?> GetGitIgnoreTemplatesAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}github/gitignore");
            response.EnsureSuccessStatusCode();

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse<GitIgnoreTemplateResponse>>(message);
            return result;
        }

        public async Task<ValidationResponse<GithubProjectLicense>?> GetGithubProjectLicensesAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}github/license");
            response.EnsureSuccessStatusCode();

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse<GithubProjectLicense>>(message);
            return result;
        }

        public async Task<ValidationResponse<GithubSecretResponse>?> GetSecretsAsync(string userName, string repoName)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}github/secrets/{userName}/{repoName}");
            response.EnsureSuccessStatusCode();

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse<GithubSecretResponse>>(message);
            return result;
        }

        public async Task<ValidationResponse<GithubSecret>?> CreateSecretAsync(GithubSecret secret, string userName, string repoName)
        {
            JsonContent content = JsonContent.Create(secret);
            HttpResponseMessage response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}github/secrets/{userName}/{repoName}", content);
            response.EnsureSuccessStatusCode();

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse<GithubSecret>>(message);
            return result;
        }

        public async Task<ValidationResponse<string>?> GetUserAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}github/user");
            response.EnsureSuccessStatusCode();

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse<string>>(message);
            return result;
        }

        public async Task<ValidationResponse?> CreateCommitAsync(GithubCreateCommitRequest request, string userName, string repoName)
        {
            JsonContent content = JsonContent.Create(request);
            HttpResponseMessage response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}github/commit/{userName}/{repoName}", content);
            response.EnsureSuccessStatusCode();

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse>(message);
            return result;
        }
    }
}
