using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients.Contracts
{
    public interface ISiteClient
    {
        Task AddSiteAsync(Site site);
        Task<ValidationResponse<Site>?> GetSingleSiteAsync(long id);
        Task<ValidationResponse<Site>?> GetSitesAsync();
        Task<ValidationResponse<Site>?> SoftDeleteSiteAsync(Site site);
    }
}
