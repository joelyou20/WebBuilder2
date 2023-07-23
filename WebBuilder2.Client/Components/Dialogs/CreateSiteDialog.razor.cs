using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using WebBuilder2.Client.Managers.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Components.Dialogs;

public partial class CreateSiteDialog
{
    [Inject] public ISiteManager SiteManager { get; set; } = default!;

    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = default!;

    private CreateSiteRequest _createSiteRequest = new();

    public void OnCreateBtnClick() => InvokeAsync(async () =>
    {
        await SiteManager.CreateSiteAsync(_createSiteRequest);
        MudDialog.Close(DialogResult.Ok(true));
    });
}
