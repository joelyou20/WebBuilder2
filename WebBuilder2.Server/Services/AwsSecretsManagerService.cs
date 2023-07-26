﻿using Amazon;
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

    /*
     *	Use this code snippet in your app.
     *	If you need more information about configurations or implementing the sample code, visit the AWS docs:
     *	https://aws.amazon.com/developer/language/net/getting-started
     */
    public async Task<string> GetSecretAsync(string name)
    {
        GetSecretValueRequest request = new GetSecretValueRequest
        {
            SecretId = name,
            VersionStage = "AWSCURRENT", // VersionStage defaults to AWSCURRENT if unspecified.
        };

        GetSecretValueResponse response = await _client.GetSecretValueAsync(request);

        string secret = JsonSerializer.Deserialize<GithubPAT>(response.SecretString)?.Value ?? "";

        return secret;
    }
}