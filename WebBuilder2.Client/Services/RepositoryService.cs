using WebBuilder2.Client.Clients;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Services
{
    public class RepositoryService : IRepositoryService
    {
        private IRepositoryClient _client;

        public RepositoryService(IRepositoryClient client)
        {
            _client = client;
        }

        public async Task<List<Repository>> GetRepositoriesAsync()
        {
            ValidationResponse<Repository> response = await _client.GetRepositoriesAsync();

            if (response == null || !response.IsSuccessful)
            {
                throw new Exception(response?.Errors.First().Message ?? "Failed to get repository Data");
            }

            return response.GetValues();

        }

        public async Task<Repository?> GetSingleRepositoryAsync(long id)
        {
            ValidationResponse<Repository>? response = await _client.GetSingleRepositoryAsync(id);

            if (response == null) return null;

            if (!response.IsSuccessful)
            {
                throw new Exception(response?.Message ?? "Failed to get repository Data");
            }

            return response.GetValues().SingleOrDefault();
        }

        public async Task<Repository?> AddRepositoryAsync(Repository repository)
        {
            ValidationResponse<Repository> response = await _client.AddRepositoryAsync(repository);

            if (response == null) return null;

            if (!response.IsSuccessful)
            {
                throw new Exception(response?.Message ?? "Failed to add repository Data");
            }

            return response.GetValues().SingleOrDefault();
        }

        public async Task<Repository?> AddRepositoriesAsync(IEnumerable<Repository> repositories)
        {
            ValidationResponse<Repository> response = await _client.AddRepositoriesAsync(repositories);

            if (response == null) return null;

            if (!response.IsSuccessful)
            {
                throw new Exception(response?.Message ?? "Failed to add repository Data");
            }

            return response.GetValues().SingleOrDefault();
        }

        public async Task<Repository?> SoftDeleteRepositoryAsync(Repository repository)
        {
            ValidationResponse<Repository> response = await _client.SoftDeleteRepositoryAsync(repository);

            if (response == null) return null;

            if (!response.IsSuccessful)
            {
                throw new Exception(response?.Message ?? "Failed to add repository");
            }

            return response.GetValues().SingleOrDefault();
        }

        public async Task<Repository?> UpdateRepositoryAsync(Repository repository)
        {
            ValidationResponse<Repository> response = await _client.UpdateRepositoryAsync(repository);

            if (response == null) return null;

            if (!response.IsSuccessful)
            {
                throw new Exception(response?.Message ?? "Failed to update repository");
            }

            return response.GetValues().SingleOrDefault();
        }
    }
}
