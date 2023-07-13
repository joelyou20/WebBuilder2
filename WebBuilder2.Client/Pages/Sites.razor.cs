using Microsoft.AspNetCore.Components;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Pages;

public partial class Sites
{
    [Inject] public ISiteService SiteService { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

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

    public void OnCreateSiteBtnClicked()
    {
        siteList.Add(new Site($"Test Site {idCounter}"));
        idCounter++;
    }

    public void OnAddExistingSiteBtnClicked() => InvokeAsync(async () =>
    {
        await SiteService.GetSitesAsync();

        var x = 1;
    });

    public void OnDeleteSiteBtnClicked(Site site)
    {
        siteList.Remove(site);
    }

    public void OnSiteCardClicked(Site site)
    {
        NavigationManager.NavigateTo($"site/{site.Name}");
    }
}
