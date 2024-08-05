
using Microsoft.AspNetCore.Components;
using WebBuilder2.Client.Managers.Contracts;
using WebBuilder2.Client.Models;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Shared;

public partial class Topbar
{
    [Inject] public ILocalStorageService LocalStorageService { get; set; } = default!;
    [Inject] public IUserManager UserManager { get; set; } = default!;
    [Inject] public IUserService UserService { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    private LoginUserResponse? _user = default!;

    protected async override Task OnInitializedAsync()
    {
        _user = await LocalStorageService.GetItemAsync<LoginUserResponse>("user");
        UserService.UserLoggedIn += UpdateUser;
        StateHasChanged();
    }

    private void UpdateUser(object? sender, LoginEventArgs e)
    {
        _user = new LoginUserResponse { UserName = e.UserName, Token = e.Token };
        StateHasChanged();
    }

    private async Task OnLogoutBtnClick()
    {
        await UserManager.HandleLogoutAsync();
        _user = null;
        NavigationManager.NavigateTo("/identity/account/login");
    }
}
