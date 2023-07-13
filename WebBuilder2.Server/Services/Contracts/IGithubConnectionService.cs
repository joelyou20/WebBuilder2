namespace WebBuilder2.Server.Services.Contracts
{
    public interface IGithubConnectionService
    {
        Task<bool> ConnectAsync();
    }
}
