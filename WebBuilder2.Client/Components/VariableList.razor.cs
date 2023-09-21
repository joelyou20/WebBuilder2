using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebBuilder2.Client.Components.Dialogs;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Components;

public partial class VariableList
{
    [Inject] public IRepositoryService RepositoryService { get; set; } = default!;
    [Inject] public IGithubService GithubService { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;

    private string _repoName = string.Empty;
    private readonly Func<RepositoryModel, string> _repoSelectConverter = r => r.Name;
    private List<GithubSecret> _variables = new();
    private List<ApiError> _errors = new();
    private List<RepositoryModel> _repositories = new();
    private RepositoryModel? _selectedRepo;

    protected override async Task OnInitializedAsync()
    {
        await InitializeRepoSelectAsync();
        await UpdateDataAsync();
    }

    private async Task InitializeRepoSelectAsync()
    {
        _repositories = await RepositoryService.GetRepositoriesAsync();

        _repoName = _repositories.First().Name;
        _selectedRepo = _repositories.First();
        StateHasChanged();
    }

    private async Task UpdateDataAsync()
    {
        if (_repositories == null || !_repositories.Any()) return;
        var result = await GithubService.GetSecretsAsync(_repoName);

        if(!result.IsSuccessful)
        {
            _errors.AddRange(result.Errors);
        }
        else
        {
            _variables = result.GetValues().SelectMany(x => x.GithubSecrets).ToList();
        }

        StateHasChanged();
    }

    public async Task OnRepoSelected(RepositoryModel repositoryModel)
    {
        _repoName = repositoryModel.Name;
        await UpdateDataAsync();
    }

    public async Task OnCreateVariableButtonClick()
    {

        DialogOptions options = new()
        {
            CloseOnEscapeKey = true,
            CloseButton = true,
            Position = DialogPosition.Center,
            FullWidth = true
        };

        DialogParameters dialogParameters = new()
        {
            { "RepositoryNames", _repositories.Select(x => x.Name).ToList() }
        };

        var dialog = await DialogService.ShowAsync<CreateGithubVariableDialog>(
            title: "Create New Variable",
            options: options,
            parameters: dialogParameters
        );

        await dialog.Result;

        await UpdateDataAsync();
    }
}
