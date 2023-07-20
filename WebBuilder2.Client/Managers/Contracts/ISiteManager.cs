using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Managers.Contracts;

public interface ISiteManager
{
    Task CreateSiteAsync(CreateSiteRequest createSiteRequest);
}
