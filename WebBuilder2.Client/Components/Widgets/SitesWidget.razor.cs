using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebBuilder2.Client.Managers.Contracts;
using WebBuilder2.Client.Pages;
using WebBuilder2.Client.Services;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Components.Widgets;

public partial class SitesWidget
{
    [Inject] public ISiteService SiteService { get; set; } = default!; // Eventually all calls will go through manager
    [Inject] public ISiteManager SiteManager { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;

    private List<Site> _siteList = new();

    protected override async Task OnInitializedAsync()
    {
        await UpdateSitesAsync();
    }

    private async Task UpdateSitesAsync()
    {
        var sites = await SiteService.GetSitesAsync();

        if (sites == null) return;

        List<Site> newSiteList = new();

        foreach (Site site in sites)
        {
            newSiteList.Add(site);
        }

        _siteList = newSiteList;

        StateHasChanged();
    }

    public void OnSiteTableValueChanged() => InvokeAsync(UpdateSitesAsync);
}
