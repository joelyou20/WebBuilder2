using Amazon;
using Amazon.Route53;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using System.Text.Json;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Server.Utils;

namespace WebBuilder2.Server.Services;

public class AwsSecretsManagerService : IAwsSecretsManagerService
{
    private AmazonSecretsManagerClient _client;

    public AwsSecretsManagerService(AmazonSecretsManagerClient client)
    {
        _client = client;
    }

    public async Task<string> GetSecretAsync(string name)
    {
        GetSecretValueRequest request = new() { SecretId = name };
        GetSecretValueResponse response = await _client.GetSecretValueAsync(request);

        string secret = JsonSerializer.Deserialize<GithubPAT>(response.SecretString)?.Value ?? "";

        return secret;
    }
}
