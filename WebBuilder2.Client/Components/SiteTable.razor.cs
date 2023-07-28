using Microsoft.AspNetCore.Components;
using WebBuilder2.Client.Managers.Contracts;
using WebBuilder2.Client.Services;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Components;

public partial class SiteTable
{
    [Inject] public ISiteService SiteService { get; set; } = default!; // Eventually all calls will go through manager
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    [Parameter] public List<SiteModel> Sites { get; set; } = new();
    [Parameter] public EventCallback ValuedChanged { get; set; } = new();

    public void OnDeleteSiteBtnClicked(SiteModel site) => InvokeAsync(async () =>
    {
        Sites.Remove(site);
        await SiteService.SoftDeleteSiteAsync(site);
        await ValuedChanged.InvokeAsync();
    });

    public void OnSiteCardClicked(SiteModel site)
    {
        NavigationManager.NavigateTo($"sites/{site.Id}");
    }
}
