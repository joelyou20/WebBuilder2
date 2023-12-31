﻿using Microsoft.AspNetCore.Components;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Pages;

public partial class SiteDetails
{
    [Inject] public ISiteService SiteService { get; set; } = default!;

    [Parameter] public long SiteId { get; set; }

    private SiteModel? _site;

    protected override async Task OnInitializedAsync()
    {
        _site = await SiteService.GetSingleSiteAsync(SiteId);
    }
}
