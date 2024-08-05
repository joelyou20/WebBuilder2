using WebBuilder2.Client.Models;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Services.Contracts;

public interface IUserService
{
    event EventHandler<LoginEventArgs>? UserLoggedIn;
    Task RegisterUserAsync(RegisterUserRequest request);
    Task<LoginUserResponse> LoginUserAsync(LoginUserRequest request);
    Task LogoutUserAsync();
}
