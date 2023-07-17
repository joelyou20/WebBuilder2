using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Services.Contracts
{
    public interface ISiteService
    {
        Task<List<Site>?> GetSitesAsync();
        Task<Site?> GetSingleSiteAsync(long id);
        Task AddSiteAsync(Site site);
    }
}
