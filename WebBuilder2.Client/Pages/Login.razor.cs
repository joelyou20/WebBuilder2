using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using WebBuilder2.Client.Observers.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Pages;

public partial class Login
{
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public IUserService UserService { get; set; } = default!;
    [Inject] public IErrorObserver ErrorObserver { get; set; } = default!;
    [Inject] public ILocalStorageService LocalStorageService { get; set; } = default!;

    private LoginUserRequest _request = new();

    private async Task HandleLogin()
    {
        var loginResponse = await UserService.LoginUserAsync(_request);
        await LocalStorageService.SetItemAsync("user", JsonConvert.SerializeObject(loginResponse));
        NavigationManager.NavigateTo("/");
    }

    private async Task OnResetPasswordBtnClicked()
    {

    }
}
