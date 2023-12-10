using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Services.Contracts
{
    public interface IRepositoryService
    {
        Task<List<RepositoryModel>?> GetRepositoriesAsync();
        Task<RepositoryModel?> GetSingleRepositoryAsync(long id);
        Task<RepositoryModel?> AddRepositoryAsync(RepositoryModel repository);
        Task<List<RepositoryModel>?> AddRepositoriesAsync(IEnumerable<RepositoryModel> repositories);
        Task<RepositoryModel?> SoftDeleteRepositoryAsync(RepositoryModel repository);
        Task<RepositoryModel?> UpdateRepositoryAsync(RepositoryModel repository);
    }
}
