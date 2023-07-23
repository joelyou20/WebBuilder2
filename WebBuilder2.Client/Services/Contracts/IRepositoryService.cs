using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Services.Contracts
{
    public interface IRepositoryService
    {
        Task<List<Repository>> GetRepositoriesAsync();
        Task<Repository?> GetSingleRepositoryAsync(long id);
        Task<Repository?> AddRepositoryAsync(Repository repository);
        Task<Repository?> SoftDeleteSiteAsync(Repository repository);
    }
}
