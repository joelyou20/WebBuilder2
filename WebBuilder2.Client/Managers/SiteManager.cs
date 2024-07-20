using System.Collections.ObjectModel;
using WebBuilder2.Client.Managers.Contracts;
using WebBuilder2.Client.Models;
using WebBuilder2.Client.Services;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Client.Utils;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Utils;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Managers;

public class SiteManager(
    ISiteService siteService, 
    IRepositoryManager repositoryManager, 
    IAwsService awsService, 
    IScriptService scriptService, 
    ILogger<SiteManager> logger, 
    IDatabaseService databaseService) : ISiteManager
{
    private ISiteService _siteService = siteService;
    private IRepositoryManager _repositoryManager = repositoryManager;
    private IAwsService _awsService = awsService;
    private IScriptService _scriptService = scriptService;
    private ILogger<SiteManager> _logger = logger;
    private IDatabaseService _databaseService = databaseService;

    private readonly SiteCreationState _state = new();

    public ObservableCollection<Job> JobList => BuildJobList();

    private ObservableCollection<Job> BuildJobList()
    {
        var jobs = EnumHelper.All<JobType>().Select(x => new Job(x));
        return new ObservableCollection<Job>(jobs);
    }

    public async Task CreateSiteAsync(CreateSiteRequest createSiteRequest)
    {
        _logger.LogInformation("Running site creation");

        //await CreateSiteJobAsync(createSiteRequest);
        //await RegisterDomainJobAsync(createSiteRequest);
        //await CreateAwsBucketsJobAsync(createSiteRequest);
        //await ConfigureAwsLoggingJobAsync();
        //await CreateRepositoryJobAsync(createSiteRequest);
        //await AddRepositorySecretsJobAsync();
        //await ScaffoldRepositoryJobAsync(createSiteRequest);
        //await AddScriptsToRepositoryJobAsync();
        //await AllowPublicAccessJobAsync();
        //await AddBucketPolicyJobAsync(createSiteRequest);
        await CreateDatabaseAsync(createSiteRequest);
    }

    private async Task CreateDatabaseAsync(CreateSiteRequest createSiteRequest)
    {
        if (createSiteRequest.DatabaseName == null) return;

        await _databaseService.PostCreateDatabaseAsync($"{createSiteRequest.DatabaseName}_database");
    }

    private async Task AddBucketPolicyJobAsync(CreateSiteRequest createSiteRequest)
    {
        // Add Bucket Policy
        _logger.LogInformation("Add Bucket Policy...");
        ScriptModel? bucketPolicyScript = await _scriptService.GetScriptByNameAsync("policy");
        if (bucketPolicyScript == null) throw new Exception("Failed to locate bucket policy script");

        bucketPolicyScript.Data = bucketPolicyScript.Data.Replace("Bucket-Name", _state.DomainBucket.Name);
        _logger.LogInformation("Policy found...");

        await _awsService.PostBucketPolicyAsync(new AwsAddBucketPolicyRequest { Bucket = _state.DomainBucket, Policy = bucketPolicyScript.Data });

        JobComplete(JobList[GetIndexOfJobType(JobType.AddBucketPolicy)]);
        _logger.LogInformation("Policy Added!");
    }

    private async Task AllowPublicAccessJobAsync()
    {
        // Allow public access to site
        await _awsService.PostConfigurePublicAccessBlockAsync(new AwsPublicAccessBlockRequest { Bucket = _state.DomainBucket, BlockPublicAcls = false });

        JobComplete(JobList[GetIndexOfJobType(JobType.AllowPublicAccess)]);
    }

    private async Task AddScriptsToRepositoryJobAsync()
    {
        var index = GetIndexOfJobType(JobType.AddScriptsToRepository);
        var script = await _scriptService.GetScriptByNameAsync("Deploy");

        if (script == null)
        {
            JobList[index].Status = JobStatus.Failure;
            JobComplete(JobList[index]);
            return;
        }

        await _repositoryManager.CreateCommitAsync(script.Data, script.Name, GitHubConstants.WorkflowsPath, _state.Repository);

        JobComplete(JobList[index]);
    }

    private async Task ScaffoldRepositoryJobAsync(CreateSiteRequest createSiteRequest)
    {
        var index = GetIndexOfJobType(JobType.ScaffoldRepository);

        // Scaffold Repository
        var addTemplateResult = await AddTemplateProjectToRepoAsync(createSiteRequest.ProjectTemplateType, new List<RepositoryModel> { _state.Repository });

        if (!addTemplateResult)
        {
            JobList[index].Status = JobStatus.Failure;
            JobComplete(JobList[index]);
            return;
        }

        JobComplete(JobList[index]);
    }

    private async Task AddRepositorySecretsJobAsync()
    {
        var index = GetIndexOfJobType(JobType.AddRepositorySecrets);

        // Add secret variables to repo
        _logger.LogInformation("Adding secrets to repository...");
        List<GithubSecret>? addedSecrets = await _repositoryManager.AddSecretsAsync(_state.Repository);
        if (addedSecrets == null)
        {
            JobList[index].Status = JobStatus.Failure;
            JobComplete(JobList[index]);
            return;
        }

        JobComplete(JobList[index]);
        _logger.LogInformation("Secrets Added!");
    }

    private async Task CreateRepositoryJobAsync(CreateSiteRequest createSiteRequest)
    {
        var index = GetIndexOfJobType(JobType.CreateRepository);

        // Create repo
        _logger.LogInformation("Creating Repository...");
        _logger.LogInformation("Repository: {0}", $"{_state.Site.Name}-repo");
        RepositoryModel? createdRepo = await _repositoryManager.CreateRepositoryAsync(createSiteRequest.TemplateRepository, _state.Site);

        if (createdRepo == null)
        {
            JobList[index].Status = JobStatus.Failure;
            JobComplete(JobList[index]);
            return;
        }
        else
        {
            _state.Site.SiteRepositoryId = createdRepo.SiteRepositoryId;
            await _siteService.UpdateSiteAsync(_state.Site);
            _state.Repository = createdRepo;
        }

        JobComplete(JobList[index]);
        _logger.LogInformation("Repository Created!");
    }

    private async Task ConfigureAwsLoggingJobAsync()
    {
        // Configure AWS logging
        _logger.LogInformation("Configuring Logging for buckets...");
        await _awsService.PostConfigureLoggingAsync(new AwsConfigureLoggingRequest
        {
            Bucket = _state.DomainBucket,
            LogBucket = _state.LoggingBucket,
            LogObjectKeyPrefix = ""
        });

        JobComplete(JobList[GetIndexOfJobType(JobType.ConfigureAWSLogging)]);
        _logger.LogInformation("Logging Configured!");
    }

    private async Task CreateAwsBucketsJobAsync(CreateSiteRequest createSiteRequest)
    {
        _state.DomainBucket = createSiteRequest.Buckets[BucketType.Domain];
        _state.SubDomainBucket = createSiteRequest.Buckets[BucketType.Subdomain];
        _state.LoggingBucket = createSiteRequest.Buckets[BucketType.Logging];

        // Create AWS Buckets
        _logger.LogInformation("Creating Buckets...");
        _logger.LogInformation("Domain Bucket: {0}", _state.DomainBucket.Name);
        _logger.LogInformation("Subdomain Bucket: {0}", _state.SubDomainBucket.Name);
        _logger.LogInformation("Logging Bucket: {0}", _state.LoggingBucket.Name);

        await _awsService.CreateBucketsAsync(new AwsCreateBucketRequest { Buckets = createSiteRequest.Buckets.Values });

        JobComplete(JobList[GetIndexOfJobType(JobType.CreateBuckets)]);
        _logger.LogInformation("Buckets Created!");
    }

    private async Task RegisterDomainJobAsync(CreateSiteRequest createSiteRequest)
    {
        _logger.LogInformation("Registering Domain...");
        JobList[GetIndexOfJobType(JobType.CreateSite)].Status = JobStatus.Pending;

        // Register domain
        await _awsService.PostRegisterDomainAsync(createSiteRequest.Domain.Name);

        JobComplete(JobList[GetIndexOfJobType(JobType.RegisterDomain)]);
    }

    private async Task<bool> CreateSiteJobAsync(CreateSiteRequest createSiteRequest)
    {
        // Add Site
        _logger.LogInformation("Creating Site...");
        JobList[GetIndexOfJobType(JobType.CreateSite)].Status = JobStatus.Pending;

        SiteModel? site = await _siteService.AddSiteAsync(new()
        {
            Name = createSiteRequest.Name,
            Description = createSiteRequest.Description,
            Region = createSiteRequest.Region
        }) ?? throw new Exception("Failed to create site");

        _state.Site = site;

        _logger.LogInformation("Site Created!");
        JobComplete(JobList[GetIndexOfJobType(JobType.CreateSite)]);

        return true;
    }

    private void JobComplete(Job job)
    {
        var index = GetIndexOfJobType(job.Type);
        JobList[index] = new Job(job.Type, job.Status);
    }

    private int GetIndexOfJobType(JobType jobType)
    {
        var job = JobList.FirstOrDefault(x => x.Type == jobType);

        if (job == null) throw new Exception($"JobType: {jobType} not found.");

        var index = JobList.Select(x => x.Type).ToList().IndexOf(job.Type);
        return index;
    }

    private async Task<bool> AddTemplateProjectToRepoAsync(ProjectTemplateType projectTemplateType, IEnumerable<RepositoryModel>? repos = null)
    {
        if (repos == null || !repos.Any()) return false;

        switch (projectTemplateType)
        {
            case ProjectTemplateType.Default:
                _logger.LogInformation("Adding default index.html to repo...");
                ScriptModel? defaultIndexScript = await _scriptService.GetScriptByNameAsync("default-index");
                if (defaultIndexScript == null) return false;
                await _repositoryManager.CreateCommitAsync(defaultIndexScript.Data, "index.html", "", repos.First());
                break;
            case ProjectTemplateType.Blazor:
                _logger.LogInformation("Adding Blazor WebAssembly project template to repo...");
                await _repositoryManager.CreateTemplateRepoAsync(ProjectTemplateType.Blazor, repos.First());
                break;
            case ProjectTemplateType.VueJS: 
                _logger.LogInformation("Adding VueJS project template to repo...");
                await _repositoryManager.CreateTemplateRepoAsync(ProjectTemplateType.VueJS, repos.First());
                break;
            default:
                break;
        }

        return true;
    }
}
