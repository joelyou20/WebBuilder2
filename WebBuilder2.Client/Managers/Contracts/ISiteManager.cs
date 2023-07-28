using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Managers.Contracts;

public interface ISiteManager
{
    Task<SiteModel?> CreateSiteAsync(CreateSiteRequest createSiteRequest);
}
