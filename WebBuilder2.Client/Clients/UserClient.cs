using System.Net.Http.Json;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Clients;

public class UserClient(HttpClient httpClient) : ClientBase(httpClient, "user"), IUserClient
{
    public async Task<RegisterUserRequest> RegisterUserAsync(RegisterUserRequest request) => await PostAsync<RegisterUserRequest>("register", JsonContent.Create(request));
    public async Task<LoginUserResponse> LoginUserAsync(LoginUserRequest request) => await PostAsync<LoginUserResponse>("login", JsonContent.Create(request));
    public async Task LogoutUserAsync() => await PostAsync("logout");
}
