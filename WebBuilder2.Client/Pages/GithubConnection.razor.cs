using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebBuilder2.Client.Components;
using WebBuilder2.Client.Components.Dialogs;
using WebBuilder2.Client.Managers;
using WebBuilder2.Client.Services;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Pages;

public partial class GithubConnection
{
    [Inject] public IGithubService GithubService { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;

    private List<Repository> _githubRepositories { get; set; } = new List<Repository>();
    private List<GithubTemplate> _templates = new();

    private bool _isTableLoading = true;

    protected override async Task OnInitializedAsync()
    {
        await UpdateReposAsync();
        await UpdateTemplatesAsync();
    }

    private async Task UpdateTemplatesAsync()
    {
        _templates = new List<GithubTemplate>
        {
            new GithubTemplate
            {
                Name = "TEST1"
            },
            new GithubTemplate
            {
                Name = "TEST2"
            },
            new GithubTemplate
            {
                Name = "TEST3"
            }
        };
    }

    public async Task OnCreateRepoBtnClick()
    {
        DialogOptions options = new()
        {
            CloseOnEscapeKey = true,
            CloseButton = true,
            Position = DialogPosition.Center,

        };

        var dialog = await DialogService.ShowAsync<CreateGithubRepoDialog>(
            title: "Create New Repo",
            options: options
        );

        await dialog.Result;

        await UpdateReposAsync();
    }

    public async Task OnCreateTemplateBtnClick()
    {

    }

    public async Task UpdateReposAsync()
    {
        var authenticateResponse = await GithubService.PostAuthenticateAsync(new GithubAuthenticationRequest(""));

        if (authenticateResponse != null && authenticateResponse.IsAuthenticated)
        {
            var response = await GithubService.GetRepositoriesAsync();
            _githubRepositories = response.Repositories.ToList();
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
