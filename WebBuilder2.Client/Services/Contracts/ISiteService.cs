﻿using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Services.Contracts
{
    public interface ISiteService
    {
        Task<List<Site>?> GetSitesAsync();
        Task<Site?> GetSingleSiteAsync(long id);
        Task AddSiteAsync(Site site);
        Task<Site?> SoftDeleteSiteAsync(Site site);
    }
}
