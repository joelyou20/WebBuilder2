using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService, ITokenService tokenService) : CustomControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly ITokenService _tokenService = tokenService;

        [HttpPost("/user/register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            await _userService.RegisterUserAsync(request);

            return Ok();
        }

        [HttpPost("/user/login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
        {
            try
            {
                var user = await _userService.GetUserAsync(request.Username);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                if (!user.EmailConfirmed)
                {
                    return Forbid("Email not confirmed.");
                }

                var result = await _userService.LoginUserAsync(user, request);

                if (result.Succeeded)
                {
                    var token = _tokenService.GenerateToken(user);
                    LoginUserResponse response = new()
                    {
                        UserName = user.UserName ?? "",
                        Token = token
                    };
                    return Ok(response);
                }
                if (result.IsLockedOut)
                {
                    return Forbid("Account is locked out.");
                }
                if (result.IsNotAllowed)
                {
                    return Forbid("Login not allowed.");
                }

                return Unauthorized("Invalid credentials.");
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("/user/logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutUserAsync();

            return Ok();
        }
    }
}
