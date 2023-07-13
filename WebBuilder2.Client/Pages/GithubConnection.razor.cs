using Microsoft.AspNetCore.Components;
using WebBuilder2.Client.Managers;
using WebBuilder2.Client.Services;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Pages;

public partial class GithubConnection
{
    [Inject] public IGithubService GithubConnectionService { get; set; } = default!;

    List<GithubRepository> _githubRepositories { get; set; } = new List<GithubRepository>();

    protected override async Task OnInitializedAsync()
    {
        var response = await GithubConnectionService.GetRepositoriesAsync();
        _githubRepositories = response.Repositories.ToList();
    }
}
