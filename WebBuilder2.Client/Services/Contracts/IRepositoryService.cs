using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Services.Contracts
{
    public interface IRepositoryService
    {
        Task<List<Repository>> GetRepositoriesAsync();
        Task<Repository?> GetSingleRepositoryAsync(long id);
        Task<Repository?> AddRepositoryAsync(Repository repository);
        Task<Repository?> AddRepositoriesAsync(IEnumerable<Repository> repositories);
        Task<Repository?> SoftDeleteRepositoryAsync(Repository repository);
        Task<Repository?> UpdateRepositoryAsync(Repository repository);
    }
}
