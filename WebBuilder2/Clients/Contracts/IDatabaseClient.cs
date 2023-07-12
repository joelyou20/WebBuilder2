using WebBuilder2.Models;

namespace WebBuilder2.Clients.Contracts
{
    public interface IDatabaseClient
    {
        Task<IEnumerable<Site>?> GetSitesAsync();
    }
}
