using Microsoft.AspNetCore.Components;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Components.Dialogs;

public partial class AddExistingSiteDialog
{
    private List<Bucket> _buckets = new();
    private List<HostedZone> _hostedZones = new();

    [Inject] public IAwsService AwsService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _buckets = (await AwsService.GetBucketsAsync()).ToList();
        _hostedZones = (await AwsService.GetHostedZonesAsync()).ToList();
    }
}
