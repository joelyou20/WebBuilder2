using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebBuilder2.Client.Services;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Components.Dialogs;

public partial class ImportGithubRepoDialog
{
    [Inject] public IGithubService GithubService { get; set; } = default!;
    [Inject] public IRepositoryService RepositoryService { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    [Parameter] public IEnumerable<long> ExistingIds { get; set; } = default!;
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = default!;

    private Dictionary<Repository, bool> _githubRepositories = new();

    private bool _dataIsLoading = true;

    protected override async Task OnInitializedAsync()
    {
        await UpdateReposAsync();
    }

    public async Task UpdateReposAsync()
    {
        ValidationResponse authenticateResponse = await GithubService.PostAuthenticateAsync(new GithubAuthenticationRequest(""));

        if (authenticateResponse != null && authenticateResponse.IsSuccessful)
        {
            var response = await GithubService.GetRepositoriesAsync();
            response.GetValues().ForEach(x =>
            {
                if(!ExistingIds.Contains(x.Id)) _githubRepositories.Add(x, false);
            });
            _dataIsLoading = false;
            StateHasChanged();
        }
        else
        {
            NavigationManager.NavigateTo($"/github/auth/{Uri.EscapeDataString(NavigationManager.Uri)}");
        }
    }

    private void OnCheckboxChecked(Repository repo)
    {
        _githubRepositories[repo] = !_githubRepositories[repo];
    }

    private void OnImportBtnClick() => InvokeAsync(async () =>
    {
        foreach (Repository? repo in _githubRepositories.Where(x => x.Value).Select(x => x.Key))
        {
            if(repo != null) await RepositoryService.AddRepositoryAsync(repo);
        }

        MudDialog.Close(DialogResult.Ok(true));
    });
}
