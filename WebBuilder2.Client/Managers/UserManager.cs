using WebBuilder2.Client.Managers.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Managers;

public class UserManager(ILocalStorageService localStorageService, IUserService userService) : IUserManager
{
    private ILocalStorageService _localStorageService = localStorageService;
    private IUserService _userService = userService;

    public async Task HandleLogoutAsync()
    {
        await _localStorageService.RemoveItemAsync("user");
        await _userService.LogoutUserAsync();

    }

}
