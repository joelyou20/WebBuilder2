using Microsoft.AspNetCore.Components;
using WebBuilder2.Client.Services;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models.Dtos;

namespace WebBuilder2.Client.Pages;

public partial class Logs
{
    [Inject] public ILogService LogService { get; set; } = default!;

    private List<LogModel>? _logs;
    private bool _isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        var response = await LogService.GetLogsAsync();

        if (response == null) throw new ArgumentNullException(nameof(response));

        _logs = response.ToList();
        _isLoading = false;
    }
}
