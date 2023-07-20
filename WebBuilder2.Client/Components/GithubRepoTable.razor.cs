using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebBuilder2.Client.Components.Dialogs;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Components;

public partial class GithubRepoTable
{
    [Parameter, EditorRequired] public List<Repository> Repositories { get; set; } = new();
    [Parameter] public bool ShowConnectButton { get; set; } = false;
    [Parameter] public bool ShowDeleteButton { get; set; } = false;
    [Parameter] public EventCallback ValueChanged { get; set; } = default!;
    [Parameter] public bool IsLoading { get; set; } = true; 
    [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> Attributes { get; set; } = new();

    public async Task OnConnectRepoButtonClicked(Repository repository)
    {
        await ValueChanged.InvokeAsync();
    }

    public async void OnDeleteRepoButtonClicked(Repository repository)
    {
        //TODO: Implement this later... Too scared to accidentally delete an important repo. Maybe implement soft delete and store github data in db?
        //var result = await GithubService.PostDeleteRepoAsync(repo);

        await ValueChanged.InvokeAsync();
    }
}
