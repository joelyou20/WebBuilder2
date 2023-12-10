using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text;
using WebBuilder2.Client.Observers;
using WebBuilder2.Client.Observers.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Components;

public partial class ErrorContainer
{
    [Inject] public IErrorObserver ErrorObserver { get; set; } = default!;

    [Parameter] public List<ApiError> Errors { get; set; } = new();

    protected override void OnInitialized()
    {
        ErrorObserver.ErrorsChanged += ErrorObserver_ErrorsChanged;
    }

    private void ErrorObserver_ErrorsChanged(object? sender, List<ApiError> e)
    {
        Errors = e;
        StateHasChanged();
    }

    public Severity ConvertSeverity(ApiErrorSeverity? severity) => severity switch
    {
        ApiErrorSeverity.Information => Severity.Info,
        ApiErrorSeverity.Warning => Severity.Warning,
        ApiErrorSeverity.Error => Severity.Error,
        _ => Severity.Error,
    };

    public string BuildErrorMessage(ApiError error)
    {
        StringBuilder sb = new();
        if (!string.IsNullOrEmpty(error.Code)) sb.Append($"Code: {error.Code}");
        if (!string.IsNullOrEmpty(error.Message)) sb.Append($"Message: {error.Message}");
        if (!string.IsNullOrEmpty(error.Resource)) sb.Append($"Resource: {error.Resource}");
        if (!string.IsNullOrEmpty(error.Field)) sb.Append($"Field: {error.Field}");

        return string.Join("; ", sb);
    }

    public void RemoveError(ApiError error)
    {
        Errors.Remove(error);
        StateHasChanged();
    }
}
