using Microsoft.AspNetCore.Components;
using WebBuilder2.Client.Managers;
using WebBuilder2.Client.Services;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Pages;

public partial class GithubConnection
{
    [Inject] public IGithubService GithubConnectionService { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    List<Repository> _githubRepositories { get; set; } = new List<Repository>();

    private bool _isTableLoading = true;

    protected override async Task OnInitializedAsync()
    {
        var authenticateResponse = await GithubConnectionService.PostAuthenticateAsync(new GithubAuthenticationRequest(""));

        if (authenticateResponse != null && authenticateResponse.IsAuthenticated)
        {
            var response = await GithubConnectionService.GetRepositoriesAsync();
            _githubRepositories = response.Repositories.ToList();
            _isTableLoading = false;
        }
        else
        {
            NavigationManager.NavigateTo($"github/auth");
        }
    }

    public void OnConnectRepoButtonClicked(Repository repository)
    {

    }
}
