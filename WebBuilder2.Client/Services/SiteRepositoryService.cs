using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Observers.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models.Dtos;

namespace WebBuilder2.Client.Services;

public class SiteRepositoryService(ISiteRepositoryClient siteRepositoryClient) : ISiteRepositoryService
{
    private readonly ISiteRepositoryClient _siteRepositoryClient = siteRepositoryClient;

    public async Task<List<SiteRepositoryModel>?> GetSiteRepositoriesAsync(Dictionary<string, string>? filter = null)
    {
        IEnumerable<SiteRepositoryModel>? result = await _siteRepositoryClient.GetSiteRepositoriesAsync(filter);

        return result?.ToList();
    }

    public async Task<SiteRepositoryModel?> GetSingleSiteRepositoryAsync(long id)
    {
        SiteRepositoryModel? result = await _siteRepositoryClient.GetSingleSiteRepositoryAsync(id);

        return result;
    }

    public async Task<SiteRepositoryModel?> AddSiteRepositoryAsync(SiteRepositoryModel siteRepository)
    {
        SiteRepositoryModel? result = await _siteRepositoryClient.AddSiteRepositoryAsync(siteRepository);

        return result;
    }

    public async Task<SiteRepositoryModel?> SoftDeleteSiteRepositoryAsync(SiteRepositoryModel siteRepository)
    {
        SiteRepositoryModel? result = await _siteRepositoryClient.SoftDeleteSiteRepositoryAsync(siteRepository);

        return result;
    }

    public async Task<SiteRepositoryModel?> UpdateSiteRepositoryAsync(SiteRepositoryModel siteRepository)
    {
        SiteRepositoryModel? result = await _siteRepositoryClient.UpdateSiteRepositoryAsync(siteRepository);

        return result;
    }
}
