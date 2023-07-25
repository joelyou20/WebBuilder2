using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebBuilder2.Client.Components.Dialogs;
using WebBuilder2.Client.Managers.Contracts;
using WebBuilder2.Client.Services;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Pages;

public partial class Sites
{
    [Inject] public ISiteService SiteService { get; set; } = default!; // Eventually all calls will go through manager
    [Inject] public IRepositoryService RepositoryService { get; set; } = default!;
    [Inject] public ISiteManager SiteManager { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;

    private List<Site> _siteList = new();

    protected override async Task OnInitializedAsync()
    {
        await UpdateSitesAsync();
    }

    public void OnCreateSiteBtnClicked() => InvokeAsync(async () =>
    {
        DialogOptions options = new()
        {
            CloseOnEscapeKey = true,
            CloseButton = true,
            Position = DialogPosition.Center,
            
        };

        var dialog = await DialogService.ShowAsync<CreateSiteDialog>(
            title: "Create New Site",
            options: options
        );

        await dialog.Result;

        await UpdateSitesAsync();
    });

    private async Task UpdateSitesAsync()
    {
        var sites = await SiteService.GetSitesAsync(_siteList.Select(x => x.Id));

        if (sites == null) return;

        List<Site> newSiteList = new();

        foreach (Site site in sites)
        {
            newSiteList.Add(site);
        }

        _siteList = newSiteList;

        StateHasChanged();
    }

    public void OnAddExistingSiteBtnClicked() => InvokeAsync(async () =>
    {
        DialogOptions options = new()
        {
            CloseOnEscapeKey = true,
            CloseButton = true,
            Position = DialogPosition.Center
        };

        await DialogService.ShowAsync<AddExistingSiteDialog>(
            title: "Add Existing Site",
            options: options
        );
    });

    public void OnSiteTableValueChanged() => InvokeAsync(UpdateSitesAsync);
}
