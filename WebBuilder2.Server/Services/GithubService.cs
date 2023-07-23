using Microsoft.IdentityModel.Tokens;
using Octokit;
using System.Net;
using WebBuilder2.Server.Data.Models;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services;

public class GithubService : IGithubService
{
    private GitHubClient _client;

    public GithubService(GitHubClient client)
    {
        _client = client;
    }

    public async Task<ValidationResponse<RespositoryResponse>> GetRespositoriesAsync()
    {
        List<Shared.Models.Repository> repos = new();

        var repositories = await _client.Repository.GetAllForCurrent();

        foreach (var repo in repositories)
        {
            repos.Add(new Shared.Models.Repository
            {
                Id = repo.Id,
                Name = repo.Name,
            });
        }

        var response = new RespositoryResponse
        {
            Repositories = repos
        };

        return ValidationResponse<RespositoryResponse>.Success(response);
    }

    public async Task<ValidationResponse> AuthenticateUserAsync(GithubAuthenticationRequest request)
    {
        try
        {
            // Check if client is already authenticated. _client.User.Current() will throw an AuthorizationException if the client is not authenticated
            User user = await _client.User.Current();
            return new ValidationResponse
            {
                IsSuccessful = true,
                Message = "Success",
            };
        }
        catch (AuthorizationException)
        {
            // IF client is not authenticated and there is no PAT in the request, throw an error
            // ELSE attempt to authorize client with PAT
            if (request == null || request.PersonalAccessToken.IsNullOrEmpty())
            {
                return new ValidationResponse
                {
                    IsSuccessful = false,
                    Message = "Failed to Authenticate",
                };
            }
            else
            {
                _client.Credentials = new Credentials(request.PersonalAccessToken);
                User user = await _client.User.Current();
                return new ValidationResponse
                {
                    IsSuccessful = user != null,
                    Message = user == null ? "Failed to Authenticate" : $"Success: User {user.Email} is authenticated"
                };
            }
        }
    }

    public async Task<ValidationResponse<Shared.Models.Repository>> CreateRepoAsync(GithubCreateRepoRequest request)
    {
        try
        {
            NewRepository newRepo = new(request.RepoName)
            {
                Description = request.Description,
                Private = request.IsPrivate,
                Visibility = request.Visibility switch
                {
                    RepoVisibility.Public => RepositoryVisibility.Public,
                    RepoVisibility.Private => RepositoryVisibility.Private,
                    RepoVisibility.Internal => RepositoryVisibility.Internal,
                    _ => throw new ArgumentOutOfRangeException(nameof(request.Visibility),
                                                               $"Not expected request visibility value {request.Visibility}"),
                },
                IsTemplate = request.IsTemplate,
                AllowAutoMerge = request.AllowAutoMerge,
                AllowMergeCommit = request.AllowMergeCommit,
                AllowRebaseMerge = request.AllowRebaseMerge,
                AllowSquashMerge = request.AllowSquashMerge,
                AutoInit = request.AutoInit,
                DeleteBranchOnMerge = request.DeleteBranchOnMerge,
                GitignoreTemplate = request.GitignoreTemplate,
                HasDownloads = request.HasDownloads,
                HasIssues = request.HasIssues,
                HasProjects = request.HasProjects,
                HasWiki = request.HasWiki,
                Homepage = request.Homepage,
                LicenseTemplate = request.LicenseTemplate,
                TeamId = request.TeamId,
                UseSquashPrTitleAsDefault = request.UseSquashPrTitleAsDefault
            };

            var createResult = await _client.Repository.Create(newRepo);

            return ValidationResponse<Shared.Models.Repository>.Success(new Shared.Models.Repository
            {
                AllowAutoMerge = createResult.AllowAutoMerge != null,
                AllowMergeCommit = createResult.AllowMergeCommit != null,
                AllowRebaseMerge = createResult.AllowRebaseMerge != null,
                AllowSquashMerge = createResult.AllowSquashMerge != null,
                AutoInit = request.AutoInit,
                CreatedDateTime = createResult.CreatedAt.DateTime,
                DeleteBranchOnMerge = createResult.DeleteBranchOnMerge != null,
                DeletedDateTime = null,
                Description = createResult.Description,
                GitIgnoreTemplate = request.GitignoreTemplate,
                HasDownloads = createResult.HasDownloads,
                HasIssues = createResult.HasIssues,
                HasProjects = request.HasProjects,
                HasWiki = createResult.HasWiki,
                Homepage = createResult.Homepage,
                Id = createResult.Id,
                IsPrivate = createResult.Private,
                IsTemplate = createResult.IsTemplate,
                LicenseTemplate = request.LicenseTemplate,
                ModifiedDateTime = createResult.CreatedAt.DateTime,
                Name = createResult.Name,
                RepoName = createResult.FullName,
                TeamId = request.TeamId,
                UseSquashPrTitleAsDefault = request.UseSquashPrTitleAsDefault,
                Visibility = request.Visibility,
                Url = createResult.Url,
                GitUrl = createResult.GitUrl,
            });
        }
        catch(ApiException ex)
        {
            var response = new ValidationResponse<Shared.Models.Repository>();
            response.Errors = ex.ApiError.Errors.Select(error => new Shared.Models.ApiError
            {
                Message = error.Message,
                Code = error.Code,
                Resource = error.Resource,
                Field = error.Field
            }).ToList();

            return response;
        }
    }

    public async Task<ValidationResponse<GitIgnoreTemplateResponse>> GetGitIgnoreTemplatesAsync()
    {
        IReadOnlyList<string> templates = await _client.GitIgnore.GetAllGitIgnoreTemplates();
        GitIgnoreTemplateResponse response = new(templates);
        return ValidationResponse<GitIgnoreTemplateResponse>.Success(response);
    }

    public async Task<ValidationResponse<GithubProjectLicense>> GetLicenseTemplatesAsync()
    {
        var licenses = await _client.Licenses.GetAllLicenses();

        return ValidationResponse<GithubProjectLicense>.Success(licenses.Select(license => new GithubProjectLicense
        {
            Featured = license.Featured,
            Key = license.Key,
            Name = license.Name,
            Url = license.Url
        }));
    }

    public RepositoryDTO ToDto(Shared.Models.Repository repo) => new()
    {
        Id = repo.Id,
        Name = repo.Name,
        AllowAutoMerge = repo.AllowAutoMerge,
        AllowMergeCommit = repo.AllowMergeCommit,
        AllowRebaseMerge = repo.AllowRebaseMerge,
        AllowSquashMerge = repo.AllowSquashMerge,
        AutoInit = repo.AutoInit,
        CreatedDateTime = repo.CreatedDateTime,
        DeleteBranchOnMerge = repo.DeleteBranchOnMerge,
        DeletedDateTime = repo.DeletedDateTime,
        Description = repo.Description,
        GitIgnoreTemplate = repo.GitIgnoreTemplate,
        HasDownloads = repo.HasDownloads,
        HasIssues = repo.HasIssues,
        HasProjects = repo.HasProjects,
        HasWiki = repo.HasWiki,
        Homepage = repo.Homepage,
        IsPrivate = repo.IsPrivate,
        IsTemplate = repo.IsTemplate,
        LicenseTemplate = repo.LicenseTemplate,
        ModifiedDateTime = repo.ModifiedDateTime,
        RepoName = repo.RepoName,
        TeamId = repo.TeamId,
        UseSquashPrTitleAsDefault = repo.UseSquashPrTitleAsDefault,
        Visibility = repo.Visibility
    };
}
