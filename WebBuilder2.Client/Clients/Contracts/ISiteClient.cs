using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients.Contracts;

public interface ISiteClient
{
    Task<ValidationResponse<SiteModel>> AddSiteAsync(SiteModel site);
    Task<ValidationResponse<SiteModel>> AddRangeSiteAsync(IEnumerable<SiteModel> sites);
    Task<ValidationResponse<SiteModel>> GetSingleSiteAsync(long id);
    Task<ValidationResponse<SiteModel>> GetSitesAsync(IEnumerable<long>? exclude = null);
    Task<ValidationResponse<SiteModel>> SoftDeleteSiteAsync(SiteModel site);
    Task<ValidationResponse<SiteModel>> SoftDeleteRangeSiteAsync(IEnumerable<SiteModel> sites);
}
