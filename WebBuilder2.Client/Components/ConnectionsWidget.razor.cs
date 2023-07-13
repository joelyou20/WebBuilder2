using Microsoft.AspNetCore.Components;

namespace WebBuilder2.Client.Components;

public partial class ConnectionsWidget
{
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    public void OnGithubConnectionBtnClick()
    {
        NavigationManager.NavigateTo("connections/github");
    }
}
