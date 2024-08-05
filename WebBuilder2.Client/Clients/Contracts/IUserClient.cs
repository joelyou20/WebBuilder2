using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients.Contracts;

public interface IUserClient
{
    Task<ValidationResponse<RegisterUserRequest>> RegisterUserAsync(RegisterUserRequest request);
    Task<ValidationResponse<LoginUserResponse>> LoginUserAsync(LoginUserRequest request);
    Task<ValidationResponse> LogoutUserAsync();
}
