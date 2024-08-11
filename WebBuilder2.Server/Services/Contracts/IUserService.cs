using Microsoft.AspNetCore.Identity;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services.Contracts
{
    public interface IUserService
    {
        Task<SignInResult> LoginUserAsync(ApplicationUser user, LoginUserRequest request);
        Task LogoutUserAsync();
        Task<IdentityResult> RegisterUserAsync(RegisterUserRequest request);
        Task<ApplicationUser?> GetUserAsync(string userName);
    }
}
