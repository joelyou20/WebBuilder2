using Microsoft.AspNetCore.Components;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Pages;

public partial class Logs
{
    [Inject] public ILogService LogService { get; set; } = default!;

    private List<LogModel>? _logs;
    private bool _isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        _logs = await LogService.GetLogsAsync();
        _isLoading = false;
    }
}
