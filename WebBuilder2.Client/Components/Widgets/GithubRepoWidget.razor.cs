using Microsoft.AspNetCore.Components;
using WebBuilder2.Client.Services;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Components.Widgets;

public partial class GithubRepoWidget
{
    // TODO: This widget is very similar in code to the GithubConnectionsPage. Revisit this later to see if this can be cleaned up.

    [Inject] public IRepositoryService RepositoryService { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    List<RepositoryModel> _repositories { get; set; } = new List<RepositoryModel>();

    private bool _isTableLoading = true;

    protected override async Task OnInitializedAsync()
    {
        await UpdateReposAsync();
    }

    public async Task UpdateReposAsync()
    {
        var response = await RepositoryService.GetRepositoriesAsync();
        _repositories = response;
        _isTableLoading = false;
        StateHasChanged();
    }

    public void OnRepoTableValueChanged() => InvokeAsync(UpdateReposAsync);
}
