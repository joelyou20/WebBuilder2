using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebBuilder2.Client.Components.Dialogs;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Pages;

public partial class Sites
{
    [Inject] public ISiteService SiteService { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;

    public List<Site> siteList = new();

    public int idCounter = 0; //HACK: Temporary until the ids can be stored in a db or json file

    protected override async Task OnInitializedAsync()
    {
        await InitializeSitesAsync();
    }

    private async Task InitializeSitesAsync()
    {
        var sites = await SiteService.GetSitesAsync();

        if (sites == null) return;

        foreach (Site site in sites)
        {
            siteList.Add(site);
        }
    }

    public void OnCreateSiteBtnClicked() => InvokeAsync(async () =>
    {
        var site = new Site($"Test Site {idCounter}");
        siteList.Add(site);
        await SiteService.AddSiteAsync(site);
        idCounter++;
    });

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

    public async void OnDeleteSiteBtnClicked(Site site)
    {
        siteList.Remove(site);
        await SiteService.SoftDeleteSiteAsync(site);
    }

    public void OnSiteCardClicked(Site site)
    {
        NavigationManager.NavigateTo($"site/{site.Name}");
    }
}
