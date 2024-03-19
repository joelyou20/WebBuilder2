using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients.Contracts;

public interface ISiteRepositoryClient
{
    Task<ValidationResponse<SiteRepositoryModel>> AddSiteRepositoryAsync(SiteRepositoryModel siteRepository);
    Task<ValidationResponse<SiteRepositoryModel>> AddRangeSiteRepositoryAsync(IEnumerable<SiteRepositoryModel> siteRepositories);
    Task<ValidationResponse<SiteRepositoryModel>> GetSingleSiteRepositoryAsync(long id);
    Task<ValidationResponse<SiteRepositoryModel>> GetSiteRepositoriesAsync(Dictionary<string, string>? filter = null);
    Task<ValidationResponse<SiteRepositoryModel>> SoftDeleteSiteRepositoryAsync(SiteRepositoryModel siteRepository);
    Task<ValidationResponse<SiteRepositoryModel>> SoftDeleteRangeSiteRepositoryAsync(IEnumerable<SiteRepositoryModel> siteRepositories);
    Task<ValidationResponse<SiteRepositoryModel>> UpdateSiteRepositoryAsync(SiteRepositoryModel siteRepository);
}
