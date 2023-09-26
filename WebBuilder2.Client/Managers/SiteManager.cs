using System.Collections.ObjectModel;
using WebBuilder2.Client.Managers.Contracts;
using WebBuilder2.Client.Models;
using WebBuilder2.Client.Services;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Client.Utils;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Managers;

public class SiteManager : ISiteManager
{
    private ISiteService _siteService;
    private IRepositoryManager _repositoryManager;
    private IAwsService _awsService;
    private IScriptService _scriptService;
    private ILogger<SiteManager> _logger;

    public SiteManager(ISiteService siteService, IRepositoryManager repositoryManager, IAwsService awsService, IScriptService scriptService, ILogger<SiteManager> logger)
    {
        _siteService = siteService;
        _repositoryManager = repositoryManager;
        _awsService = awsService;
        _scriptService = scriptService;
        _logger = logger;
    }

    public ObservableCollection<Job> BuildCreateSiteJobList()
    {
        return new ObservableCollection<Job>
        {
            new Job
            {
                Name = "Create Site"
            },
            new Job
            {
                Name = "Register Domain"
            },
            new Job
            {
                Name = "Create Buckets"
            },
            new Job
            {
                Name = "Configure AWS Logging"
            },
            new Job
            {
                Name = "Create Repository"
            },
            new Job
            {
                Name = "Add Repository Secrets"
            },
            new Job
            {
                Name = "Scaffold Repository"
            },
            new Job
            {
                Name = "Allow Public Access"
            },
            new Job
            {
                Name = "Add Bucket Policy"
            },
        };
    }

