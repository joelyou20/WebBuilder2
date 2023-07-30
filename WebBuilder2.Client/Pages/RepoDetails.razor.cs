using Microsoft.AspNetCore.Components;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Pages;

public partial class RepoDetails
{
    [Inject] public IRepositoryService RepositoryService { get; set; } = default!;

    [Parameter] public long Id { get; set; }

    private RepositoryModel? _repo;

    protected override async Task OnInitializedAsync()
    {
        _repo = await RepositoryService.GetSingleRepositoryAsync(Id);

        if (_repo == null) throw new KeyNotFoundException($"Repository with Id of {Id} was not found in database.");
        StateHasChanged();
    }
}
