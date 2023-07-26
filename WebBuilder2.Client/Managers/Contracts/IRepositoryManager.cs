using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Managers.Contracts;

public interface IRepositoryManager
{
    Task<ValidationResponse<RepositoryModel>> CreateRepoAsync(RepositoryModel repo, SiteModel site);
}
