using Newtonsoft.Json;
using System.Net.Http.Json;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients
{
    public class RepositoryClient(HttpClient httpClient) : ClientBase<RepositoryModel>(httpClient, "repository"), IRepositoryClient
    {
        public async Task<IEnumerable<RepositoryModel>> GetRepositoriesAsync() => await GetAsync();
        public async Task<RepositoryModel> GetSingleRepositoryAsync(long id) => await GetSingleAsync(id);
        public async Task<RepositoryModel> SoftDeleteRepositoryAsync(RepositoryModel repository) => await SoftDeleteAsync(repository);
        public async Task<RepositoryModel> AddRepositoryAsync(RepositoryModel repository) => await AddAsync(repository);
        public async Task<IEnumerable<RepositoryModel>> AddRepositoriesAsync(IEnumerable<RepositoryModel> repositories) => await AddRangeAsync(repositories);
        public async Task<RepositoryModel> UpdateRepositoryAsync(RepositoryModel repository) => await UpdateAsync(repository);
    }
}
