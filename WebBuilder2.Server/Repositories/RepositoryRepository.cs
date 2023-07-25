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
        var query = _db.Repositories.Where(s => s.DeletedDateTime == null);

        if (exclude != null) query = query.Where(s => !exclude.Any(e => s.Id == e));

        return query.Select(r => new Repository()
        {
            Id = r.Id,
            AllowAutoMerge = r.AllowAutoMerge,
            AllowMergeCommit = r.AllowMergeCommit,
            AllowRebaseMerge = r.AllowRebaseMerge,
            AllowSquashMerge = r.AllowSquashMerge,
            AutoInit = r.AutoInit,
            DeleteBranchOnMerge = r.DeleteBranchOnMerge,
            GitIgnoreTemplate = r.GitIgnoreTemplate,
            HasDownloads = r.HasDownloads,
            HasIssues = r.HasIssues,
            HasProjects = r.HasProjects,
            HasWiki = r.HasWiki,
            Homepage = r.Homepage,
            Description = r.Description,
            IsPrivate = r.IsPrivate,
            IsTemplate = r.IsTemplate,
            LicenseTemplate = r.LicenseTemplate,
            TeamId = r.TeamId,
            Name = r.Name,
            RepoName = r.RepoName,
            UseSquashPrTitleAsDefault = r.UseSquashPrTitleAsDefault,
            Visibility = r.Visibility,
            HtmlUrl = r.HtmlUrl,
            GitUrl = r.GitUrl,
            CreatedDateTime = r.CreatedDateTime,
            DeletedDateTime = r.DeletedDateTime,
            ModifiedDateTime = r.ModifiedDateTime
        });
    }

    public void AddRange(IEnumerable<Repository> values)
    {
        if (!values.Any()) return;
        _db.AddRange(values.Select(ToDto));
        var result = _db.SaveChanges();
        if (result <= 0) throw new DbUpdateException("Failed to save changes.");
    }

    public void UpdateRange(IEnumerable<Repository> values)
    {
        var dtos = values.Select(ToDto);
        _db.Repositories.UpdateRange(dtos);
        var result = _db.SaveChanges();
        if (result <= 0) throw new DbUpdateException("Failed to save changes.");
    }

    public void UpsertRange(IEnumerable<Repository> values)
    {
        IEnumerable<long> valuesList = values.Select(x => x.Id);
        List<Repository> existingValues = Get()?.Where(x => valuesList.Contains(x.Id)).ToList() ?? new List<Repository>();
        List<Repository> newValues = values.Where(x => !existingValues.Any(y => y.Id.Equals(x.Id))).ToList();

        if (existingValues.Any()) UpdateRange(values);
        if (newValues.Any()) AddRange(newValues);
    }

    public void DeleteRange(IEnumerable<Repository> values)
    {
        var dtos = values.Select(ToDto);
        _db.RemoveRange(dtos);
        var result = _db.SaveChanges();
        if (result <= 0) throw new DbUpdateException("Failed to save changes.");
    }

    public void SoftDeleteRange(IEnumerable<Repository> values)
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
        HtmlUrl = repository.HtmlUrl,
        GitUrl = repository.GitUrl
    };
}
