using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients;

public class SiteRepositoryClient : ClientBase<SiteRepositoryModel>, ISiteRepositoryClient
{
    public SiteRepositoryClient(HttpClient httpClient) : base(httpClient, "siteRepository") { }

    public async Task<SiteRepositoryModel> AddSiteRepositoryAsync(SiteRepositoryModel siteRepository) => await AddAsync(siteRepository);
    public async Task<IEnumerable<SiteRepositoryModel>> AddRangeSiteRepositoryAsync(IEnumerable<SiteRepositoryModel> siteRepositories) => await AddRangeAsync(siteRepositories);
    public async Task<SiteRepositoryModel> GetSingleSiteRepositoryAsync(long id) => await GetSingleAsync(id);
    public async Task<IEnumerable<SiteRepositoryModel>> GetSiteRepositoriesAsync(Dictionary<string, string>? filter = null) => await GetAsync(filter: filter);
    public async Task<SiteRepositoryModel> SoftDeleteSiteRepositoryAsync(SiteRepositoryModel siteRepository) => await SoftDeleteAsync(siteRepository);
    public async Task<IEnumerable<SiteRepositoryModel>> SoftDeleteRangeSiteRepositoryAsync(IEnumerable<SiteRepositoryModel> siteRepositories) => await SoftDeleteRangeAsync(siteRepositories);
    public async Task<SiteRepositoryModel> UpdateSiteRepositoryAsync(SiteRepositoryModel siteRepository) => await UpdateAsync(siteRepository);
}
