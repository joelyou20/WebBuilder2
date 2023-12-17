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

    [Parameter] public SiteModel? Site { get; set; }
    [Parameter] public bool IsReadOnly { get; set; } = true;
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = default!;

    private AwsNewSSLCertificateRequest _request { get; set; } = new();

    public void OnValidSubmit() => InvokeAsync(async () =>
    {
        _request.AlternativeNames = new List<string>
        {
            _request.DomainName.Replace("www.", "")
        };

        AwsNewSSLCertificateResponse? response = await AwsService.PostNewSSLCertificateAsync(_request);

        if (response != null && Site != null)
        {
            Site.SSLCertificateIssueDate = DateTime.Now;
            Site.SSLARN = response.Arn;
            SiteModel? updateResponse = await SiteService.UpdateSiteAsync(Site);
        }

        MudDialog.Close(DialogResult.Ok(true));
    });
}
