using Microsoft.AspNetCore.Components;

namespace WebBuilder2.Client.Components;

public partial class Error
{
    [Inject] public ILogger<Error> Logger { get; set; } = default!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    public void ProcessError(Exception ex)
    {
        Logger.LogError("Error:ProcessError - Type: {Type} Message: {Message}",
            ex.GetType(), ex.Message);
    }
}
