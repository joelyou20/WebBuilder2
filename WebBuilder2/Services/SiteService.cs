using System.Net.Http;
using System.Net.Http.Json;
using WebBuilder2.Clients.Contracts;
using WebBuilder2.Models;
using WebBuilder2.Services.Contracts;

namespace WebBuilder2.Services
{
    public class SiteService : ISiteService
    {
        private IDatabaseClient _databaseClient;

        public SiteService(IDatabaseClient databaseClient) 
        {
            _databaseClient = databaseClient;
        }

        public async Task<List<Site>?> GetSitesAsync() => (await _databaseClient.GetSitesAsync())?.ToList();
    }
}
