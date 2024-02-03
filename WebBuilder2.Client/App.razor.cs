using Microsoft.AspNetCore.Components;
using WebBuilder2.Client.Managers.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client;

public partial class App
{
    [Inject] public IGithubService GithubService { get; set; } = default!;
    [Inject] public IAwsService AwsService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await GithubService.PostAuthenticateAsync();
    }
}
