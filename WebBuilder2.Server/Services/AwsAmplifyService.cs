using Amazon.Amplify;
using Amazon.Amplify.Model;
using Amazon.CostExplorer;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Server.Settings;
using WebBuilder2.Server.Utils;
using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services;

public class AwsAmplifyService(AmazonAmplifyClient client, IAwsSecretsManagerService awsSecretsManagerService, IConfiguration configuration) : IAwsAmplifyService
{
    private readonly AmazonAmplifyClient _client = client;
    private readonly IAwsSecretsManagerService _awsSecretsManagerService = awsSecretsManagerService;

    public async Task<CreateAppResponse> CreateAppFromRepoAsync(RepositoryModel repo)
    {
        string? accessToken = configuration[AwsAmplifySettingsStore.AccessToken];
        var token = await _awsSecretsManagerService.GetSecretAsync(AwsSecret.GithubPat);

        if (string.IsNullOrEmpty(token)) throw new AmazonAmplifyException("Failed to retrieve Github PAT");

        var request = new CreateAppRequest
        {
            Name = repo.SiteRepository?.Site?.Name,
            AccessToken = accessToken,
            Repository = repo.HtmlUrl,
        };

        var response = await _client.CreateAppAsync(request);

        return response;
    }
}
