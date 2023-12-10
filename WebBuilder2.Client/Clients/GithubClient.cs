﻿using Amazon.Runtime.Internal;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Observers;
using WebBuilder2.Client.Observers.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients
{
    public class GithubClient : IGithubClient
    {
        private HttpClient _httpClient;
        private IErrorObserver _errorObserver;

        public GithubClient(HttpClient httpClient, IErrorObserver errorObserver)
        {
            _httpClient = httpClient;
            _errorObserver = errorObserver;
        }

        #region Repos

        public async Task<ValidationResponse<RepositoryModel>> GetRepositoriesAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}github/repos");
            response.EnsureSuccessStatusCode();

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse<RepositoryModel>>(message, new JsonSerializerSettings
            {
                Error = OnError
            })!;
            return result;
        }

        public async Task<ValidationResponse<RepositoryModel>> PostCreateRepoAsync(RepositoryModel repository)
        {
            var content = JsonContent.Create(repository);
            HttpResponseMessage response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}github/repos/create", content);
            response.EnsureSuccessStatusCode();

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse<RepositoryModel>>(message, new JsonSerializerSettings
            {
                Error = OnError
            })!;
            return result;
        }

        public async Task<ValidationResponse<RepoContent>> PostRepositoryContentAsync(string userName, string repoName, string? path = null)
        {
            var content = JsonContent.Create(path);
            HttpResponseMessage response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}github/repos/{userName}/{repoName}", content);
            response.EnsureSuccessStatusCode();

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse<RepoContent>>(message, new JsonSerializerSettings
            {
                Error = OnError
            })!;
            return result;
        }

        public async Task<ValidationResponse> PostCopyRepoAsync(GithubCopyRepoRequest request)
        {
            var content = JsonContent.Create(request);
            HttpResponseMessage response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}github/repos/copy", content);
            response.EnsureSuccessStatusCode();

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse>(message, new JsonSerializerSettings
            {
                Error = OnError
            })!;
            return result;
        }

        #endregion

        #region Git

        public async Task<ValidationResponse<GitTreeItem>> GetGitTreeAsync(string userName, string repoName)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}github/git/tree/{userName}/{repoName}");
            response.EnsureSuccessStatusCode();

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse<GitTreeItem>>(message, new JsonSerializerSettings
            {
                Error = OnError
            })!;
            return result;
        }

        #endregion

        #region Auth

        public async Task<ValidationResponse> PostAuthenticateAsync(GithubAuthenticationRequest request)
        {
            var content = JsonContent.Create(request);
            HttpResponseMessage response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}github/auth", content);
            response.EnsureSuccessStatusCode();

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse>(message, new JsonSerializerSettings
            {
                Error = OnError
            })!;
            return result;
        }

        #endregion

        #region GitIgnore

        public async Task<ValidationResponse<GitIgnoreTemplateResponse>> GetGitIgnoreTemplatesAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}github/gitignore");
            response.EnsureSuccessStatusCode();

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse<GitIgnoreTemplateResponse>>(message, new JsonSerializerSettings
            {
                Error = OnError
            })!;
            return result;
        }

        #endregion

        #region License

        public async Task<ValidationResponse<GithubProjectLicense>> GetGithubProjectLicensesAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}github/license");
            response.EnsureSuccessStatusCode();

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse<GithubProjectLicense>>(message, new JsonSerializerSettings
            {
                Error = OnError
            })!;
            return result;
        }

        #endregion

        #region Secrets

        public async Task<ValidationResponse<GithubSecretResponse>> GetSecretsAsync(string userName, string repoName)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}github/secrets/{userName}/{repoName}");
            response.EnsureSuccessStatusCode();

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse<GithubSecretResponse>>(message, new JsonSerializerSettings
            {
                Error = OnError
            })!;
            return result;
        }

        public async Task<ValidationResponse<GithubSecret>> CreateSecretAsync(GithubSecret secret, string userName, string repoName) =>
            await CreateSecretAsync(new GithubSecret[] { secret }, userName, repoName);

        public async Task<ValidationResponse<GithubSecret>> CreateSecretAsync(IEnumerable<GithubSecret> secrets, string userName, string repoName)
        {
            JsonContent content = JsonContent.Create(secrets);
            HttpResponseMessage response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}github/secrets/{userName}/{repoName}", content);
            response.EnsureSuccessStatusCode();

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse<GithubSecret>>(message, new JsonSerializerSettings
            {
                Error = OnError
            })!;
            return result;
        }

        #endregion

        #region User

        public async Task<ValidationResponse<string>> GetUserAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}github/user");
            response.EnsureSuccessStatusCode();

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse<string>>(message, new JsonSerializerSettings
            {
                Error = OnError
            })!;
            return result;
        }

        #endregion

        #region Commit

        public async Task<ValidationResponse> CreateCommitAsync(GithubCreateCommitRequest request, string userName, string repoName)
        {
            JsonContent content = JsonContent.Create(request);
            HttpResponseMessage response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}github/commit/{userName}/{repoName}", content);
            response.EnsureSuccessStatusCode();

            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse>(message, new JsonSerializerSettings
            {
                Error = OnError
            })!;
            return result;
        }

        #endregion

        private void OnError(object? sender, Newtonsoft.Json.Serialization.ErrorEventArgs e)
        {
            _errorObserver.AddError(e.ErrorContext.Error);
        }
    }
}
