using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Observers.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Services
{
    public class SiteService : ServiceBase, ISiteService
    {
        private ISiteClient _siteClient;

        public SiteService(ISiteClient siteClient, IErrorObserver errorObserver, ILogService logService) : base(errorObserver, logService)
        {
            _siteClient = siteClient;
        }

        public async Task<List<SiteModel>?> GetSitesAsync(Dictionary<string, string>? filter = null)
        {
            IEnumerable<SiteModel>? result = await ExecuteAsync(() => _siteClient.GetSitesAsync(filter));

            return result?.ToList();
        }

        public async Task<SiteModel?> GetSingleSiteAsync(long id)
        {
            IEnumerable<SiteModel>? result = await ExecuteAsync(() => _siteClient.GetSingleSiteAsync(id));

            return result?.SingleOrDefault();
        }

        public async Task<SiteModel?> AddSiteAsync(SiteModel site)
        {
            IEnumerable<SiteModel>? result = await ExecuteAsync(() => _siteClient.AddSiteAsync(site));

            return result?.SingleOrDefault();
        }

        public async Task<SiteModel?> SoftDeleteSiteAsync(SiteModel site)
        {
            IEnumerable<SiteModel>? result = await ExecuteAsync(() => _siteClient.AddSiteAsync(site));

            return result?.SingleOrDefault();
        }

        public async Task<SiteModel?> UpdateSiteAsync(SiteModel site)
        {
            IEnumerable<SiteModel>? result = await ExecuteAsync(() => _siteClient.UpdateSiteAsync(site));

            return result?.SingleOrDefault();
        }
    }
}
