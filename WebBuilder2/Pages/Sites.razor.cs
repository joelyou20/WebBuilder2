using Microsoft.AspNetCore.Components;
using WebBuilder2.Models;
using WebBuilder2.Services.Contracts;

namespace WebBuilder2.Pages;

public partial class Sites
{
    [Inject] public ISiteService SiteService { get; set; } = default!;

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
        siteList.Add(new Site(idCounter, $"Test Site {idCounter}"));
        idCounter++;
    }

    public void OnDeleteSiteBtnClicked(Site site)
    {
        siteList.Remove(site);
    }
}
