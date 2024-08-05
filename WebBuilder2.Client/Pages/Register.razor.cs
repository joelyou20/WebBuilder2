using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Pages;

public partial class Register
{
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public IUserService UserService { get; set; } = default!;
    [Inject] public IPasswordHasher<ApplicationUser> PasswordHasher { get; set; } = default!;

    private string _password = string.Empty;
    private RegisterUserRequest _request = new();

    private async Task HandleRegistration()
    {
        var user = new ApplicationUser { UserName = _request.Email, Email = _request.Email };
        _request.PasswordHash = PasswordHasher.HashPassword(user, _password);
        await UserService.RegisterUserAsync(_request);
        NavigationManager.NavigateTo("/");
    }
}
