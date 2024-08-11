using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients.Contracts;

public interface ISiteRepositoryClient
{
    Task<SiteRepositoryModel> AddSiteRepositoryAsync(SiteRepositoryModel siteRepository);
    Task<IEnumerable<SiteRepositoryModel>> AddRangeSiteRepositoryAsync(IEnumerable<SiteRepositoryModel> siteRepositories);
    Task<SiteRepositoryModel> GetSingleSiteRepositoryAsync(long id);
    Task<IEnumerable<SiteRepositoryModel>> GetSiteRepositoriesAsync(Dictionary<string, string>? filter = null);
    Task<SiteRepositoryModel> SoftDeleteSiteRepositoryAsync(SiteRepositoryModel siteRepository);
    Task<IEnumerable<SiteRepositoryModel>> SoftDeleteRangeSiteRepositoryAsync(IEnumerable<SiteRepositoryModel> siteRepositories);
    Task<SiteRepositoryModel> UpdateSiteRepositoryAsync(SiteRepositoryModel siteRepository);
}
