using Microsoft.AspNetCore.Identity;
using Sodium;
using System.Text;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Server.Services
{
    public class UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

        public async Task<SignInResult> LoginUserAsync(LoginUserRequest request, ApplicationUser user)
        {
            SignInResult result = null!;
            try
            {
                if (string.IsNullOrEmpty(request.Username))
                {
                    throw new ArgumentNullException(nameof(request.Username));
                }

                if (string.IsNullOrEmpty(request.Password))
                {
                    throw new ArgumentNullException(nameof(request.Password));
                }
                var test = await _userManager.CheckPasswordAsync(user, request.Password);

                result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, request.RememberMe, lockoutOnFailure: true);

                return result;
            }
            catch (Exception ex)
            {
                result = SignInResult.Failed;
                throw;
            }
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterUserRequest request)
        {
            ApplicationUser user = new() { UserName = request.Email, Email = request.Email };
            IdentityResult result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return result;
            }
            else
            {
                var message = BuildIdentityErrorMessage(result.Errors);
                throw new Exception(message);
            }
        }

        public async Task<ApplicationUser?> GetUserAsync(string userName) => await _userManager.FindByNameAsync(userName);

        public async Task LogoutUserAsync() => await _signInManager.SignOutAsync();

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
