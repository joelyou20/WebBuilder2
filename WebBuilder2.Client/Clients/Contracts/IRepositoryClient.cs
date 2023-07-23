using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients.Contracts
{
    public interface IRepositoryClient
    {
        Task<ValidationResponse<Repository>> GetRepositoriesAsync();
        Task<ValidationResponse<Repository>> GetSingleRepositoryAsync(long id);
        Task<ValidationResponse<Repository>> SoftDeleteRepositoryAsync(Repository repository);
        Task<ValidationResponse<Repository>> AddRepositoryAsync(Repository repository);
    }
}
