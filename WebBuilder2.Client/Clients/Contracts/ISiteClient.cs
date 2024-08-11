using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients.Contracts;

public interface ISiteClient
{
    Task<SiteModel> AddSiteAsync(SiteModel site);
    Task<IEnumerable<SiteModel>> AddRangeSiteAsync(IEnumerable<SiteModel> sites);
    Task<SiteModel> GetSingleSiteAsync(long id);
    Task<IEnumerable<SiteModel>> GetSitesAsync(Dictionary<string, string>? filter = null);
    Task<SiteModel> SoftDeleteSiteAsync(SiteModel site);
    Task<IEnumerable<SiteModel>> SoftDeleteRangeSiteAsync(IEnumerable<SiteModel> sites);
    Task<SiteModel> UpdateSiteAsync(SiteModel site);
}
