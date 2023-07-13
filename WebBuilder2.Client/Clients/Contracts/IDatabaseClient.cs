using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Clients.Contracts
{
    public interface IDatabaseClient
    {
        Task<Site?> GetSingleSiteAsync(int id);
        Task<IEnumerable<Site>?> GetSitesAsync();
    }
}
