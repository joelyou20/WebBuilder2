using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Clients.Contracts
{
    public interface ISiteClient
    {
        Task<Site?> GetSingleSiteAsync(string name);
        Task<IEnumerable<Site>?> GetSitesAsync();
    }
}
