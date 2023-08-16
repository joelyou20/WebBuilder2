using MudBlazor;
using WebBuilder2.Client.Managers.Contracts;
using WebBuilder2.Client.Services;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Managers;

public class RepositoryManager : IRepositoryManager
{
    private IGithubService _githubService;
    private IRepositoryService _repositoryService;

    public RepositoryManager(IGithubService githubService, IRepositoryService repositoryService)
    {
        _githubService = githubService;
        _repositoryService = repositoryService;
    }

    public async Task<ValidationResponse<RepositoryModel>> CreateRepositoryAsync(RepositoryModel repo, SiteModel site)
    {
        repo.RepoName = $"{site.Name}-repo";
        repo.Description = $"{site.Name}-repo description";
        repo.Homepage = $"{site.Name}-repo Homepage";

        var createRepoResponse = await _githubService.PostCreateRepoAsync(repo);

        if (!createRepoResponse.Errors.Any() && createRepoResponse.Values != null && createRepoResponse.Values.Any())
        {
            // Add Site data to github created repo response
            var createdRepo = createRepoResponse.Values.Single();
            createdRepo.Site = site;
            createdRepo.SiteId = site.Id;

            var newRepo = await _repositoryService.AddRepositoriesAsync(new List<RepositoryModel> { createdRepo });

            if (newRepo == null) return ValidationResponse<RepositoryModel>.Failure(message: "Failed to add repo");

        }

        return createRepoResponse;
    }

    public async Task<ValidationResponse<GithubSecret>> AddSecretsAsync(RepositoryModel repo)
    {
        // Add secrets to repo

        // Get S3 Bucket
        string s3BucketName = "";

        // Get AWS Access Key Id
        string awsAccessKeyId = "";

        // Get AWS Secret Access Key
        string awsSecretAccessKey = "";

        // Get AWS Region
        string awsRegion = "";

        var createSecretsResponse = await _githubService.CreateSecretAsync(new GithubSecret[]
        {
                new GithubSecret { Name = "AWS_S3_BUCKET", Value = s3BucketName },
                new GithubSecret { Name = "AWS_ACCESS_KEY_ID", Value = awsAccessKeyId },
                new GithubSecret { Name = "AWS_SECRET_ACCESS_KEY", Value = awsSecretAccessKey },
                new GithubSecret { Name = "AWS_REGION", Value = awsRegion }
        }, repo.RepoName);

        if (createSecretsResponse == null) return ValidationResponse<GithubSecret>.Failure(message: "Failed to add repo");

        return createSecretsResponse;
    }
}
