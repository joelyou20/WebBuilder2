using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using WebBuilder2.Server.Data;
using WebBuilder2.Server.Data.Models;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services;

public class RepositoryDbService : IRepositoryDbService
{
    private readonly AppDbContext _appDBContext;

    public RepositoryDbService(AppDbContext appDBContext)
    {
        _appDBContext = appDBContext;
    }

    public async Task<ValidationResponse<Repository>> GetSingleAsync(long id)
    {
        try
        {
            RepositoryDTO? dto = await _appDBContext.Repositories
                .FirstOrDefaultAsync(c => c.Id.Equals(id) && c.DeletedDateTime == null);

            return dto == null ? new ValidationResponse<Repository>() : ValidationResponse<Repository>.Success(new List<Repository> { dto!.FromDto() });
        }
        catch (Exception ex)
        {
            return ValidationResponse<Repository>.Failure(message: $"Error: {ex.Message}. EXCEPTION: {ex.InnerException}");
        }
    }

    public async Task<ValidationResponse<Repository>> GetAllAsync()
    {
        try
        {
            var repositories = (await _appDBContext.Repositories.ToListAsync())
                .Select(githubTemplate => githubTemplate.FromDto())
                .Where(githubTemplate => githubTemplate.DeletedDateTime == null);
            return ValidationResponse<Repository>.Success(repositories);
        }
        catch (Exception ex)
        {
            return ValidationResponse<Repository>.Failure(new List<Repository>(), message: $"Error: {ex.Message}. EXCEPTION: {ex.InnerException}");
        }
    }

    public async Task<ValidationResponse<Repository>> InsertAsync(Repository value)
    {
        try
        {
            ValidationResponse<Repository> response = await GetSingleAsync(value.Id);

            if (response.IsSuccessful) return ValidationResponse<Repository>.EntityAlreadyExists(new List<Repository> { value });

            await _appDBContext.Repositories.AddAsync(ToDto(value));
            var result = await _appDBContext.SaveChangesAsync();

            return result == 0 ? ValidationResponse<Repository>.Failure(new List<Repository> { value }) : ValidationResponse<Repository>.Success(new List<Repository> { value });
        }
        catch (Exception ex)
        {
            return ValidationResponse<Repository>.Failure(new List<Repository> { value }, message: $"Error: {ex.Message}. EXCEPTION: {ex.InnerException}");
        }
    }

    public async Task<ValidationResponse<Repository>> UpdateAsync(Repository value)
    {
        try
        {
            _appDBContext.Repositories.Update(ToDto(value));
            var result = await _appDBContext.SaveChangesAsync();
            return result == 0 ? ValidationResponse<Repository>.Failure(new List<Repository> { value }) : ValidationResponse<Repository>.Success(new List<Repository> { value });
        }
        catch (DbUpdateException ex)
        {
            return ValidationResponse<Repository>.Failure(value, message: $"Error: {ex.Message}. EXCEPTION: {ex.InnerException}");
        }
    }

    public async Task<ValidationResponse<Repository>> UpsertAsync(Repository value)
    {
        try
        {
            ValidationResponse<Repository> response = await GetSingleAsync(value.Id);

            if (response.IsSuccessful) _appDBContext.Repositories.Update(ToDto(value));
            else await _appDBContext.Repositories.AddAsync(ToDto(value));

            var result = await _appDBContext.SaveChangesAsync();

            return result == 0 ? ValidationResponse<Repository>.Failure(new List<Repository> { value }) : ValidationResponse<Repository>.Success(new List<Repository> { value });
        }
        catch (DbUpdateException ex)
        {
            return ValidationResponse<Repository>.Failure(value, message: $"Error: {ex.Message}. EXCEPTION: {ex.InnerException}");
        }
    }

    public async Task<ValidationResponse<Repository>> DeleteAsync(Repository value)
    {
        try
        {
            _appDBContext.Remove(ToDto(value));
            var result = await _appDBContext.SaveChangesAsync();
            return result == 0 ?
                ValidationResponse<Repository>.Failure(new List<Repository> { value }) :
                ValidationResponse<Repository>.Success(new List<Repository> { value });
        }
        catch (DbUpdateException ex)
        {
            return ValidationResponse<Repository>.Failure(value, message: $"Error: {ex.Message}. EXCEPTION: {ex.InnerException}");
        }
    }

    public async Task<ValidationResponse<Repository>> SoftDeleteAsync(Repository value)
    {
        try
        {
            value.DeletedDateTime = DateTime.UtcNow;
            _appDBContext.Repositories.Update(ToDto(value));
            var result = await _appDBContext.SaveChangesAsync();
            return result == 0 ?
                ValidationResponse<Repository>.Failure(new List<Repository> { value }) :
                ValidationResponse<Repository>.Success(new List<Repository> { value });
        }
        catch (DbUpdateException ex)
        {
            return ValidationResponse<Repository>.Failure(value, message: $"Error: {ex.Message}. EXCEPTION: {ex.InnerException}");
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
