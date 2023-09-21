using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Components;

public partial class ErrorContainer
{
    [Parameter] public IEnumerable<ApiError> Errors { get; set; } = Enumerable.Empty<ApiError>();

    public Severity ConvertSeverity(ApiErrorSeverity? severity) => severity switch
    {
        ApiErrorSeverity.Information => Severity.Info,
        ApiErrorSeverity.Warning => Severity.Warning,
        ApiErrorSeverity.Error => Severity.Error,
        _ => Severity.Error,
    };
}
