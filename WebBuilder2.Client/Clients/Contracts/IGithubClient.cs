namespace WebBuilder2.Client.Clients.Contracts;

public interface IGithubClient
{
    Task<bool> PostConnectionRequestAsync();
}
