using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Xml.Linq;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients
{
    public class SiteClient : ClientBase<Site>, ISiteClient
    {
        public SiteClient(HttpClient httpClient) : base(httpClient, "site") { }

        public async Task<ValidationResponse<Site>> AddSiteAsync(Site site) => await AddAsync(site);
        public async Task<ValidationResponse<Site>> AddRangeSiteAsync(IEnumerable<Site> sites) => await AddRangeAsync(sites);
        public async Task<ValidationResponse<Site>> GetSingleSiteAsync(long id) => await GetSingleAsync(id);
        public async Task<ValidationResponse<Site>> GetSitesAsync(IEnumerable<long>? exclude = null) => await GetAsync(exclude);
        public async Task<ValidationResponse<Site>> SoftDeleteSiteAsync(Site site) => await SoftDeleteAsync(site);
        public async Task<ValidationResponse<Site>> SoftDeleteRangeSiteAsync(IEnumerable<Site> sites) => await SoftDeleteRangeAsync(sites);
    }
}
