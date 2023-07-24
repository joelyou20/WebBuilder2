using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using WebBuilder2.Server.Data;
using WebBuilder2.Server.Data.Models;
using WebBuilder2.Server.Repositories.Contracts;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Repositories;

public class RepositoryRepository : IRepositoryRepository
{
    private readonly AppDbContext _db;
    private readonly ILogger<RepositoryRepository> _logger;

    public RepositoryRepository(AppDbContext db, ILogger<RepositoryRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public IQueryable<Repository>? Get(IEnumerable<long>? exclude = null)
    {
        try
        {
            var query = _db.Repositories.Where(s => s.DeletedDateTime == null);

            if (exclude != null) query = query.Where(s => !exclude.Any(e => s.Id == e));

            return query.Select(s => s.FromDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve repositories from database");
            return null;
        }
    }

    public void AddRange(IEnumerable<Repository> values)
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
            _logger.LogError(ex, "Failed to add repositories to database");
        }
    }

    public void UpdateRange(IEnumerable<Repository> values)
    {
        try
        {
            var dtos = values.Select(ToDto);
            _db.Repositories.UpdateRange(dtos);
            var result = _db.SaveChanges();
            if (result <= 0) throw new DbUpdateException("Failed to save changes.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to retrieve repositories from database");
        }
    }

    public void UpsertRange(IEnumerable<Repository> values)
    {
        try
        {
            List<Repository> existingValues = Get()?.Where(x => values.Select(y => y.Id == x.Id).Any()).ToList() ?? new List<Repository>();
            List<Repository> newValues = values.Where(x => !existingValues.Select(y => y.Id == x.Id).Any()).ToList();

            if (existingValues.Any()) UpdateRange(values);
            if (newValues.Any()) AddRange(newValues);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to upsert repositories");
        }
    }

    public void DeleteRange(IEnumerable<Repository> values)
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
            _logger.LogError(ex, "Failed to delete repositories from database");
        }
    }

    public void SoftDeleteRange(IEnumerable<Repository> values)
    {
        try
        {
            var softDeletedValues = values.Select(x =>
            {
                var copy = ToDto(x);
                copy.DeletedDateTime = DateTime.UtcNow;
                return copy;
            });
            _db.Repositories.UpdateRange(softDeletedValues);
            var result = _db.SaveChanges();
            if (result <= 0) throw new DbUpdateException("Failed to save changes.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to soft delete repositories from database");
        }
    }

    public RepositoryDTO ToDto(Repository repository) => new()
    {
        Id = repository.Id,
        Name = repository.Name,
        AllowAutoMerge = repository.AllowAutoMerge,
        AllowMergeCommit = repository.AllowMergeCommit,
        AllowRebaseMerge = repository.AllowRebaseMerge,
        AllowSquashMerge = repository.AllowSquashMerge,
        AutoInit = repository.AutoInit,
        CreatedDateTime = repository.CreatedDateTime,
        DeleteBranchOnMerge = repository.DeleteBranchOnMerge,
        DeletedDateTime = repository.DeletedDateTime,
        Description = repository.Description,
        GitIgnoreTemplate = repository.GitIgnoreTemplate,
        HasDownloads = repository.HasDownloads,
        HasIssues = repository.HasIssues,
        HasProjects = repository.HasProjects,
        HasWiki = repository.HasWiki,
        Homepage = repository.Homepage,
        IsPrivate = repository.IsPrivate,
        IsTemplate = repository.IsTemplate,
        LicenseTemplate = repository.LicenseTemplate,
        ModifiedDateTime = repository.ModifiedDateTime,
        RepoName = repository.RepoName,
        TeamId = repository.TeamId,
        UseSquashPrTitleAsDefault = repository.UseSquashPrTitleAsDefault,
        Visibility = repository.Visibility,
        Url = repository.Url,
        GitUrl = repository.GitUrl
    };
}
