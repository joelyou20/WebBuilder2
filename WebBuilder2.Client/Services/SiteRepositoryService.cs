using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Observers.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Services;

public class SiteRepositoryService : ServiceBase, ISiteRepositoryService
{
    private ISiteRepositoryClient _siteRepositoryClient;

    public SiteRepositoryService(ISiteRepositoryClient siteRepositoryClient, IErrorObserver errorObserver, ILogService logService) : base(errorObserver, logService)
    {
        _siteRepositoryClient = siteRepositoryClient;
    }

    public async Task<List<SiteRepositoryModel>?> GetSiteRepositoriesAsync(Dictionary<string, string>? filter = null)
    {
        IEnumerable<SiteRepositoryModel>? result = await ExecuteAsync(() => _siteRepositoryClient.GetSiteRepositoriesAsync(filter));

        return result?.ToList();
    }

    public async Task<SiteRepositoryModel?> GetSingleSiteRepositoryAsync(long id)
    {
        IEnumerable<SiteRepositoryModel>? result = await ExecuteAsync(() => _siteRepositoryClient.GetSingleSiteRepositoryAsync(id));

        return result?.SingleOrDefault();
    }

    public async Task<SiteRepositoryModel?> AddSiteRepositoryAsync(SiteRepositoryModel siteRepository)
    {
        IEnumerable<SiteRepositoryModel>? result = await ExecuteAsync(() => _siteRepositoryClient.AddSiteRepositoryAsync(siteRepository));

        return result?.SingleOrDefault();
    }

    public async Task<SiteRepositoryModel?> SoftDeleteSiteRepositoryAsync(SiteRepositoryModel siteRepository)
    {
        IEnumerable<SiteRepositoryModel>? result = await ExecuteAsync(() => _siteRepositoryClient.SoftDeleteSiteRepositoryAsync(siteRepository));

        return result?.SingleOrDefault();
    }

    public async Task<SiteRepositoryModel?> UpdateSiteRepositoryAsync(SiteRepositoryModel siteRepository)
    {
        IEnumerable<SiteRepositoryModel>? result = await ExecuteAsync(() => _siteRepositoryClient.UpdateSiteRepositoryAsync(siteRepository));

        return result?.SingleOrDefault();
    }
}
