using Amazon.Runtime;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Octokit;
using Sodium;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using WebBuilder2.Server.Data.Models;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Server.Utils;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebBuilder2.Server.Services;

public class GithubService : IGithubService
{
    private IGitHubClient _client;
    private IAwsSecretsManagerService _awsSecretsManagerService;

    public GithubService(IGitHubClient client, IAwsSecretsManagerService awsSecretsManagerService)
    {
        _client = client;
        _awsSecretsManagerService = awsSecretsManagerService;
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

    public async Task<ValidationResponse<GithubSecretResponse>> GetSecretsAsync(string userName, string repoName)
    {
        using var client = new HttpClient();

        HttpRequestMessage request = await BuildRequestAsync(HttpMethod.Get, "actions/secrets", userName, repoName);

        HttpResponseMessage response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string message = await response.Content.ReadAsStringAsync();
        GithubSecretResponse? result = JsonConvert.DeserializeObject<GithubSecretResponse>(message);

        if(result == null) return ValidationResponse<GithubSecretResponse>.Failure();

        return ValidationResponse<GithubSecretResponse>.Success(result);
    }

    public async Task<ValidationResponse<GithubSecret>> CreateSecretAsync(IEnumerable<GithubSecret> secrets, string userName, string repoName)
    {
        using var client = new HttpClient();

        GithubPublicKey? publicKey = await GetPublicKeyAsync(userName, repoName) ?? throw new NotFoundException("Github public key not found", HttpStatusCode.NotFound);

        foreach(GithubSecret secret in secrets)
        {
            if (secret.Value == null) throw new ArgumentNullException($"Github Secret: {secret.Name} has no value.");

            string encodedSecret = EncodeSecret(secret.Value, publicKey.Key);

            GithubCreateSecretRequest githubCreateSecretRequest = new(encodedSecret, publicKey.Id);
            JsonContent content = JsonContent.Create(githubCreateSecretRequest);

            HttpRequestMessage request = await BuildRequestAsync(HttpMethod.Put, $"actions/secrets/{secret.Name}", userName, repoName, content);

            HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        return ValidationResponse<GithubSecret>.Success(secrets);
    }

    public async Task<ValidationResponse<string>> GetUserAsync()
    {
        var user = await _client.User.Current();
        return ValidationResponse<string>.Success(user.Login);
    }

    public async Task<ValidationResponse> CreateCommitAsync(string owner, string repoName, GithubCreateCommitRequest request)
    {
        CreateFileRequest createFileRequest = new(request.Message, request.Content);

        await _client.Repository.Content.CreateFile(owner, repoName, request.Path, createFileRequest);

        return ValidationResponse.Success();
    }

    public async Task<ValidationResponse<RepoContent>> GetRepositoryContentAsync(string owner, string repoName, string? path = null)
    {
        List<RepositoryContent>? repoContent = null;

        if (path == null)
        {
            repoContent = (await _client.Repository.Content.GetAllContents(owner, repoName)).ToList();
        }
        else
        {
            var contentBytes = await _client.Repository.Content.GetRawContent(owner, repoName, path);
            var bytesAsString = Convert.ToBase64String(contentBytes);
            repoContent = new List<RepositoryContent> { new RepositoryContent(
                name: path.Split('\\').Last(), 
                path: path, 
                sha: "", 
                size: 0, 
                type: Octokit.ContentType.File, 
                downloadUrl: "", 
                url: "", 
                gitUrl: "", 
                htmlUrl: "", 
                encoding: "", 
                encodedContent: bytesAsString, 
                target: "", 
                submoduleGitUrl: ""
            )};
        }

        if (repoContent == null) return ValidationResponse<RepoContent>.Failure(message: "Failed to get repository content");

        FileType ConvertFileType(Octokit.ContentType contentType) => (contentType) switch
        {
            Octokit.ContentType.File => FileType.File,
            Octokit.ContentType.Dir => FileType.Directory,
            Octokit.ContentType.Symlink => FileType.Symlink,
            Octokit.ContentType.Submodule => FileType.Submodule,
            _ => throw new Exception($"Unknown file type discovered in Repository: {repoName}"),
        };

        string DecodeContent(string content)
        {
            byte[] data = Convert.FromBase64String(content);
            string decodedContent = Encoding.UTF8.GetString(data);
            return decodedContent;
        }

        return ValidationResponse<RepoContent>.Success(repoContent.Select(x => new RepoContent(x.Name, x.Path, DecodeContent(x.EncodedContent), ConvertFileType(x.Type.Value))));
    }

    public async Task<ValidationResponse<GitTreeItem>> GetGitTreeAsync(string owner, string repoName)
    {
        var reference = "refs/heads/master";
        TreeResponse treeResponse = await _client.Git.Tree.GetRecursive(owner, repoName, reference);

        IEnumerable<GitTreeItem> gitTree = BuildGitTreeRecursive(treeResponse.Tree.ToArray());

        return ValidationResponse<GitTreeItem>.Success(gitTree);
    }


    public IEnumerable<GitTreeItem> BuildGitTreeRecursive(TreeItem[] tree, TreeItem? previousItem = null)
    {
        var gitTree = new HashSet<GitTreeItem>();

        GitTreeType ConvertTreeType(TreeType treeType) => (treeType) switch
        {
            TreeType.Blob => GitTreeType.Blob,
            TreeType.Tree => GitTreeType.Tree,
            TreeType.Commit => GitTreeType.Commit,
            _ => throw new Exception($"Unknown tree type discovered in Repository"),
        };

        string GetBasePath(string? path)
        {
            if (path == null) return string.Empty;

            int index = path.LastIndexOf('/');
            return index == -1 ? path : path[..index];
        }

        for (int i = 0; i < tree.Length; i++)
        {
            TreeItem item = tree[i];
            Console.WriteLine(item.Path);
            string itemPrefix = GetBasePath(item.Path);
            if (previousItem != null && !itemPrefix.Equals(previousItem.Path)) break;

            if (item.Type == TreeType.Tree)
            {
                var items = BuildGitTreeRecursive(tree[(i+1)..tree.Length], item);
                var leaf = new GitTreeItem(item.Path, item.Sha, item.Mode, GetFileExtensionFromPath(item.Path), ConvertTreeType(item.Type.Value), items);
                gitTree.Add(leaf);
                var leafCount = GetLeafs(leaf).Count();
                i += leafCount;
            }
            else if (item.Type == TreeType.Blob)
            {
                gitTree.Add(new GitTreeItem(item.Path, item.Sha, item.Mode, GetFileExtensionFromPath(item.Path), ConvertTreeType(item.Type.Value)));
            }
        }

        return gitTree;
    }


    #region Private Methods

    private FileExtension GetFileExtensionFromPath(string path)
    {
        var extension = Path.GetExtension(path);
        return extension switch
        {
            ".pdf" => FileExtension.PDF,
            ".png" => FileExtension.PNG,
            ".jpg" => FileExtension.JPG,
            ".jpeg" => FileExtension.JPEG,
            ".cs" => FileExtension.CSharp,
            ".mp3" => FileExtension.MP3,
            ".json" => FileExtension.Json,
            _ => FileExtension.Text
        };
    }

    private IEnumerable<GitTreeItem> GetLeafs(GitTreeItem source)
    {
        if(source.Items == null) return Enumerable.Empty<GitTreeItem>();

        var list = new List<GitTreeItem>();

        foreach (var item in source.Items)
        {
            list.Add(item);

            foreach (var subchild in GetLeafs(item))
            {
                list.Add(subchild);
            }
        }

        return list;
    }

    private async Task<GithubPublicKey?> GetPublicKeyAsync(string userName, string repoName)
    {
        using var client = new HttpClient();

        HttpRequestMessage request = await BuildRequestAsync(HttpMethod.Get, "actions/secrets/public-key", userName, repoName);

        HttpResponseMessage response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string message = await response.Content.ReadAsStringAsync();
        GithubPublicKey? result = JsonConvert.DeserializeObject<GithubPublicKey>(message);

        return result;
    }

    private async Task<HttpRequestMessage> BuildRequestAsync(HttpMethod method, string endpoint, string userName, string repoName, JsonContent? content = null)
    {
        string pat = await _awsSecretsManagerService.GetSecretAsync(AwsSecret.GithubPat);

        HttpRequestMessage request = new(method, $"https://api.github.com/repos/{userName}/{repoName}/{endpoint}");
        request.Headers.Add("Accept", "application/vnd.github+json");
        request.Headers.Add("Authorization", $"Bearer {pat}");
        request.Headers.Add("X-GitHub-Api-Version", "2022-11-28");
        request.Headers.Add("User-Agent", "request");
        request.Headers.Add("Cookie", "_octo=GH1.1.1578474083.1689685007; logged_in=no");
        if(content != null) request.Content = content;

        return request;
    }

    private string EncodeSecret(string secret, string publicKey)
    {
        var encodedSecret = Encoding.UTF8.GetBytes(secret);
        var encodedPublicKey = Convert.FromBase64String(publicKey);

        var sealedPublicKeyBox = SealedPublicKeyBox.Create(encodedSecret, encodedPublicKey);

        return Convert.ToBase64String(sealedPublicKeyBox);
    }

    private RepositoryModel ParseRepository(Octokit.Repository repo) => new()
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

    #endregion
}
