//using Microsoft.EntityFrameworkCore;
//using WebBuilder2.Server.Data.Models;
//using WebBuilder2.Server.Data;
//using WebBuilder2.Server.Data.Models.Contracts;
//using WebBuilder2.Server.Repositories.Contracts;
//using WebBuilder2.Shared.Models;

//namespace WebBuilder2.Server.Repositories;

//public class RepositoryBase<T1, T2> : IRepository<T1, T2>
//    where T1 : AuditableEntity
//    where T2 : IDto<T1>
//{
//    private readonly AppDbContext _db;
//    private readonly DbSet<T2> _dbSet;
//    private readonly ILogger<RepositoryRepository> _logger;

//    public RepositoryBase(AppDbContext db, DbSet<T2> dbSet, ILogger<RepositoryRepository> logger)
//    {
//        _db = db;
//        _dbSet = dbSet;
//        _logger = logger;
//    }

//    public IQueryable<T1>? Get()
//    {
//        try
//        {
//            IQueryable<T1> query = from r in _db.Repositories
//                                           where r.DeletedDateTime == null
//                                           select r.FromDto();
//            return query;
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Failed to retrieve repositories from database");
//            return null;
//        }
//    }

//    public void AddRange(IEnumerable<T1> values)
//    {
//        try
//        {
//            if (!values.Any()) return;
//            _db.AddRange(values);
//            var result = _db.SaveChanges();
//            if (result <= 0) throw new DbUpdateException("Failed to save changes.");
//        }
//        catch (DbUpdateException ex)
//        {
//            _logger.LogError(ex, "Failed to add repositories to database");
//        }
//    }

//    public void UpdateRange(IEnumerable<T1> values)
//    {
//        try
//        {
//            var dtos = values.Select(ToDto);
//            _db.Repositories.UpdateRange(dtos);
//            var result = _db.SaveChanges();
//            if (result <= 0) throw new DbUpdateException("Failed to save changes.");
//        }
//        catch (DbUpdateException ex)
//        {
//            _logger.LogError(ex, "Failed to retrieve repositories from database");
//        }
//    }

//    public void UpsertRange(IEnumerable<T1> values)
//    {
//        try
//        {
//            List<T1> existingValues = Get()?.Where(x => values.Select(y => y.Id == x.Id).Any()).ToList() ?? new List<T1>();
//            List<T1> newValues = values.Where(x => !existingValues.Select(y => y.Id == x.Id).Any()).ToList();

//            if (existingValues.Any()) UpdateRange(values);
//            if (newValues.Any()) AddRange(newValues);
//        }
//        catch (DbUpdateException ex)
//        {
//            _logger.LogError(ex, "Failed to upsert repositories");
//        }
//    }

//    public void DeleteRange(IEnumerable<T1> values)
//    {
//        try
//        {
//            var dtos = values.Select(ToDto);
//            _db.RemoveRange(dtos);
//            var result = _db.SaveChanges();
//            if (result <= 0) throw new DbUpdateException("Failed to save changes.");
//        }
//        catch (DbUpdateException ex)
//        {
//            _logger.LogError(ex, "Failed to delete repositories from database");
//        }
//    }

//    public void SoftDeleteRange(IEnumerable<T1> values)
//    {
//        try
//        {
//            var softDeletedValues = values.Select(x =>
//            {
//                var copy = ToDto(x);
//                copy.DeletedDateTime = DateTime.UtcNow;
//                return copy;
//            });
//            _db.Repositories.UpdateRange(softDeletedValues);
//            var result = _db.SaveChanges();
//            if (result <= 0) throw new DbUpdateException("Failed to save changes.");
//        }
//        catch (DbUpdateException ex)
//        {
//            _logger.LogError(ex, "Failed to soft delete repositories from database");
//        }
//    }

//    public T2 ToDto(T1 repository) => new()
//    {
//        Id = repository.Id,
//        Name = repository.Name,
//        AllowAutoMerge = repository.AllowAutoMerge,
//        AllowMergeCommit = repository.AllowMergeCommit,
//        AllowRebaseMerge = repository.AllowRebaseMerge,
//        AllowSquashMerge = repository.AllowSquashMerge,
//        AutoInit = repository.AutoInit,
//        CreatedDateTime = repository.CreatedDateTime,
//        DeleteBranchOnMerge = repository.DeleteBranchOnMerge,
//        DeletedDateTime = repository.DeletedDateTime,
//        Description = repository.Description,
//        GitIgnoreTemplate = repository.GitIgnoreTemplate,
//        HasDownloads = repository.HasDownloads,
//        HasIssues = repository.HasIssues,
//        HasProjects = repository.HasProjects,
//        HasWiki = repository.HasWiki,
//        Homepage = repository.Homepage,
//        IsPrivate = repository.IsPrivate,
//        IsTemplate = repository.IsTemplate,
//        LicenseTemplate = repository.LicenseTemplate,
//        ModifiedDateTime = repository.ModifiedDateTime,
//        RepoName = repository.RepoName,
//        TeamId = repository.TeamId,
//        UseSquashPrTitleAsDefault = repository.UseSquashPrTitleAsDefault,
//        Visibility = repository.Visibility,
//        Url = repository.Url,
//        GitUrl = repository.GitUrl
//    };
//}
