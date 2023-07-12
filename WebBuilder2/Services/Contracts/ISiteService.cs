using WebBuilder2.Models;

namespace WebBuilder2.Services.Contracts
{
    public interface ISiteService
    {
        Task<List<Site>?> GetSitesAsync();
    }
}
