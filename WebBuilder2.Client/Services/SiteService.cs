using System.Net.Http;
using System.Net.Http.Json;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Services
{
    public class SiteService : ISiteService
    {
        private ISiteClient _siteClient;

        public SiteService(ISiteClient siteClient)
        {
            _siteClient = siteClient;
        }
        public async Task<List<Site>?> GetSitesAsync()
        {
            ValidationResponse<Site>? response = await _siteClient.GetSitesAsync();

            if (response == null || !response.IsSuccessful || response.Values == null || !response.Values.Any())
            {
                // Handle error -> response.Message
                return null;
            }

            return response.Values!.ToList();

        }

        public async Task<Site?> GetSingleSiteAsync(long id)
        {
            ValidationResponse<Site>? response = await _siteClient.GetSingleSiteAsync(id);

            if (response == null) return null;

            if(!response.IsSuccessful)
            {
                // Handle error -> response.Message
            }

            return response.Values!.SingleOrDefault();
        }

        public async Task AddSiteAsync(Site site) => await _siteClient.AddSiteAsync(site);

        public async Task<Site?> SoftDeleteSiteAsync(Site site)
        {
            ValidationResponse<Site>? response = await _siteClient.SoftDeleteSiteAsync(site);

            if (response == null) return null;

            if (!response.IsSuccessful)
            {
                // Handle error -> response.Message
            }

            return response.Values!.SingleOrDefault();
        }
    }
}
