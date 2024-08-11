using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models.Dtos;

namespace WebBuilder2.Client.Services
{
    public class RepositoryService(IRepositoryClient client) : IRepositoryService
    {
        private readonly IRepositoryClient _client = client;

        public async Task<List<RepositoryModel>?> GetRepositoriesAsync()
        {
            IEnumerable<RepositoryModel>? result = await _client.GetRepositoriesAsync();

            return result?.ToList();
        }

        public async Task<RepositoryModel?> GetSingleRepositoryAsync(long id)
        {
            RepositoryModel? result = await _client.GetSingleRepositoryAsync(id);

            return result;
        }

        public async Task<RepositoryModel?> AddRepositoryAsync(RepositoryModel repository)
        {
            RepositoryModel? result = await _client.AddRepositoryAsync(repository);

            return result;
        }

        public async Task<List<RepositoryModel>?> AddRepositoriesAsync(IEnumerable<RepositoryModel> repositories)
        {
            IEnumerable<RepositoryModel>? result = await _client.AddRepositoriesAsync(repositories);

            return result?.ToList();
        }

        public async Task<RepositoryModel?> SoftDeleteRepositoryAsync(RepositoryModel repository)
        {
            RepositoryModel? result = await _client.SoftDeleteRepositoryAsync(repository);

            return result;
        }

        public async Task<RepositoryModel?> UpdateRepositoryAsync(RepositoryModel repository)
        {
            RepositoryModel? result = await _client.UpdateRepositoryAsync(repository);

            return result;
        }
    }
}
