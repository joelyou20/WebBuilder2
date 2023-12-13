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

        public IQueryable<SiteModel>? Get(IEnumerable<long>? exclude = null)
        {
            var query = _db.Site
                .Include(s => s.Repository)
                .Where(s => s.DeletedDateTime == null);

            if (exclude != null) query = query.Where(s => !exclude.Any(e => s.Id == e));

            return query.Select(s => new SiteModel
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Repository = s.Repository == null ? null : s.Repository.FromDto(),
                CreatedDateTime = s.CreatedDateTime,
                ModifiedDateTime = s.ModifiedDateTime,
                DeletedDateTime = s.DeletedDateTime,
                SSLCertificateIssueDate = s.SSLCertificateIssueDate
            });
        }

        public IEnumerable<SiteModel> AddRange(IEnumerable<SiteModel> values)
        {
            if (!values.Any()) throw new ArgumentNullException(paramName: nameof(values));
            var dtos = values.Select(ToDto).ToArray();
            _db.AddRange(dtos);
            var result = _db.SaveChanges();
            if (result <= 0) throw new DbUpdateException("Failed to save changes.");
            return dtos.Select(x => x.FromDto());
        }

        public IEnumerable<SiteModel> UpdateRange(IEnumerable<SiteModel> values)
        {
            var dtos = values.Select(ToDto).ToArray();
            _db.Site.UpdateRange(dtos);
            var result = _db.SaveChanges();
            if (result <= 0) throw new DbUpdateException("Failed to save changes.");
            return dtos.Select(x => x.FromDto());
        }

        public IEnumerable<SiteModel> UpsertRange(IEnumerable<SiteModel> values)
        {
            IEnumerable<long> valuesList = values.Select(x => x.Id).ToArray();
            List<SiteModel> existingValues = Get()?.Where(x => valuesList.Contains(x.Id)).ToList() ?? new List<SiteModel>();
            List<SiteModel> newValues = values.Where(x => !existingValues.Select(y => y.Id == x.Id).Any()).ToList();

            var result = new List<SiteModel>();

            if (existingValues.Any()) result.AddRange(UpdateRange(values));
            if (newValues.Any()) result.AddRange(AddRange(newValues));
            return result;
        }

        public IEnumerable<SiteModel> DeleteRange(IEnumerable<SiteModel> values)
        {
            var dtos = values.Select(ToDto).ToArray();
            _db.RemoveRange(dtos);
            var result = _db.SaveChanges();
            if (result <= 0) throw new DbUpdateException("Failed to save changes.");
            return dtos.Select(x => x.FromDto());
        }

        public IEnumerable<SiteModel> SoftDeleteRange(IEnumerable<SiteModel> values)
        {
            var softDeletedValues = values.Select(x =>
            {
                var copy = ToDto(x);
                copy.DeletedDateTime = DateTime.UtcNow;
                return copy;
            }).ToArray();
            _db.Site.UpdateRange(softDeletedValues);
            var result = _db.SaveChanges();
            if (result <= 0) throw new DbUpdateException("Failed to save changes.");
            return softDeletedValues.Select(x => x.FromDto());
        }

        public Site ToDto(SiteModel site) => new()
        {
            Id = site.Id,
            Name = site.Name,
            Description = site.Description,
            CreatedDateTime = site.CreatedDateTime,
            ModifiedDateTime = site.ModifiedDateTime,
            DeletedDateTime = site.DeletedDateTime,
            SSLCertificateIssueDate = site.SSLCertificateIssueDate
        };
    }
}
