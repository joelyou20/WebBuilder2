using WebBuilder2.Client.Clients;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Observers.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Services
{
    public class RepositoryService : ServiceBase, IRepositoryService
    {
        private IRepositoryClient _client;

        public RepositoryService(IRepositoryClient client, IErrorObserver errorObserver, ILogService logService) : base(errorObserver, logService)
        {
            _client = client;
        }

        public async Task<List<RepositoryModel>?> GetRepositoriesAsync()
        {
            IEnumerable<RepositoryModel>? result = await ExecuteAsync(_client.GetRepositoriesAsync);

            return result?.ToList();
        }

        public async Task<RepositoryModel?> GetSingleRepositoryAsync(long id)
        {
            IEnumerable<RepositoryModel>? result = await ExecuteAsync(() => _client.GetSingleRepositoryAsync(id));

            return result?.SingleOrDefault();
        }

        public async Task<RepositoryModel?> AddRepositoryAsync(RepositoryModel repository)
        {
            IEnumerable<RepositoryModel>? result = await ExecuteAsync(() => _client.AddRepositoryAsync(repository));

            return result?.SingleOrDefault();
        }

        public async Task<List<RepositoryModel>?> AddRepositoriesAsync(IEnumerable<RepositoryModel> repositories)
        {
            IEnumerable<RepositoryModel>? result = await ExecuteAsync(() => _client.AddRepositoriesAsync(repositories));

            return result?.ToList();
        }

        public async Task<RepositoryModel?> SoftDeleteRepositoryAsync(RepositoryModel repository)
        {
            IEnumerable<RepositoryModel>? result = await ExecuteAsync(() => _client.SoftDeleteRepositoryAsync(repository));

            return result?.SingleOrDefault();
        }

        public async Task<RepositoryModel?> UpdateRepositoryAsync(RepositoryModel repository)
        {
            IEnumerable<RepositoryModel>? result = await ExecuteAsync(() => _client.UpdateRepositoryAsync(repository));

            return result?.SingleOrDefault();
        }
    }
}
