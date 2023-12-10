using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Newtonsoft.Json.Linq;
using WebBuilder2.Server.Services.Contracts;

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

        var jObject = JObject.Parse(response.SecretString);
        string? secret = jObject[name]?.Value<string>();

        return secret ?? throw new Exception($"Cannot locate secret with name: {name}");
    }
}
