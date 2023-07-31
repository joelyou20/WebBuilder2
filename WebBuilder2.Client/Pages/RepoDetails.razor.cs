using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Client.Utils;
using System.IO;
using WebBuilder2.Shared.Validation;
using MudBlazor;

namespace WebBuilder2.Client.Pages;

public partial class RepoDetails
{
    [Inject] public IRepositoryService RepositoryService { get; set; } = default!;
    [Inject] public IGithubService GithubService { get; set; } = default!;

    [Parameter] public long Id { get; set; }

    private RepositoryModel? _repo;
    private List<ApiError> _errors = new();
    private GitTreeItem? _selectedFile;
    private HashSet<GitTreeItem> _content = new();

    protected override async Task OnInitializedAsync()
    {
        await InitializeDataAsync();
    }

    private async Task InitializeDataAsync()
    {
        _repo = await RepositoryService.GetSingleRepositoryAsync(Id);

        if (_repo == null) throw new KeyNotFoundException($"Repository with Id of {Id} was not found in database.");

        ValidationResponse<GitTreeItem>? gitTreeResponse = await GithubService.GetGitTreeAsync(_repo.Name);

        if (gitTreeResponse == null)
        {
            var errorMessage = $"Failed to retrieve Repository content for repo with name: {_repo.Name}";
            _errors.AddRange(new ApiError[] { new ApiError($"Failed to retrieve Repository content for repo with name: {_repo.Name}") });
            throw new Exception(errorMessage);
        }
        else if (gitTreeResponse != null && !gitTreeResponse.HasValues)
        {
            _errors.AddRange(gitTreeResponse.Errors);
        }
        else
        {
            _content = gitTreeResponse!.GetValues().ToHashSet();
        }

        StateHasChanged();
    }

    public async Task OnUploadFileBtnClicked(IBrowserFile file)
    {
        if(_repo == null) return;

        string fileAsString = await FileReader.ReadFileAsync(file);
        GithubCreateCommitRequest request = new()
        {
            Content = fileAsString,
            Message = $"Add file {file.Name}",
            Path = $"resources/test/{file.Name.Replace(' ', '_')}"
        };
        await GithubService.CreateCommitAsync(request, _repo.Name);
        await InitializeDataAsync();
        StateHasChanged();
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
