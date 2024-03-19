using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients;

public class SiteRepositoryClient : ClientBase<SiteRepositoryModel>, ISiteRepositoryClient
{
    public SiteRepositoryClient(HttpClient httpClient) : base(httpClient, "siteRepository") { }

    public async Task<ValidationResponse<SiteRepositoryModel>> AddSiteRepositoryAsync(SiteRepositoryModel siteRepository) => await AddAsync(siteRepository);
    public async Task<ValidationResponse<SiteRepositoryModel>> AddRangeSiteRepositoryAsync(IEnumerable<SiteRepositoryModel> siteRepositories) => await AddRangeAsync(siteRepositories);
    public async Task<ValidationResponse<SiteRepositoryModel>> GetSingleSiteRepositoryAsync(long id) => await GetSingleAsync(id);
    public async Task<ValidationResponse<SiteRepositoryModel>> GetSiteRepositoriesAsync(Dictionary<string, string>? filter = null) => await GetAsync(filter: filter);
    public async Task<ValidationResponse<SiteRepositoryModel>> SoftDeleteSiteRepositoryAsync(SiteRepositoryModel siteRepository) => await SoftDeleteAsync(siteRepository);
    public async Task<ValidationResponse<SiteRepositoryModel>> SoftDeleteRangeSiteRepositoryAsync(IEnumerable<SiteRepositoryModel> siteRepositories) => await SoftDeleteRangeAsync(siteRepositories);
    public async Task<ValidationResponse<SiteRepositoryModel>> UpdateSiteRepositoryAsync(SiteRepositoryModel siteRepository) => await UpdateAsync(siteRepository);
}
