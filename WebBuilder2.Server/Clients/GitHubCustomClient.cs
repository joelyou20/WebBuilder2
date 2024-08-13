using Newtonsoft.Json;
using Sodium;
using System.Text;
using WebBuilder2.Server.Clients.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Server.Clients;

public class GitHubCustomClient(HttpClient client) : IGitHubCustomClient
{
    private readonly HttpClient _client = client;

    public async Task<GithubPublicKey> GetPublicKeyAsync(string userName, string repoName, string pat) => 
        await GetAsync<GithubPublicKey>("actions/secrets/public-key", userName, repoName, pat);

    public async Task<IEnumerable<GithubSecret>> GetGithubSecrets(string userName, string repoName, string pat)
    {
        GithubSecretResponse response = await GetAsync<GithubSecretResponse>("actions/secrets", userName, repoName, pat);
        return response.GithubSecrets;
    }

    public async Task CreateSecretAsync(string userName, string repoName, string pat, GithubSecret secret)
    {
        ArgumentNullException.ThrowIfNull(secret);
        ArgumentNullException.ThrowIfNull(secret.Value);

        var publicKey = await GetPublicKeyAsync(userName, repoName, pat);

        string encodedSecret = EncodeSecret(secret.Value, publicKey.Key);

        GithubCreateSecretRequest githubCreateSecretRequest = new(encodedSecret, publicKey.Id);
        JsonContent content = JsonContent.Create(githubCreateSecretRequest);

        await PutAsync($"actions/secrets/{secret.Name}", userName, repoName, pat, content);
    }

    #region Private Methods

    private string EncodeSecret(string secret, string publicKey)
    {
        var encodedSecret = Encoding.UTF8.GetBytes(secret);
        var encodedPublicKey = Convert.FromBase64String(publicKey);

        var sealedPublicKeyBox = SealedPublicKeyBox.Create(encodedSecret, encodedPublicKey);

        return Convert.ToBase64String(sealedPublicKeyBox);
    }

    private async Task PutAsync(string endpoint, string userName, string repoName, string pat, JsonContent? content = null)
    {
        HttpRequestMessage request = BuildRequestMessage(HttpMethod.Put, endpoint, userName, repoName, pat, content);

        HttpResponseMessage response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();
    }

    private async Task<T> GetAsync<T>(string endpoint, string userName, string repoName, string pat, JsonContent? content = null) where T : class
    {
        HttpRequestMessage request = BuildRequestMessage(HttpMethod.Get, endpoint, userName, repoName, pat, content);

        HttpResponseMessage response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        string message = await response.Content.ReadAsStringAsync();
        T? result = JsonConvert.DeserializeObject<T>(message);

        if (result == null) throw new JsonException($"Failed to convert response to {nameof(GithubPublicKey)}. Message content: {message}");

        return result;
    }

    private HttpRequestMessage BuildRequestMessage(HttpMethod method, string endpoint, string userName, string repoName, string githubPAT, JsonContent? content = null)
    {
        HttpRequestMessage request = new(method, $"https://api.github.com/repos/{userName}/{repoName}/{endpoint}");
        request.Headers.Add("Accept", "application/vnd.github+json");
        request.Headers.Add("Authorization", $"Bearer {githubPAT}");
        request.Headers.Add("X-GitHub-Api-Version", "2022-11-28");
        request.Headers.Add("User-Agent", "request");
        request.Headers.Add("Cookie", "_octo=GH1.1.1578474083.1689685007; logged_in=no");
        if (content != null) request.Content = content;

        return request;
    }
    #endregion
}
