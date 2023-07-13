using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Clients.Contracts
{
    public interface ISiteClient
    {
        Task<IEnumerable<Site>?> GetSitesAsync();
    }
}
