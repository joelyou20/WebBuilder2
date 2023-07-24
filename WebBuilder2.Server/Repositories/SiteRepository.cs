using Microsoft.EntityFrameworkCore;
using WebBuilder2.Server.Data;
using WebBuilder2.Server.Data.Models;
using WebBuilder2.Shared.Models;
using WebBuilder2.Server.Repositories.Contracts;
using System.Linq;

namespace WebBuilder2.Server.Repositories
{
    public class SiteRepository : ISiteRepository
    {
        private readonly AppDbContext _db;
        private readonly ILogger<SiteRepository> _logger;

        public SiteRepository(AppDbContext db, ILogger<SiteRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public IQueryable<Site>? Get(IEnumerable<long>? exclude = null)
        {
            try
            {
                var query = _db.Sites.Where(s => s.DeletedDateTime == null);

                if (exclude != null) query = query.Where(s => !exclude.Any(e => s.Id == e));

                return query.Select(s => s.FromDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve sites from database");
                return null;
            }
        }

        public void AddRange(IEnumerable<Site> values)
        {
            try
            {
                if (!values.Any()) return;
                _db.AddRange(values);
                var result = _db.SaveChanges();
                if (result <= 0) throw new DbUpdateException("Failed to save changes.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Failed to add sites to database");
            }
        }

        public void UpdateRange(IEnumerable<Site> values)
        {
            try
            {
                var dtos = values.Select(ToDto);
                _db.Sites.UpdateRange(dtos);
                var result = _db.SaveChanges();
                if (result <= 0) throw new DbUpdateException("Failed to save changes.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Failed to retrieve sites from database");
            }
        }

        public void UpsertRange(IEnumerable<Site> values)
        {
            try
            {
                List<Site> existingValues = Get()?.Where(x => values.Select(y => y.Id == x.Id).Any()).ToList() ?? new List<Site>();
                List<Site> newValues = values.Where(x => !existingValues.Select(y => y.Id == x.Id).Any()).ToList();

                if (existingValues.Any()) UpdateRange(values);
                if (newValues.Any()) AddRange(newValues);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Failed to upsert sites");
            }
        }

        public void DeleteRange(IEnumerable<Site> values)
        {
            try
            {
                var dtos = values.Select(ToDto);
                _db.RemoveRange(dtos);
                var result = _db.SaveChanges();
                if (result <= 0) throw new DbUpdateException("Failed to save changes.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Failed to delete sites from database");
            }
        }

        public void SoftDeleteRange(IEnumerable<Site> values)
        {
            try
            {
                var softDeletedValues = values.Select(x =>
                {
                    var copy = ToDto(x);
                    copy.DeletedDateTime = DateTime.UtcNow;
                    return copy;
                });
                _db.Sites.UpdateRange(softDeletedValues);
                var result = _db.SaveChanges();
                if (result <= 0) throw new DbUpdateException("Failed to save changes.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Failed to soft delete sites from database");
            }
        }

        public SiteDTO ToDto(Site site) => new()
        {
            Id = site.Id,
            Name = site.Name,
            CreatedDateTime = site.CreatedDateTime,
            ModifiedDateTime = site.ModifiedDateTime,
            DeletedDateTime = site.DeletedDateTime
        };
    }
}
