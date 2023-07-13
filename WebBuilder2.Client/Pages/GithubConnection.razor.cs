using Microsoft.AspNetCore.Components;
using WebBuilder2.Client.Managers;
using WebBuilder2.Client.Services;
using WebBuilder2.Client.Services.Contracts;

namespace WebBuilder2.Client.Pages;

public partial class GithubConnection
{
    [Inject] public IGithubConnectionService GithubConnectionService { get; set; } = default!;

    public void OnConnectBtnClick() => InvokeAsync(async () =>
    {
        await GithubConnectionService.ConnectAsync();
    });
}
