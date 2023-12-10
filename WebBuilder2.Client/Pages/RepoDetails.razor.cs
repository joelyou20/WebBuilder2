using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Client.Utils;
using System.IO;
using WebBuilder2.Shared.Validation;
using MudBlazor;
using WebBuilder2.Client.Components;
using System.Text;
using WebBuilder2.Client.Managers.Contracts;
using Blace.Editing;

namespace WebBuilder2.Client.Pages;

public partial class RepoDetails
{
    [Inject] public IRepositoryService RepositoryService { get; set; } = default!;
    [Inject] public IGithubService GithubService { get; set; } = default!;
    [Inject] public IRepositoryManager RepositoryManager { get; set; } = default!;

    [Parameter] public long Id { get; set; }

    private RepositoryModel? _repo;
    private List<ApiError> _errors = new();
    private HashSet<GitTreeItem> _repoTree = new();
    private string _fileContent = string.Empty;
    private CodeEditor? _codeEditor;

    protected override async Task OnInitializedAsync()
    {
        await InitializeDataAsync();
    }

    private async Task InitializeDataAsync()
    {
        _repo = await RepositoryService.GetSingleRepositoryAsync(Id);

        if (_repo == null) throw new KeyNotFoundException($"Repository with Id of {Id} was not found in database.");

        List<GitTreeItem>? gitTreeResponse = await GithubService.GetGitTreeAsync(_repo.Name);

        _repoTree = gitTreeResponse!.ToHashSet();

        StateHasChanged();
    }

    public async Task OnUploadFileBtnClicked(IBrowserFile file)
    {
        if(_repo == null) return;
        await RepositoryManager.CreateCommitAsync(file, _repo);
        await InitializeDataAsync();
        StateHasChanged();
    }

    public async Task OnSelectedFileChanged(GitTreeItem item)
    {
        if(_repo == null || item == null || item.Type == GitTreeType.Tree) return;

        RepoContent? repoContent = await GithubService.GetRepositoryContentAsync(_repo.Name, item.Path);

        if(repoContent == null) throw new Exception("Failed to get repository content");

        _fileContent = repoContent.Content;

        if(_codeEditor != null) await _codeEditor.Refresh(_fileContent, GetSyntax(item.Path));

        StateHasChanged();
    }

    private Syntax GetSyntax(string fileName)
    {
        var extension = Path.GetExtension(fileName);
        return extension switch
        {
            ".cs" => Syntax.Csharp,
            ".yaml" => Syntax.Yaml,
            ".css" => Syntax.Css,
            ".js" => Syntax.Javascript,
            ".ps1" => Syntax.Powershell,
            ".md" => Syntax.Markdown,
            ".json" => Syntax.Json,
            ".html" => Syntax.Html,
            _ => Syntax.Text,
        };
    }

    private string GetIcon(GitTreeItem item) => item.Type switch
    {
        GitTreeType.Blob => item.Extension switch
        {
            FileExtension.PDF => Icons.Custom.FileFormats.FilePdf,
            FileExtension.MP3 => Icons.Custom.FileFormats.FileMusic,
            FileExtension.PNG => Icons.Custom.FileFormats.FileImage,
            FileExtension.JPG => Icons.Custom.FileFormats.FileImage,
            FileExtension.JPEG => Icons.Custom.FileFormats.FileImage,
            _ => Icons.Custom.FileFormats.FileCode
        },
        GitTreeType.Tree => item.IsExpanded ? Icons.Custom.Uncategorized.FolderOpen : Icons.Custom.Uncategorized.Folder,
        GitTreeType.Commit => Icons.Material.Filled.Cloud,
        _ => throw new NotImplementedException(),
    };
}
