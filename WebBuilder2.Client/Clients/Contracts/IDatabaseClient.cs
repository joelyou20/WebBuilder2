using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Clients.Contracts
{
    public interface IDatabaseClient
    {
        Task<SiteRepository?> GetSingleSiteRepositoryBySiteNameAsync(string siteName);
        Task<SiteRepository?> GetSingleSiteRepositoryByRepositoryIdAsync(long repoId);
        Task<IEnumerable<SiteRepository>?> GetSiteRepositoriesAsync();
    }
}
