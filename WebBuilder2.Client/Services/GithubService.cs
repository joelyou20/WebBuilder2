using Microsoft.AspNetCore.Components;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Services
{
    public class GithubService : IGithubService
    {
        private IGithubClient _client;
        private NavigationManager _navigationManager;

        public GithubService(IGithubClient client, NavigationManager navigationManager)
        {
            _client = client;
            _navigationManager = navigationManager;
        }

        public async Task<ValidationResponse<RepositoryModel>> GetRepositoriesAsync()
        {
            return await _client.GetRepositoriesAsync();
        }
        public async Task<ValidationResponse<GitIgnoreTemplateResponse>> GetGitIgnoreTemplatesAsync()
        {
            return await _client.GetGitIgnoreTemplatesAsync();
        }
        public async Task<ValidationResponse<GithubProjectLicense>> GetGithubProjectLicensesAsync()
        {
            return await _client.GetGithubProjectLicensesAsync();
        }
        public async Task<ValidationResponse<GithubSecretResponse>> GetSecretsAsync()
        {
            return await _client.GetSecretsAsync();
        }
        public async Task<ValidationResponse> PostAuthenticateAsync(GithubAuthenticationRequest request)
        {
            return await _client.PostAuthenticateAsync(request);
        }
        public async Task<ValidationResponse<RepositoryModel>> PostCreateRepoAsync(RepositoryModel repository)
        {
            ValidationResponse authenticateResponse = await PostAuthenticateAsync(new GithubAuthenticationRequest(""));

            if (authenticateResponse != null && authenticateResponse.IsSuccessful)
            {
                return await _client.PostCreateRepoAsync(repository);
            }
            else
            {
                _navigationManager.NavigateTo($"/github/auth/{Uri.EscapeDataString(_navigationManager.Uri)}");
                return ValidationResponse<RepositoryModel>.NotAuthenticated();
            }
        }
    }
}
