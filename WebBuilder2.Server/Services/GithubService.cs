using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Octokit;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using WebBuilder2.Server.Data.Models;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services;

public class GithubService : IGithubService
{
    private IGitHubClient _client;

    public GithubService(IGitHubClient client)
    {
        _client = client;
    }

    public async Task<ValidationResponse<RepositoryModel>> GetRepositoriesAsync()
    {
        List<RepositoryModel> repos = new();

        var repositories = await _client.Repository.GetAllForCurrent();

        foreach (var repo in repositories)
        {
            repos.Add(ParseRepository(repo));
        }

        return ValidationResponse<RepositoryModel>.Success(repos);
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
            return new ValidationResponse
            {
                IsSuccessful = false,
                Message = "Failed to Authenticate",
            };
        }
    }

    public async Task<ValidationResponse<RepositoryModel>> CreateRepoAsync(RepositoryModel repository)
    {
        try
        {
            NewRepository newRepo = new(repository.RepoName)
            {
                Description = repository.Description,
                Private = repository.IsPrivate,
                Visibility = repository.Visibility switch
                {
                    RepoVisibility.Public => RepositoryVisibility.Public,
                    RepoVisibility.Private => RepositoryVisibility.Private,
                    RepoVisibility.Internal => RepositoryVisibility.Internal,
                    _ => throw new ArgumentOutOfRangeException(nameof(repository.Visibility),
                                                               $"Not expected request visibility value {repository.Visibility}"),
                },
                IsTemplate = repository.IsTemplate,
                AllowAutoMerge = repository.AllowAutoMerge,
                AllowMergeCommit = repository.AllowMergeCommit,
                AllowRebaseMerge = repository.AllowRebaseMerge,
                AllowSquashMerge = repository.AllowSquashMerge,
                AutoInit = repository.AutoInit,
                DeleteBranchOnMerge = repository.DeleteBranchOnMerge,
                GitignoreTemplate = repository.GitIgnoreTemplate,
                HasDownloads = repository.HasDownloads,
                HasIssues = repository.HasIssues,
                HasProjects = repository.HasProjects,
                HasWiki = repository.HasWiki,
                Homepage = repository.Homepage,
                LicenseTemplate = repository.LicenseTemplate,
                TeamId = repository.TeamId,
                UseSquashPrTitleAsDefault = repository.UseSquashPrTitleAsDefault
            };

            var createResult = await _client.Repository.Create(newRepo);

            var response = ParseRepository(createResult);

            response.AutoInit = repository.AutoInit;
            response.GitIgnoreTemplate = repository.GitIgnoreTemplate;
            response.HasProjects = repository.HasProjects;
            response.LicenseTemplate = repository.LicenseTemplate;
            response.TeamId = repository.TeamId;
            response.UseSquashPrTitleAsDefault = repository.UseSquashPrTitleAsDefault;
            response.Visibility = repository.Visibility;

            return ValidationResponse<RepositoryModel>.Success(response);
        }
        catch (ApiException ex)
        {
            var response = new ValidationResponse<RepositoryModel>
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
        IReadOnlyList<LicenseMetadata> licenses = await _client.Licenses.GetAllLicenses();

        return ValidationResponse<GithubProjectLicense>.Success(licenses.Select(license => new GithubProjectLicense
        {
            Featured = license.Featured,
            Key = license.Key,
            Name = license.Name,
            Url = license.Url
        }));
    }

    public async Task<ValidationResponse<GithubSecretResponse>> GetSecretsAsync()
    {
        var client = new HttpClient();

        var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/repos/joelyou20/Portfolio/actions/secrets");
        request.Headers.Add("Accept", "application/vnd.github+json");
        request.Headers.Add("Authorization", "Bearer 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");
        request.Headers.Add("X-GitHub-Api-Version", "2022-11-28");
        request.Headers.Add("User-Agent", "request");
        request.Headers.Add("Cookie", "_octo=GH1.1.1578474083.1689685007; logged_in=no");

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var message = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<GithubSecretResponse>(message);

        if(result == null) return ValidationResponse<GithubSecretResponse>.Failure();

        return ValidationResponse<GithubSecretResponse>.Success(result);
    }

    public RepositoryModel ParseRepository(Octokit.Repository repo) => new RepositoryModel
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
        ExternalId = repo.Id,
        IsPrivate = repo.Private,
        IsTemplate = repo.IsTemplate,
        ModifiedDateTime = repo.CreatedAt.DateTime,
        Name = repo.Name,
        RepoName = repo.FullName,
        GitUrl = repo.GitUrl,
        HtmlUrl = repo.HtmlUrl,
    };

    public Data.Models.Repository ToDto(RepositoryModel repo) => new()
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
