using Amazon.Amplify;
using Amazon.Amplify.Model;
using Amazon.CostExplorer;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Server.Utils;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services;

public class AwsAmplifyService : IAwsAmplifyService
{
    private AmazonAmplifyClient _client;
    private IAwsSecretsManagerService _awsSecretsManagerService;

    public AwsAmplifyService(AmazonAmplifyClient client, IAwsSecretsManagerService awsSecretsManagerService)
    {
        _client = client;
        _awsSecretsManagerService = awsSecretsManagerService;
    }

    public async Task<ValidationResponse> CreateAppFromRepoAsync(RepositoryModel repo)
    {
        if (repo.SiteRepository == null) return ValidationResponse.Failure("Attempted to connect site to Amplify, but repo does not have a connected site.");

        var token = await _awsSecretsManagerService.GetSecretAsync(AwsSecret.GithubPat);

        if (token == null) return ValidationResponse.Failure("Failed to retrieve Github PAT");

        var request = new CreateAppRequest
        {
            Name = repo.SiteRepository.Site.Name,
            AccessToken = "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",
            Repository = repo.HtmlUrl,
        };

        var response = await _client.CreateAppAsync(request);

        return ValidationResponse.Default();
    }
}
