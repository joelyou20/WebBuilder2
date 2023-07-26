using Newtonsoft.Json;
using System.Net.Http.Json;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients
{
    public class RepositoryClient : ClientBase<RepositoryModel>, IRepositoryClient
    {
        public RepositoryClient(HttpClient httpClient) : base(httpClient, "repository") { }

        public async Task<ValidationResponse<RepositoryModel>> GetRepositoriesAsync() => await GetAsync();
        public async Task<ValidationResponse<RepositoryModel>> GetSingleRepositoryAsync(long id) => await GetSingleAsync(id);
        public async Task<ValidationResponse<RepositoryModel>> SoftDeleteRepositoryAsync(RepositoryModel repository) => await SoftDeleteAsync(repository);
        public async Task<ValidationResponse<RepositoryModel>> AddRepositoryAsync(RepositoryModel repository) => await AddAsync(repository);
        public async Task<ValidationResponse<RepositoryModel>> AddRepositoriesAsync(IEnumerable<RepositoryModel> repositories) => await AddRangeAsync(repositories);
        public async Task<ValidationResponse<RepositoryModel>> UpdateRepositoryAsync(RepositoryModel repository) => await UpdateAsync(repository);
    }
}