    public async Task<ValidationResponse> CreateSiteAsync(CreateSiteRequest createSiteRequest, ObservableCollection<Job> jobList)
    {
        int index = 0;

        void JobComplete(JobStatus jobStatus)
        {
            jobList[index] = new Job
            {
                Name = jobList[index].Name,
                Status = jobStatus
            };
            if (jobStatus != JobStatus.Success || jobList.Count - 1 == index) return;
            jobList[index + 1] = new Job
            {
                Name = jobList[index + 1].Name,
                Status = JobStatus.Pending
            };
            index++;
        }

        try
        {
            _logger.LogInformation("Running site creation");
            var domainBucket = createSiteRequest.Buckets[BucketType.Domain];
            var subDomainBucket = createSiteRequest.Buckets[BucketType.Subdomain];
            var loggingBucket = createSiteRequest.Buckets[BucketType.Logging];

            // Add Site
            _logger.LogInformation("Creating Site...");
            jobList[index].Status = JobStatus.Pending;

            SiteModel? site = await _siteService.AddSiteAsync(new()
            {
                Name = createSiteRequest.Name,
                Description = createSiteRequest.Description,
            });
            _logger.LogInformation("Site Created!");

            if (site == null)
            {
                JobComplete(JobStatus.Failure);
                return ValidationResponse.Failure($"Failed to create site. Site value = {site}");
            }
            JobComplete(JobStatus.Success);

            var repo = createSiteRequest.TemplateRepository;

            // Register domain
            //_ = await _awsService.PostRegisterDomainAsync(createSiteRequest.Domain.Name) ?? 
            //    throw new Exception($"Failed to register domain {createSiteRequest.Domain.Name}");
            JobComplete(JobStatus.Success);

            // Create AWS Buckets
            _logger.LogInformation("Creating Buckets...");
            _logger.LogInformation("Domain Bucket: {0}", domainBucket.Name);
            _logger.LogInformation("Subdomain Bucket: {0}", subDomainBucket.Name);
            _logger.LogInformation("Logging Bucket: {0}", loggingBucket.Name);

            //var createBucketsResponse = await _awsService.CreateBucketsAsync(new AwsCreateBucketRequest { Buckets = createSiteRequest.Buckets.Values });
            //if (!createBucketsResponse.IsSuccessful) return createBucketsResponse;

            JobComplete(JobStatus.Success);
            _logger.LogInformation("Buckets Created!");

            // Configure AWS logging
            //_logger.LogInformation("Configuring Logging for buckets...");
            //var configureLoggingResponse = await _awsService.PostConfigureLoggingAsync(new AwsConfigureLoggingRequest
            //{
            //    Bucket = domainBucket,
            //    LogBucket = loggingBucket,
            //    LogObjectKeyPrefix = ""
            //});
            //if (!configureLoggingResponse.IsSuccessful) return configureLoggingResponse;

            JobComplete(JobStatus.Success);
            //_logger.LogInformation("Logging Configured!");

            // Create repo
            _logger.LogInformation("Creating Repository...");
            _logger.LogInformation("Repository: {0}", $"{site.Name}-repo");
            ValidationResponse<RepositoryModel> createRepoResponse = await _repositoryManager.CreateRepositoryAsync(repo, site);
            if (!createRepoResponse.HasValues || createRepoResponse.Values?.Single().Site == null)
            {
                JobComplete(JobStatus.Failure);
                return createRepoResponse.GetResponse();
            }

            JobComplete(JobStatus.Success);
            _logger.LogInformation("Repository Created!");

            // Add secret variables to repo
            _logger.LogInformation("Adding secrets to repository...");
            var addSecretsResponse = await _repositoryManager.AddSecretsAsync(repo);
            if (!addSecretsResponse.IsSuccessful)
            {
                JobComplete(JobStatus.Failure);
                return addSecretsResponse.GetResponse();
            }

            JobComplete(JobStatus.Success);
            _logger.LogInformation("Secrets Added!");

            // Scaffold Repository
            var addTemplateProjectResponse = await AddTemplateProjectToRepoAsync(createSiteRequest.ProjectTemplateType, createRepoResponse.Values);

            if (!addTemplateProjectResponse.IsSuccessful)
            {
                JobComplete(JobStatus.Failure);
                return addTemplateProjectResponse;
            }
            JobComplete(JobStatus.Success);

            // Allow public access to site
            //var configurePublicAccessBlockResponse = await _awsService.PostConfigurePublicAccessBlockAsync(new AwsPublicAccessBlockRequest { Bucket = domainBucket, BlockPublicAcls = false });
            //if (!configurePublicAccessBlockResponse.IsSuccessful) return configurePublicAccessBlockResponse;
            JobComplete(JobStatus.Success);

            // Add Bucket Policy
            //_logger.LogInformation("Add Bucket Policy...");
            //ScriptModel? bucketPolicyScript = await _scriptService.GetScriptByNameAsync("policy");
            //if (bucketPolicyScript == null) return ValidationResponse.Failure("Failed to locate bucket policy script");

            //bucketPolicyScript.Data = bucketPolicyScript.Data.Replace("Bucket-Name", domainBucket.Name);
            //_logger.LogInformation("Policy found...");

            //var addBucketPolicyResponse = await _awsService.PostBucketPolicyAsync(new AwsAddBucketPolicyRequest { Bucket = domainBucket, Policy = bucketPolicyScript.Data });
            //if (!addBucketPolicyResponse.IsSuccessful) return addBucketPolicyResponse!;

            JobComplete(JobStatus.Success);
            //_logger.LogInformation("Policy Added!");

            return ValidationResponse.Success();
        }
        catch (Exception ex)
        {
            return ValidationResponse.Failure(ex);
        }
    }

    private async Task<ValidationResponse> AddTemplateProjectToRepoAsync(ProjectTemplateType projectTemplateType, IEnumerable<RepositoryModel>? repos = null)
    {
        if (repos == null || !repos.Any()) return ValidationResponse.Failure("Invalid repo response for AddTemplateProjectToRepoAsync");

        switch (projectTemplateType)
        {
            case ProjectTemplateType.Default:
                _logger.LogInformation("Adding default index.html to repo...");
                ScriptModel? defaultIndexScript = await _scriptService.GetScriptByNameAsync("default-index");
                if (defaultIndexScript == null) return ValidationResponse.Failure("Failed to locate default index.html script");
                return await _repositoryManager.CreateCommitAsync(defaultIndexScript.Data, "index.html", repos.First());
            case ProjectTemplateType.BlazorWebAssembly:
                _logger.LogInformation("Adding Blazor WebAssembly project template to repo...");
                return await _repositoryManager.CreateTemplateRepoAsync(ProjectTemplateType.BlazorWebAssembly, repos.First());
            default:
                break;
        }

        return ValidationResponse.Failure("Failed to add project template to repo.");
    }
}
