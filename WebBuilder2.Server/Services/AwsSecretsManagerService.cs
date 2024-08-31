using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Server.Utils;

namespace WebBuilder2.Server.Services;

public class AwsSecretsManagerService(AmazonSecretsManagerClient client) : IAwsSecretsManagerService
{
    private readonly AmazonSecretsManagerClient _client = client;

    public async Task<string> GetSecretAsync(string name)
    {
        GetSecretValueRequest request = new() { SecretId = name };
        GetSecretValueResponse response = await _client.GetSecretValueAsync(request);

        AmazonServiceResponseValidator<AmazonSecretsManagerException>.Validate(response, $"Failed to get secret value with name: {name}");

        // Check if the response.SecretString is a valid JSON string
        JObject jObject;
        try
        {
            jObject = JObject.Parse(response.SecretString);
        }
        catch (JsonReaderException ex)
        {
            throw new JsonReaderException($"Secret value is not valid JSON: {response.SecretString}", ex);
        }

        string? secret = jObject[name]?.Value<string>();

        return secret ?? throw new Exception($"JSON response has no key with name: {name}");
    }
}
