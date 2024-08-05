using System.Net.Http.Json;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients;

public class UserClient(HttpClient httpClient) : ClientBase(httpClient, "user"), IUserClient
{
    public async Task<ValidationResponse<RegisterUserRequest>> RegisterUserAsync(RegisterUserRequest request) => await PostAsync<RegisterUserRequest>("register", JsonContent.Create(request));
    public async Task<ValidationResponse<LoginUserResponse>> LoginUserAsync(LoginUserRequest request) => await PostAsync<LoginUserResponse>("login", JsonContent.Create(request));
    public async Task<ValidationResponse> LogoutUserAsync() => await PostAsync("logout");
}
