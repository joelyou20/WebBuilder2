using Microsoft.AspNetCore.Components;

namespace WebBuilder2.Client.Components;

public partial class CustomErrorBoundary : ErrorBoundaryBase
{
    protected override Task OnErrorAsync(Exception exception)
    {
        ErrorObserver.ReportError(exception.Message);
        return Task.CompletedTask;
    }
}
