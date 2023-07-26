using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebBuilder2.Client.Components.Dialogs;
using WebBuilder2.Client.Services;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Components;

public partial class GithubRepoTable
{
    [Inject] public IRepositoryService RepositoryService { get; set; } = default!;

    [Parameter, EditorRequired] public List<RepositoryModel> Repositories { get; set; } = new();
    [Parameter] public bool ShowConnectButton { get; set; } = false;
    [Parameter] public bool ShowDeleteButton { get; set; } = false;
    [Parameter] public bool ShowAddTemplateButton { get; set; } = false;
    [Parameter] public EventCallback ValueChanged { get; set; } = default!;
    [Parameter] public bool IsLoading { get; set; } = true; 
    [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> Attributes { get; set; } = new();

    public async Task OnConnectRepoButtonClicked(RepositoryModel repository)
    {
        await ValueChanged.InvokeAsync();
    }

    public async Task OnDeleteRepoButtonClicked(RepositoryModel repository)
    {
        await RepositoryService.SoftDeleteRepositoryAsync(repository);

        await ValueChanged.InvokeAsync();
    }

    public async Task OnAddTemplateButtonClicked(RepositoryModel repository)
    {
        repository.IsTemplate = true;
        await RepositoryService.UpdateRepositoryAsync(repository);
        await ValueChanged.InvokeAsync();
        StateHasChanged();
    }
}
