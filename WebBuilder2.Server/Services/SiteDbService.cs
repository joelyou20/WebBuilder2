﻿using Microsoft.EntityFrameworkCore;
using WebBuilder2.Server.Data;
using WebBuilder2.Server.Data.Models;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services
{
    public class SiteDbService : ISiteDbService
    {
        #region Property
        private readonly AppDbContext _appDBContext;
        #endregion

        #region Constructor
        public SiteDbService(AppDbContext appDBContext)
        {
            _appDBContext = appDBContext;
        }
        #endregion

        #region Get Site by Id
        public async Task<ValidationResponse<Site>> GetSingleAsync(long id)
        {
            SiteDTO? dto = await _appDBContext.Sites
                .FirstOrDefaultAsync(c => c.Id.Equals(id) && c.DeletedDateTime == null);

            return dto == null ? ValidationResponse<Site>.Failure(null) : ValidationResponse<Site>.Success(new List<Site> { dto!.FromDto() });
        }
        #endregion

        #region Get List of Sites
        public async Task<ValidationResponse<Site>> GetAllAsync()
        {
            var sites = (await _appDBContext.Sites.ToListAsync())
                .Select(site => site.FromDto())
                .Where(site => site.DeletedDateTime == null);
            return sites.Any() ? 
                ValidationResponse<Site>.Success(sites) :
                ValidationResponse<Site>.Failure(sites);
        }
        #endregion

        #region Insert Site
        public async Task<ValidationResponse<Site>> InsertAsync(Site value)
        {
            ValidationResponse<Site> response = await GetSingleAsync(value.Id);

            if (response.IsSuccessful) return ValidationResponse<Site>.EntityAlreadyExists(new List<Site> { value });

            await _appDBContext.Sites.AddAsync(ToDto(value));
            var result = await _appDBContext.SaveChangesAsync();

            return result == 0 ? ValidationResponse<Site>.Failure(new List<Site> { value }) : ValidationResponse<Site>.Success(new List<Site> { value });
        }
        #endregion

        #region Update Site
        public async Task<ValidationResponse<Site>> UpdateAsync(Site value)
        {
            _appDBContext.Sites.Update(ToDto(value));
            var result = await _appDBContext.SaveChangesAsync();
            return result == 0 ? ValidationResponse<Site>.Failure(new List<Site> { value }) : ValidationResponse<Site>.Success(new List<Site> { value });
        }
        #endregion

        #region Upsert Site
        public async Task<ValidationResponse<Site>> UpsertAsync(Site value)
        {
            ValidationResponse<Site> response = await GetSingleAsync(value.Id);

            if (response.IsSuccessful) _appDBContext.Sites.Update(ToDto(value));
            else await _appDBContext.Sites.AddAsync(ToDto(value));

            var result = await _appDBContext.SaveChangesAsync();

            return result == 0 ? ValidationResponse<Site>.Failure(new List<Site> { value }) : ValidationResponse<Site>.Success(new List<Site> { value });
        }
        #endregion

        #region DeleteSite
        public async Task<ValidationResponse<Site>> DeleteAsync(Site value)
        {
            _appDBContext.Remove(ToDto(value));
            var result = await _appDBContext.SaveChangesAsync();
            return result == 0 ? 
                ValidationResponse<Site>.Failure(new List<Site> { value }) : 
                ValidationResponse<Site>.Success(new List<Site> { value });
        }
        #endregion

        public SiteDTO ToDto(Site site) => new()
        {
            Id = site.Id,
            Name = site.Name,
            CreatedDateTime = site.CreatedDateTime,
            ModifiedDateTime = site.ModifiedDateTime,
            DeletedDateTime = site.DeletedDateTime
        };

        public async Task<ValidationResponse<Site>> SoftDeleteAsync(Site value)
        {
            value.DeletedDateTime = DateTime.UtcNow;
            _appDBContext.Sites.Update(ToDto(value));
            var result = await _appDBContext.SaveChangesAsync();
            return result == 0 ? 
                ValidationResponse<Site>.Failure(new List<Site> { value }) : 
                ValidationResponse<Site>.Success(new List<Site> { value });
        }
    }
}
