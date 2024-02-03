using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Pages;

public partial class GithubLogin
{
    [Inject] public IGithubService GithubService { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    [Parameter] public string ReturnUrl { get; set; } = string.Empty;


    private void OnValidSubmit(EditContext context) => InvokeAsync(async () =>
    {
        /*if (_githubAuthenticationRequest != null) */ 
        await GithubService.PostAuthenticateAsync();

        NavigationManager.NavigateTo(ReturnUrl);
    });
}
