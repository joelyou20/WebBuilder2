using System.Net.Http;
using System.Net.Http.Json;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Services
{
    public class SiteService : ISiteService
    {
        private IDatabaseClient _databaseClient;
        private ISiteClient _siteClient;

        public SiteService(IDatabaseClient databaseClient, ISiteClient siteClient) 
        {
            _databaseClient = databaseClient;
            _siteClient = siteClient;
        }

        //public async Task<List<Site>?> GetSitesAsync() => (await _databaseClient.GetSitesAsync())?.ToList();
        public async Task<List<Site>?> GetSitesAsync() => (await _siteClient.GetSitesAsync())?.ToList();

        public async Task<Site?> GetSingleSiteAsync(int id) => (await _databaseClient.GetSingleSiteAsync(id));
    }
}
