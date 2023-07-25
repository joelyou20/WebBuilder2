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
            var query = _db.Sites.Where(s => s.DeletedDateTime == null);

            if (exclude != null) query = query.Where(s => !exclude.Any(e => s.Id == e));

            return query.Select(s => new Site
            {
                Id = s.Id,
                Name = s.Name,
                CreatedDateTime = s.CreatedDateTime,
                ModifiedDateTime = s.ModifiedDateTime,
                DeletedDateTime = s.DeletedDateTime
            });
        }

        public IEnumerable<Site> AddRange(IEnumerable<Site> values)
        {
            if (!values.Any()) throw new ArgumentNullException(paramName: "values");
            var dtos = values.Select(ToDto);
            _db.AddRange();
            var result = _db.SaveChanges();
            if (result <= 0) throw new DbUpdateException("Failed to save changes.");
            return dtos.Select(x => x.FromDto());
        }

        public IEnumerable<Site> UpdateRange(IEnumerable<Site> values)
        {
            var dtos = values.Select(ToDto);
            _db.Sites.UpdateRange(dtos);
            var result = _db.SaveChanges();
            if (result <= 0) throw new DbUpdateException("Failed to save changes.");
            return dtos.Select(x => x.FromDto());
        }

        public IEnumerable<Site> UpsertRange(IEnumerable<Site> values)
        {
            IEnumerable<long> valuesList = values.Select(x => x.Id);
            List<Site> existingValues = Get()?.Where(x => valuesList.Contains(x.Id)).ToList() ?? new List<Site>();
            List<Site> newValues = values.Where(x => !existingValues.Select(y => y.Id == x.Id).Any()).ToList();

            var result = new List<Site>();

            if (existingValues.Any()) result.AddRange(UpdateRange(values));
            if (newValues.Any()) result.AddRange(AddRange(newValues));
            return result;
        }

        public IEnumerable<Site> DeleteRange(IEnumerable<Site> values)
        {
            var dtos = values.Select(ToDto);
            _db.RemoveRange(dtos);
            var result = _db.SaveChanges();
            if (result <= 0) throw new DbUpdateException("Failed to save changes.");
            return dtos.Select(x => x.FromDto());
        }

        public IEnumerable<Site> SoftDeleteRange(IEnumerable<Site> values)
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
            return softDeletedValues.Select(x => x.FromDto());
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
