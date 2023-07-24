using Newtonsoft.Json;
using System.Net.Http.Json;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients
{
    public class RepositoryClient : ClientBase<Repository>, IRepositoryClient
    {
        public RepositoryClient(HttpClient httpClient) : base(httpClient, "repository") { }

        public async Task<ValidationResponse<Repository>> GetRepositoriesAsync() => await GetAsync();
        public async Task<ValidationResponse<Repository>> GetSingleRepositoryAsync(long id) => await GetSingleAsync(id);
        public async Task<ValidationResponse<Repository>> SoftDeleteRepositoryAsync(Repository repository) => await SoftDeleteAsync(repository);
        public async Task<ValidationResponse<Repository>> AddRepositoryAsync(Repository repository) => await AddAsync(repository);
    }
}
