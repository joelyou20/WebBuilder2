using System.Net.Http;
using System.Net.Http.Json;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Managers;
using WebBuilder2.Client.Managers.Contracts;
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

        public async Task<List<Site>> GetSitesAsync()
        {
            ValidationResponse<Site> response = await _siteClient.GetSitesAsync();

            if (response == null || !response.IsSuccessful)
            {
                throw new Exception(response?.Message ?? "Failed to get site Data");
            }

            return response.GetValues();

        }

        public async Task<Site?> GetSingleSiteAsync(long id)
        {
            ValidationResponse<Site>? response = await _siteClient.GetSingleSiteAsync(id);

            if (response == null) return null;

            if(!response.IsSuccessful)
            {
                throw new Exception(response?.Message ?? "Failed to get site Data");
            }

            return response.GetValues().SingleOrDefault();
        }

        public async Task<Site?> AddSiteAsync(Site site)
        {
            ValidationResponse<Site> response = await _siteClient.AddSiteAsync(site);

            if (response == null) return null;

            if (!response.IsSuccessful)
            {
                throw new Exception(response?.Message ?? "Failed to add site Data");
            }

            return response.GetValues().SingleOrDefault();
        }

        public async Task<Site?> SoftDeleteSiteAsync(Site site)
        {
            ValidationResponse<Site> response = await _siteClient.SoftDeleteSiteAsync(site);

            if (response == null) return null;

            if (!response.IsSuccessful)
            {
                throw new Exception(response?.Message ?? "Failed to add site");
            }

            return response.GetValues().SingleOrDefault();
        }
    }
}
