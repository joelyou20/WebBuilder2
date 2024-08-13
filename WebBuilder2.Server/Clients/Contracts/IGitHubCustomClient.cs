using WebBuilder2.Shared.Models;

namespace WebBuilder2.Server.Clients.Contracts;

public interface IGitHubCustomClient
{
    Task<GithubPublicKey> GetPublicKeyAsync(string userName, string repoName, string pat);
    Task<IEnumerable<GithubSecret>> GetGithubSecrets(string userName, string repoName, string pat);
    Task CreateSecretAsync(string userName, string repoName, string pat, GithubSecret secret);
}
