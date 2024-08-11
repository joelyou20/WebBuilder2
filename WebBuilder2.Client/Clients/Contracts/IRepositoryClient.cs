using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients.Contracts;

public interface IRepositoryClient
{
    Task<IEnumerable<RepositoryModel>> GetRepositoriesAsync();
    Task<RepositoryModel> GetSingleRepositoryAsync(long id);
    Task<RepositoryModel> SoftDeleteRepositoryAsync(RepositoryModel repository);
    Task<RepositoryModel> AddRepositoryAsync(RepositoryModel repository);
    Task<IEnumerable<RepositoryModel>> AddRepositoriesAsync(IEnumerable<RepositoryModel> repositories);
    Task<RepositoryModel> UpdateRepositoryAsync(RepositoryModel repository);
}
