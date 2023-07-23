using Amazon.Runtime.Internal.Transform;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebBuilder2.Client.Components;
using WebBuilder2.Client.Components.Dialogs;
using WebBuilder2.Client.Managers;
using WebBuilder2.Client.Services;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Pages;

public partial class GithubConnection
{
    [Inject] public IGithubService GithubService { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;

    private List<Repository> _githubRepositories { get; set; } = new List<Repository>();
    private List<Repository> _templates => _githubRepositories.Where(x => x.IsTemplate).ToList();

    private bool _isTableLoading = true;

    protected override async Task OnInitializedAsync()
    {
        await UpdateReposAsync();
    }

    public async Task OnCreateRepoBtnClick()
    {
        DialogOptions options = new()
        {
            CloseOnEscapeKey = true,
            CloseButton = true,
            Position = DialogPosition.Center,
            FullWidth = true
        };

        var dialog = await DialogService.ShowAsync<CreateGithubRepoDialog>(
            title: "Create New Repo",
            options: options
        );

        await dialog.Result;

        await UpdateReposAsync();
    }

    public async Task OnImportRepoBtnClick()
    {
        DialogOptions options = new()
        {
            CloseOnEscapeKey = true,
            CloseButton = true,
            Position = DialogPosition.Center,
            FullWidth = true
        };

        var dialog = await DialogService.ShowAsync<CreateGithubRepoDialog>(
            title: "Create New Repo",
            options: options
        );

        await dialog.Result;

        await UpdateReposAsync();
    }

    public async Task UpdateReposAsync()
    {
        ValidationResponse authenticateResponse = await GithubService.PostAuthenticateAsync(new GithubAuthenticationRequest(""));

        if (authenticateResponse != null && authenticateResponse.IsSuccessful)
        {
            var response = await GithubService.GetRepositoriesAsync();
            _githubRepositories = response.GetValues();
            _isTableLoading = false;
            StateHasChanged();
        }
        else
        {
            NavigationManager.NavigateTo($"/github/auth/{Uri.EscapeDataString(NavigationManager.Uri)}");
        }
    }

    public void OnRepoTableValueChanged() => InvokeAsync(UpdateReposAsync);
}
