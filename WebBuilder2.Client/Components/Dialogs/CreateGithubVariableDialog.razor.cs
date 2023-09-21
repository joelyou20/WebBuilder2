using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebBuilder2.Client.Services;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebBuilder2.Client.Components.Dialogs;

public partial class CreateGithubVariableDialog
{
    [Inject] public IGithubService GithubService { get; set; } = default!;

    [Parameter] public IEnumerable<string> RepositoryNames { get; set; } = Enumerable.Empty<string>();
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = default!;

    private GithubSecret _secret = new GithubSecret();
    private string _selectedRepo = string.Empty;
    private List<ApiError> _errors = new();

    protected override void OnInitialized()
    {
        _selectedRepo = RepositoryNames.First();
        StateHasChanged();
    }

    public void OnRepoSelected(string repo) => _selectedRepo = repo;

    public async Task OnValidSubmit()
    {
        ValidationResponse<GithubSecret> response = await GithubService.CreateSecretAsync(_secret, _selectedRepo);

        if (response.IsSuccessful)
        {
            MudDialog.Close(DialogResult.Ok(true));
        }
        else
        {
            _errors.AddRange(response.Errors);
        }

        StateHasChanged();
    }
}
