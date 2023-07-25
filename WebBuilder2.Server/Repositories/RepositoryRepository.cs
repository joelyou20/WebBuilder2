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
        var query = _db.Repository.Where(s => s.DeletedDateTime == null);

        if (exclude != null) query = query.Where(s => !exclude.Any(e => s.Id == e));

        return query.Select(r => new Repository()
        {
            Id = r.Id,
            SiteId = r.SiteId,
            ExternalId = r.ExternalId,
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

    public IEnumerable<Repository> AddRange(IEnumerable<Repository> values)
    {
        if (!values.Any()) throw new ArgumentNullException(paramName: "values");
        var dtos = values.Select(ToDto).ToArray();

        _db.AddRange(dtos);
        var result = _db.SaveChanges();
        if (result <= 0) throw new DbUpdateException("Failed to save changes.");
        return dtos.Select(x => x.FromDto());
    }

    public IEnumerable<Repository> UpdateRange(IEnumerable<Repository> values)
    {
        var dtos = values.Select(ToDto).ToArray();
        _db.Repository.UpdateRange(dtos);
        var result = _db.SaveChanges();
        if (result <= 0) throw new DbUpdateException("Failed to save changes.");
        return dtos.Select(x => x.FromDto());
    }

    public IEnumerable<Repository> UpsertRange(IEnumerable<Repository> values)
    {
        IEnumerable<long> valuesList = values.Select(x => x.Id);
        List<Repository> existingValues = Get()?.Where(x => valuesList.Contains(x.Id)).ToList() ?? new List<Repository>();
        List<Repository> newValues = values.Where(x => !existingValues.Any(y => y.Id.Equals(x.Id))).ToList();

        var result = new List<Repository>();

        if (existingValues.Any()) result.AddRange(UpdateRange(values));
        if (newValues.Any()) result.AddRange(AddRange(newValues));
        return result;
    }

    public IEnumerable<Repository> DeleteRange(IEnumerable<Repository> values)
    {
        var dtos = values.Select(ToDto).ToArray(); ;
        _db.RemoveRange(dtos);
        var result = _db.SaveChanges();
        if (result <= 0) throw new DbUpdateException("Failed to save changes.");
        return dtos.Select(x => x.FromDto());
    }

    public IEnumerable<Repository> SoftDeleteRange(IEnumerable<Repository> values)
    {
        var softDeletedValues = values.Select(x =>
        {
            var copy = ToDto(x);
            copy.DeletedDateTime = DateTime.UtcNow;
            return copy;
        }).ToArray();
        _db.Repository.UpdateRange(softDeletedValues);
        var result = _db.SaveChanges();
        if (result <= 0) throw new DbUpdateException("Failed to save changes.");
        return softDeletedValues.Select(x => x.FromDto());
    }

    public RepositoryDTO ToDto(Repository repository) => new()
    {
        Id = repository.Id,
        SiteId = repository.SiteId,
        ExternalId = repository.ExternalId,
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
