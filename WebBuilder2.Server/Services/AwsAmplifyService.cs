using Amazon.Amplify;
using Amazon.Amplify.Model;
using WebBuilder2.Server.Options;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Server.Utils;
using WebBuilder2.Shared.Models.Dtos;

namespace WebBuilder2.Server.Services;

public class AwsAmplifyService(AmazonAmplifyClient client, IAwsSecretsManagerService awsSecretsManagerService, IConfiguration configuration) : IAwsAmplifyService
{
    private readonly AmazonAmplifyClient _client = client;
    private readonly IAwsSecretsManagerService _awsSecretsManagerService = awsSecretsManagerService;

    public async Task<CreateAppResponse> CreateAppFromRepoAsync(RepositoryModel repo)
    {
        string? accessToken = configuration[AwsAmplifyOptionsStore.AccessToken];
        var token = await _awsSecretsManagerService.GetSecretAsync(AwsSecret.GithubPat);

        if (string.IsNullOrEmpty(token)) throw new AmazonAmplifyException("Failed to retrieve Github PAT");

        var request = new CreateAppRequest
        {
            Name = repo.SiteRepository?.Site?.Name,
            AccessToken = accessToken,
            Repository = repo.HtmlUrl,
        };

        CreateAppResponse response = await _client.CreateAppAsync(request);

        AmazonServiceResponseValidator<AmazonAmplifyException>.Validate(response);

        return response;
    }
}
