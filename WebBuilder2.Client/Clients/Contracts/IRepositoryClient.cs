using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients.Contracts
{
    public interface IRepositoryClient
    {
        Task<ValidationResponse<RepositoryModel>> GetRepositoriesAsync();
        Task<ValidationResponse<RepositoryModel>> GetSingleRepositoryAsync(long id);
        Task<ValidationResponse<RepositoryModel>> SoftDeleteRepositoryAsync(RepositoryModel repository);
        Task<ValidationResponse<RepositoryModel>> AddRepositoryAsync(RepositoryModel repository);
        Task<ValidationResponse<RepositoryModel>> AddRepositoriesAsync(IEnumerable<RepositoryModel> repositories);
        Task<ValidationResponse<RepositoryModel>> UpdateRepositoryAsync(RepositoryModel repository);
    }
}
