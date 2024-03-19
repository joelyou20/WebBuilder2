using Microsoft.EntityFrameworkCore;
using WebBuilder2.Server.Data;
using WebBuilder2.Server.Repositories.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Server.Repositories
{
    public class SiteRepositoryRepository : ISiteRepositoryRepository
    {
        private readonly AppDbContext _db;
        private readonly ILogger<SiteRepositoryRepository> _logger;

        public SiteRepositoryRepository(AppDbContext db, ILogger<SiteRepositoryRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public IQueryable<SiteRepositoryModel>? Get(IEnumerable<long>? exclude = null)
        {
            var query = _db.SiteRepository
                .Where(s => s.DeletedDateTime == null);

            if (exclude != null) query = query.Where(s => !exclude.Any(e => s.Id == e));

            return query.Select(s => new SiteRepositoryModel
            {
                Id = s.Id,
                Repository = s.Repository.FromDto(),
                Site = s.Site.FromDto(),
                SiteId = s.SiteId,
                RepositoryId = s.RepositoryId,
                CreatedDateTime = s.CreatedDateTime,
                DeletedDateTime = s.DeletedDateTime,
                ModifiedDateTime = s.ModifiedDateTime
            });
        }

        public IEnumerable<SiteRepositoryModel> AddRange(IEnumerable<SiteRepositoryModel> values)
        {
            if (!values.Any()) throw new ArgumentNullException(paramName: nameof(values));
            var dtos = values.Select(ToDto).ToArray();
            _db.AddRange(dtos);
            var result = _db.SaveChanges();
            if (result <= 0) throw new DbUpdateException("Failed to save changes.");
            return dtos.Select(x => x.FromDto());
        }

        public IEnumerable<SiteRepositoryModel> UpdateRange(IEnumerable<SiteRepositoryModel> values)
        {
            var dtos = values.Select(ToDto).ToArray();
            _db.SiteRepository.UpdateRange(dtos);
            var result = _db.SaveChanges();
            if (result <= 0) throw new DbUpdateException("Failed to save changes.");
            return dtos.Select(x => x.FromDto());
        }

        public IEnumerable<SiteRepositoryModel> UpsertRange(IEnumerable<SiteRepositoryModel> values)
        {
            IEnumerable<long> valuesList = values.Select(x => x.Id).ToArray();
            List<SiteRepositoryModel> existingValues = Get()?.Where(x => valuesList.Contains(x.Id)).ToList() ?? new List<SiteRepositoryModel>();
            List<SiteRepositoryModel> newValues = values.Where(x => !existingValues.Select(y => y.Id == x.Id).Any()).ToList();

            var result = new List<SiteRepositoryModel>();

            if (existingValues.Any()) result.AddRange(UpdateRange(values));
            if (newValues.Any()) result.AddRange(AddRange(newValues));
            return result;
        }

        public IEnumerable<SiteRepositoryModel> DeleteRange(IEnumerable<SiteRepositoryModel> values)
        {
            var dtos = values.Select(ToDto).ToArray();
            _db.RemoveRange(dtos);
            var result = _db.SaveChanges();
            if (result <= 0) throw new DbUpdateException("Failed to save changes.");
            return dtos.Select(x => x.FromDto());
        }

        public IEnumerable<SiteRepositoryModel> SoftDeleteRange(IEnumerable<SiteRepositoryModel> values)
        {
            var softDeletedValues = values.Select(x =>
            {
                var copy = ToDto(x);
                copy.DeletedDateTime = DateTime.UtcNow;
                return copy;
            }).ToArray();
            _db.SiteRepository.UpdateRange(softDeletedValues);
            var result = _db.SaveChanges();
            if (result <= 0) throw new DbUpdateException("Failed to save changes.");
            return softDeletedValues.Select(x => x.FromDto());
        }

        public Data.Models.SiteRepository ToDto(SiteRepositoryModel siteRepository) => new()
        {
            Id = siteRepository.Id,
            RepositoryId = siteRepository.RepositoryId,
            SiteId = siteRepository.SiteId,
            CreatedDateTime = siteRepository.CreatedDateTime,
            ModifiedDateTime = siteRepository.ModifiedDateTime,
            DeletedDateTime = siteRepository.DeletedDateTime,
        };
    }
}
