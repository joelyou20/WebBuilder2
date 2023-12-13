using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebBuilder2.Client.Components.Dialogs;
using WebBuilder2.Client.Managers.Contracts;
using WebBuilder2.Client.Services;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Components;

public partial class SiteTable
{
    [Inject] public ISiteService SiteService { get; set; } = default!; // Eventually all calls will go through manager
    [Inject] public IRepositoryService RepositoryService { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;

    [Parameter] public List<SiteModel> Sites { get; set; } = new();
    [Parameter] public EventCallback ValueChanged { get; set; } = new();

    public void OnDeleteSiteBtnClicked(SiteModel site) => InvokeAsync(async () =>
    {
        Sites.Remove(site);
        // TODO: For the time being I will be soft deleting connected repos when sites are deleted
        if(site.Repository != null) await RepositoryService.SoftDeleteRepositoryAsync(site.Repository);
        await SiteService.SoftDeleteSiteAsync(site);
        await ValueChanged.InvokeAsync();
    });

    public void OnAddSSLCertBtnClicked(SiteModel site) => InvokeAsync(async () =>
    {
        DialogOptions options = new()
        {
            CloseOnEscapeKey = true,
            CloseButton = true,
            Position = DialogPosition.Center,
            FullWidth = true,
        };

        DialogParameters dialogParameters = new()
        {
            { "Site", site }
        };

        var dialog = await DialogService.ShowAsync<AddSSLCertificateDialog>(
            title: "Add New SSL Certificate to Site",
            options: options,
            parameters: dialogParameters
        );

        await dialog.Result;
    });

    public void OnSiteCardClicked(SiteModel site)
    {
        NavigationManager.NavigateTo($"sites/{site.Id}");
    }
}
