using Microsoft.AspNetCore.Components;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Components.Widgets;

public partial class GithubRepoWidget
{
    // TODO: This widget is very similar in code to the GithubConnectionsPage. Revisit this later to see if this can be cleaned up.

    [Inject] public IGithubService GithubService { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    List<Repository> _githubRepositories { get; set; } = new List<Repository>();

    private bool _isTableLoading = true;

    protected override async Task OnInitializedAsync()
    {
        await UpdateReposAsync();
    }

    public async Task UpdateReposAsync()
    {
        var authenticateResponse = await GithubService.PostAuthenticateAsync(new GithubAuthenticationRequest(""));

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
