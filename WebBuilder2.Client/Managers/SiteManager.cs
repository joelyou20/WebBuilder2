using WebBuilder2.Client.Managers.Contracts;
using WebBuilder2.Client.Services;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Managers;

public class SiteManager : ISiteManager
{
    private ISiteService _siteService;
    private IRepositoryManager _repositoryManager;
    private IAwsService _awsService;

    public SiteManager(ISiteService siteService, IRepositoryManager repositoryManager, IAwsService awsService)
    {
        _siteService = siteService;
        _repositoryManager = repositoryManager;
        _awsService = awsService;
    }

    public async Task<ValidationResponse<SiteModel>> CreateSiteAsync(CreateSiteRequest createSiteRequest)
    {
        SiteModel? site = await _siteService.AddSiteAsync(new()
        {
            Name = createSiteRequest.Name,
            Description = createSiteRequest.Description,
        });

        if (site == null) throw new Exception($"Failed to create site. Site value = {site}");

        var repo = createSiteRequest.TemplateRepository;

        // Create AWS Buckets

        var createBucketResponse = await _awsService.CreateBucketsAsync(new AwsCreateBucketRequest
        {
            Buckets = new Bucket[]
            {
                new Bucket(createSiteRequest.Domain.Name, createSiteRequest.Region),
                new Bucket($"www.{createSiteRequest.Domain.Name}", createSiteRequest.Region)
            }
        });

        if (createBucketResponse == null) throw new Exception("Failed to create Buckets");

        // Create repo
        var createRepoResponse = await _repositoryManager.CreateRepositoryAsync(repo, site);

        if(!createRepoResponse.HasValues || createRepoResponse.Values?.Single().Site == null) throw new Exception($"Failed to create repo. Repo value = {repo}");

        var createAppResponse = await _awsService.CreateAmplifyAppAsync(createRepoResponse.Values.Single()!);

        // Add secret variables to repo
        var addSecretsResponse = await _repositoryManager.AddSecretsAsync(repo);

        return ValidationResponse<SiteModel>.Success(value: site);
    }
}
