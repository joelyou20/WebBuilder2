using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients.Contracts
{
    public interface ISiteClient
    {
        Task<ValidationResponse<Site>> AddSiteAsync(Site site);
        Task<ValidationResponse<Site>> AddRangeSiteAsync(IEnumerable<Site> sites);
        Task<ValidationResponse<Site>> GetSingleSiteAsync(long id);
        Task<ValidationResponse<Site>> GetSitesAsync(IEnumerable<long>? exclude = null);
        Task<ValidationResponse<Site>> SoftDeleteSiteAsync(Site site);
        Task<ValidationResponse<Site>> SoftDeleteRangeSiteAsync(IEnumerable<Site> sites);
    }
}
