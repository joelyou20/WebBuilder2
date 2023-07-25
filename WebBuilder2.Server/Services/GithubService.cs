using Microsoft.AspNetCore.Mvc;
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

    public async Task<ValidationResponse<Shared.Models.Repository>> GetRepositoriesAsync()
    {
        List<Shared.Models.Repository> repos = new();

        var repositories = await _client.Repository.GetAllForCurrent();

        foreach (var repo in repositories)
        {
            repos.Add(ParseRepository(repo));
        }

        return ValidationResponse<Shared.Models.Repository>.Success(repos);
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

            var response = ParseRepository(createResult);

            response.AutoInit = request.AutoInit;
            response.GitIgnoreTemplate = request.GitignoreTemplate;
            response.HasProjects = request.HasProjects;
            response.LicenseTemplate = request.LicenseTemplate;
            response.TeamId = request.TeamId;
            response.UseSquashPrTitleAsDefault = request.UseSquashPrTitleAsDefault;
            response.Visibility = request.Visibility;

            return ValidationResponse<Shared.Models.Repository>.Success(response);
        }
        catch (ApiException ex)
        {
            var response = new ValidationResponse<Shared.Models.Repository>
            {
                Errors = ex.ApiError.Errors.Select(error => new Shared.Models.ApiError
                {
                    Message = error.Message,
                    Code = error.Code,
                    Resource = error.Resource,
                    Field = error.Field
                }).ToList()
            };

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

    public Shared.Models.Repository ParseRepository(Octokit.Repository repo) => new Shared.Models.Repository
    {
        AllowAutoMerge = repo.AllowAutoMerge != null,
        AllowMergeCommit = repo.AllowMergeCommit != null,
        AllowRebaseMerge = repo.AllowRebaseMerge != null,
        AllowSquashMerge = repo.AllowSquashMerge != null,
        CreatedDateTime = repo.CreatedAt.DateTime,
        DeleteBranchOnMerge = repo.DeleteBranchOnMerge != null,
        DeletedDateTime = null,
        Description = repo.Description ?? "No description", // This is added to solve issues when importing repos that don't have existing values
        HasDownloads = repo.HasDownloads,
        HasIssues = repo.HasIssues,
        HasWiki = repo.HasWiki,
        Homepage = repo.Homepage.IsNullOrEmpty() ? "No homepage" : repo.Homepage, // This is added to solve issues when importing repos that don't have existing values
        Id = repo.Id,
        IsPrivate = repo.Private,
        IsTemplate = repo.IsTemplate,
        ModifiedDateTime = repo.CreatedAt.DateTime,
        Name = repo.Name,
        RepoName = repo.FullName,
        GitUrl = repo.GitUrl,
        HtmlUrl = repo.HtmlUrl,
    };

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
