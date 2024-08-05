using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using System.Text;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services
{
    public class UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService) : IUserService
    {
        private UserManager<ApplicationUser> _userManager = userManager;
        private SignInManager<ApplicationUser> _signInManager = signInManager;
        private ITokenService _tokenService = tokenService;

        public async Task<SignInResult> LoginUserAsync(ApplicationUser user, LoginUserRequest request)
        {
            var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, request.RememberMe, lockoutOnFailure: true);

            return result;
        }

        public async Task<ValidationResponse> RegisterUserAsync(RegisterUserRequest request)
        {
            var user = new ApplicationUser { UserName = request.Email, Email = request.Email };
            var result = await _userManager.CreateAsync(user, request.PasswordHash);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return ValidationResponse.Success();
            }
            else
            {
                var message = BuildIdentityErrorMessage(result.Errors);
                return ValidationResponse.Failure(message);
            }
        }

        public async Task<ApplicationUser?> GetUserAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<ValidationResponse> LogoutUserAsync()
        {
            await _signInManager.SignOutAsync();

            return ValidationResponse.Success();
        }

        private string BuildIdentityErrorMessage(IEnumerable<IdentityError> errors)
        {
            var sb = new StringBuilder();

            foreach (var error in errors)
            {
                sb.AppendLine($"{error.Code}: {error.Description}");
            }

            return sb.ToString();
        }
    }
}
