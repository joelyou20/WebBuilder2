using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Components.Dialogs;

public partial class AddSSLCertificateDialog
{
    [Inject] public IAwsService AwsService { get; set; } = default!;
    [Inject] public ISiteService SiteService { get; set; } = default!;

    [Parameter] public SiteModel Site { get; set; } = default!;
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = default!;

    private AwsNewSSLCertificateRequest _request { get; set; } = new();

    private List<string> _alternateDomainNames { get; set; } = new();

    public void OnValidSubmit() => InvokeAsync(async () =>
    {
        _request.AlternativeNames = _alternateDomainNames;
        AwsNewSSLCertificateResponse? response = await AwsService.PostNewSSLCertificateAsync(_request);

        if(response != null)
        {
            SiteModel? updateResponse = await SiteService.UpdateSiteAsync(Site);

            if (updateResponse == null) MudDialog.Close(DialogResult.Ok(false));
        }

        MudDialog.Close(DialogResult.Ok(true));
    });

    public void OnAddAlternateDomainName()
    {
        _alternateDomainNames.Add("");
        StateHasChanged();
    }

    public void OnRemoveAlternateDomainName(string domainName)
    {
        _alternateDomainNames.Remove(domainName);
        StateHasChanged();
    }
}
