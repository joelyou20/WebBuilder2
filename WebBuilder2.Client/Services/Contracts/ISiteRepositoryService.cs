using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Services.Contracts;

public interface ISiteRepositoryService
{
    Task<List<SiteRepositoryModel>?> GetSiteRepositoriesAsync(Dictionary<string, string>? filter = null);
    Task<SiteRepositoryModel?> GetSingleSiteRepositoryAsync(long id);
    Task<SiteRepositoryModel?> AddSiteRepositoryAsync(SiteRepositoryModel siteRepository);
    Task<SiteRepositoryModel?> SoftDeleteSiteRepositoryAsync(SiteRepositoryModel siteRepository);
    Task<SiteRepositoryModel?> UpdateSiteRepositoryAsync(SiteRepositoryModel siteRepository);
}
