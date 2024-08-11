using Amazon.Runtime.Internal;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Models;
using WebBuilder2.Client.Observers.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Services
{
    public class UserService(IUserClient client) : IUserService
    {
        private readonly IUserClient _client = client;
        public event EventHandler<LoginEventArgs>? UserLoggedIn;

        public async Task RegisterUserAsync(RegisterUserRequest request) => await _client.RegisterUserAsync(request);

        public async Task<LoginUserResponse> LoginUserAsync(LoginUserRequest request)
        {
            var response = await _client.LoginUserAsync(request);
            UserLoggedIn?.Invoke(this, new LoginEventArgs { UserName = response.UserName, Token = response.Token });
            return response;
        }

        public async Task LogoutUserAsync()
        {
            await _client.LogoutUserAsync();
        }
    }
}
