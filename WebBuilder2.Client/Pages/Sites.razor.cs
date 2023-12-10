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
    [Inject] public IAwsService AwsService { get; set; } = default!;
    [Inject] public ISiteService SiteService { get; set; } = default!; // Eventually all calls will go through manager
    [Inject] public IRepositoryService RepositoryService { get; set; } = default!;
    [Inject] public ISiteManager SiteManager { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;

    private List<SiteModel> _siteList = new();
    private List<Domain>? _registeredDomains;
    private List<ApiError> _errors = new();

    protected override async Task OnInitializedAsync()
    {
        await UpdateDomainsAsync();
        await UpdateSitesAsync();
    }

    private async Task UpdateDomainsAsync()
    {
        _registeredDomains = await AwsService.GetRegisteredDomainsAsync();
    }

    private async Task UpdateSitesAsync()
    {
        var sites = await SiteService.GetSitesAsync(new Dictionary<string, string> { { nameof(SiteModel.Id).ToLower(), string.Join(",", _siteList.Select(x => x.Id)) } });

        if (sites == null) return;

        List<SiteModel> newSiteList = new();

        foreach (SiteModel site in sites)
        {
            newSiteList.Add(site);
        }

        _siteList = newSiteList;

        StateHasChanged();
    }

    public void OnCreateSiteBtnClicked() => InvokeAsync(async () =>
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
            { "RegisteredDomains", _registeredDomains }
        };

        var dialog = await DialogService.ShowAsync<CreateSiteDialog>(
            title: "Create New Site",
            options: options,
            parameters: dialogParameters
        );

        await dialog.Result;

        await UpdateSitesAsync();
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

    public async Task OnGetSuggestedDomainNamesClicked()
    {
        DialogOptions options = new()
        {
            CloseOnEscapeKey = true,
            CloseButton = true,
            Position = DialogPosition.Center
        };

        await DialogService.ShowAsync<GetDomainNameSuggestionsDialog>(
            title: "Get Domain Name Suggestion",
            options: options
        );
    }

    public void OnSiteTableValueChanged() => InvokeAsync(UpdateSitesAsync);
}
