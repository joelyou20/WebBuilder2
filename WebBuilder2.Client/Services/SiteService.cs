using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models.Dtos;

namespace WebBuilder2.Client.Services
{
    public class SiteService(ISiteClient siteClient) : ISiteService
    {
        private readonly ISiteClient _siteClient = siteClient;

        public async Task<List<SiteModel>?> GetSitesAsync(Dictionary<string, string>? filter = null)
        {
            IEnumerable<SiteModel>? result = await _siteClient.GetSitesAsync(filter);

            return result?.ToList();
        }

        public async Task<SiteModel?> GetSingleSiteAsync(long id)
        {
            SiteModel? result = await _siteClient.GetSingleSiteAsync(id);

            return result;
        }

        public async Task<SiteModel?> AddSiteAsync(SiteModel site)
        {
            SiteModel? result = await _siteClient.AddSiteAsync(site);

            return result;
        }

        public async Task<SiteModel?> SoftDeleteSiteAsync(SiteModel site)
        {
            SiteModel? result = await _siteClient.SoftDeleteSiteAsync(site);

            return result;
        }

        public async Task<SiteModel?> UpdateSiteAsync(SiteModel site)
        {
            SiteModel? result = await _siteClient.UpdateSiteAsync(site);

            return result;
        }
    }
}
