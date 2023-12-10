using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Services.Contracts
{
    public interface ISiteService
    {
        Task<List<SiteModel>?> GetSitesAsync(Dictionary<string, string>? filter = null);
        Task<SiteModel?> GetSingleSiteAsync(long id);
        Task<SiteModel?> AddSiteAsync(SiteModel site);
        Task<SiteModel?> SoftDeleteSiteAsync(SiteModel site);
    }
}
