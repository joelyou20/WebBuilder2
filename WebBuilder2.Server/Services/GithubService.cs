using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Octokit;
using Sodium;
using System.Net;
using System.Text;
using WebBuilder2.Server.Clients.Contracts;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Server.Utils;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Utils;

namespace WebBuilder2.Server.Services;

public class GithubService(IGitHubClient client, IGitHubCustomClient customClient, IAwsSecretsManagerService awsSecretsManagerService) : IGithubService
{
    private readonly IGitHubClient _client = client;
    private readonly IGitHubCustomClient _customClient = customClient;
    private readonly IAwsSecretsManagerService _awsSecretsManagerService = awsSecretsManagerService;

    public async Task<IEnumerable<RepositoryModel>> GetRepositoriesAsync()
    {
        List<RepositoryModel> repos = [];

        var repositories = await _client.Repository.GetAllForCurrent();

        foreach (var repo in repositories)
        {
            repos.Add(ParseRepository(repo));
        }

        return repos;
    }

    public async Task AuthenticateUserAsync()
    {
        // Check if client is already authenticated. _client.User.Current() will throw an AuthorizationException if the client is not authenticated
        await _client.User.Current();
    }

    public async Task<RepositoryModel> CreateRepoAsync(RepositoryModel repository)
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

        return response;
    }

    public async Task<GitIgnoreTemplateResponse> GetGitIgnoreTemplatesAsync()
    {
        IReadOnlyList<string> templates = await _client.GitIgnore.GetAllGitIgnoreTemplates();
        
        if (templates == null) throw new ArgumentNullException(nameof(templates));
        
        GitIgnoreTemplateResponse response = new(templates);
        return response;
    }

    public async Task<IEnumerable<GithubProjectLicense>> GetLicenseTemplatesAsync()
    {
        IReadOnlyList<LicenseMetadata> licenses = await _client.Licenses.GetAllLicenses();

        return licenses.Select(license => new GithubProjectLicense
        {
            Featured = license.Featured,
            Key = license.Key,
            Name = license.Name,
            Url = license.Url
        });
    }

    public async Task<IEnumerable<GithubSecret>> GetSecretsAsync(string userName, string repoName)
    {
        ArgumentNullException.ThrowIfNull(userName);
        ArgumentNullException.ThrowIfNull(repoName);

        string pat = await _awsSecretsManagerService.GetSecretAsync(AwsSecret.GithubPat);
        var result = await _customClient.GetGithubSecrets(userName, repoName, pat);

        return result;
    }

    public async Task CreateSecretAsync(IEnumerable<GithubSecret> secrets, string userName, string repoName)
    {
        ArgumentNullException.ThrowIfNull(secrets);
        ArgumentNullException.ThrowIfNull(userName);
        ArgumentNullException.ThrowIfNull(repoName);

        if (!secrets.Any()) throw new Exception("Empty list provided");

        string pat = await _awsSecretsManagerService.GetSecretAsync(AwsSecret.GithubPat);
        
        foreach (GithubSecret secret in secrets)
        {
            if (secret.Value == null) throw new ArgumentNullException($"Github Secret: {secret.Name} has no value.");

            await _customClient.CreateSecretAsync(userName, repoName, pat, secret);
        }
    }

    public async Task<string> GetUserAsync()
    {
        var user = await _client.User.Current();
        return user.Login;
    }

    public async Task<Reference> CreateBranchAsync(string owner, long repoId, string branchName, Commit? commit = null)
    {
        ArgumentNullException.ThrowIfNull(owner);
        ArgumentNullException.ThrowIfNull(repoId);
        ArgumentNullException.ThrowIfNull(branchName);

        commit ??= await GetMasterRefAsync(owner, repoId);
        NewReference reference = new($"refs/heads/{branchName}", commit.Sha);
        IReadOnlyList<Reference> branches = await _client.Git.Reference.GetAll(repoId);
        Reference? existingBranch = branches.FirstOrDefault(x => x.Ref == reference.Ref);

        if (existingBranch != null) return existingBranch;

        Reference branch = await _client.Git.Reference.Create(repoId, reference);

        return branch;
    }

    public async Task CreateCommitAsync(string owner, long repoId, GithubCreateCommitRequest request)
    {
        foreach (NewFile file in request.Files)
        {
            CreateFileRequest createFileRequest = new(request.Message, file.Content);
            await _client.Repository.Content.CreateFile(repoId, file.Path, createFileRequest);
        }
    }

    public async Task<IEnumerable<RepoContent>> GetRepositoryContentAsync(string owner, string repoName, string? path = null)
    {
        List<RepositoryContent>? repoContentList = null;

        if (path == null)
        {
            repoContentList = [.. (await _client.Repository.Content.GetAllContents(owner, repoName))];
        }
        else
        {
            byte[] contentBytes = await _client.Repository.Content.GetRawContent(owner, repoName, path);
            var bytesAsString = Convert.ToBase64String(contentBytes);
            repoContentList = [ new RepositoryContent(
                name: path.Split('\\').Last(), 
                path: path, 
                sha: "", 
                size: 0, 
                type: ContentType.File, 
                downloadUrl: "", 
                url: "", 
                gitUrl: "", 
                htmlUrl: "", 
                encoding: "", 
                encodedContent: bytesAsString, 
                target: "", 
                submoduleGitUrl: ""
            )];
        }

        FileType ConvertFileType(ContentType contentType) => (contentType) switch
        {
            ContentType.Dir => FileType.Directory,
            ContentType.Symlink => FileType.Symlink,
            ContentType.Submodule => FileType.Submodule,
            _ => FileType.File
        };

        string DecodeContent(string content)
        {
            byte[] data = Convert.FromBase64String(content);
            string decodedContent = Encoding.UTF8.GetString(data);
            return decodedContent;
        }

        return repoContentList.Select(x => new RepoContent(x.Name, x.Path, DecodeContent(x.EncodedContent), ConvertFileType(x.Type.Value)));
    }

    public async Task<IEnumerable<GitTreeItem>> GetGitTreeAsync(string owner, string repoName)
    {
        var reference = "refs/heads/master";
        TreeResponse treeResponse = await _client.Git.Tree.GetRecursive(owner, repoName, reference);

        IEnumerable<GitTreeItem> gitTree = BuildGitTreeRecursive(treeResponse.Tree.ToArray());

        return gitTree;
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
                var leaf = new GitTreeItem(item.Path, item.Sha, item.Mode, FileExtensionHelpers.GetFileExtensionFromPath(item.Path), ConvertTreeType(item.Type.Value), items);
                gitTree.Add(leaf);
                var leafCount = GetLeafs(leaf).Count();
                i += leafCount;
            }
            else if (item.Type == TreeType.Blob)
            {
                gitTree.Add(new GitTreeItem(item.Path, item.Sha, item.Mode, FileExtensionHelpers.GetFileExtensionFromPath(item.Path), ConvertTreeType(item.Type.Value)));
            }
        }

        return gitTree;
    }

    // OctoKit has not implemented functionality for copying the contents of one repo into another, so I am handling
    //  that by using commands
    public async Task<bool> CopyRepoAsync(string clonedRepoName, string newRepoName, string? path)
    {
        User user = await _client.User.Current();
        string currentDirectory = Directory.GetCurrentDirectory();
#if TEST
        string scriptPath = @".\test.sh";
#else
        string scriptPath = @".\Scripts\CopyGitRepo.sh";
#endif
        bool isSuccessful = await ScriptRunner.RunAsync(scriptPath, [clonedRepoName, newRepoName, user.Login]);

        return isSuccessful;
    }


    #region Private Methods

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
        string pat = await _awsSecretsManagerService.GetSecretAsync(AwsSecret.GithubPat);
        var result = await _customClient.GetPublicKeyAsync(userName, repoName, pat);

        return result;
    }

    private RepositoryModel ParseRepository(Repository repo) => new()
    {
        AllowAutoMerge = repo.AllowAutoMerge ?? false,
        AllowMergeCommit = repo.AllowMergeCommit ?? false,
        AllowRebaseMerge = repo.AllowRebaseMerge ?? false,
        AllowSquashMerge = repo.AllowSquashMerge ?? false,
        CreatedDateTime = repo.CreatedAt.DateTime,
        DeleteBranchOnMerge = repo.DeleteBranchOnMerge ?? false,
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

    private async Task<Commit> GetMasterRefAsync(string owner, long repoId)
    {
        var headMasterRef = "heads/master";

        // Get reference of master branch
        Reference masterReference = await _client.Git.Reference.Get(repoId, headMasterRef);

        // Get the laster commit of this branch
        return await _client.Git.Commit.Get(repoId, masterReference.Object.Sha);
    }

    #endregion
}
