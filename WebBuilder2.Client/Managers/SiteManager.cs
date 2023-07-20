using WebBuilder2.Client.Managers.Contracts;
using WebBuilder2.Client.Services;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Managers;

public class SiteManager : ISiteManager
{
    private ISiteService _siteService;

    public SiteManager(ISiteService siteService)
    {
        _siteService = siteService;
    }

    public async Task CreateSiteAsync(CreateSiteRequest createSiteRequest)
    {
        // TODO: WHERE ALL THE MAGIC WILL HAPPEN

        Site site = new()
        {
            Name = createSiteRequest.SiteName
        };
        await _siteService.AddSiteAsync(site);
    }
}
