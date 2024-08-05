using Amazon.Runtime.Internal;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Models;
using WebBuilder2.Client.Observers.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Services
{
    public class UserService(IUserClient client, IErrorObserver errorObserver, ILogService logService) : ServiceBase(errorObserver, logService), IUserService
    {
        private readonly IUserClient _client = client;
        public event EventHandler<LoginEventArgs>? UserLoggedIn;

        public async Task RegisterUserAsync(RegisterUserRequest request) => await ExecuteAsync(() => _client.RegisterUserAsync(request));

        public async Task<LoginUserResponse> LoginUserAsync(LoginUserRequest request)
        {
            var response = (await ExecuteAsync(() => _client.LoginUserAsync(request)))!.Single();
            UserLoggedIn?.Invoke(this, new LoginEventArgs { UserName = response.UserName, Token = response.Token });
            return response;
        }

        public async Task LogoutUserAsync()
        {
            await ExecuteAsync(() => _client.LogoutUserAsync());
        }
    }
}
