using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Xml.Linq;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients
{
    public class SiteClient : ClientBase<SiteModel>, ISiteClient
    {
        public SiteClient(HttpClient httpClient) : base(httpClient, "site") { }

        public async Task<ValidationResponse<SiteModel>> AddSiteAsync(SiteModel site) => await AddAsync(site);
        public async Task<ValidationResponse<SiteModel>> AddRangeSiteAsync(IEnumerable<SiteModel> sites) => await AddRangeAsync(sites);
        public async Task<ValidationResponse<SiteModel>> GetSingleSiteAsync(long id) => await GetSingleAsync(id);
        public async Task<ValidationResponse<SiteModel>> GetSitesAsync(Dictionary<string, string>? filter = null) => await GetAsync(filter: filter);
        public async Task<ValidationResponse<SiteModel>> SoftDeleteSiteAsync(SiteModel site) => await SoftDeleteAsync(site);
        public async Task<ValidationResponse<SiteModel>> SoftDeleteRangeSiteAsync(IEnumerable<SiteModel> sites) => await SoftDeleteRangeAsync(sites);
        public async Task<ValidationResponse<SiteModel>> UpdateSiteAsync(SiteModel site) => await UpdateAsync(site);
    }
}
