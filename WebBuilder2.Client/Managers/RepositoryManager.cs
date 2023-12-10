using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.IO;
using WebBuilder2.Client.Managers.Contracts;
using WebBuilder2.Client.Services;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Client.Utils;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Utils;
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

    public async Task<RepositoryModel?> CreateRepositoryAsync(RepositoryModel repo, SiteModel site)
    {
        repo.RepoName = $"{site.Name}-repo";
        repo.Description = $"{site.Name}-repo description";
        repo.Homepage = $"{site.Name}-repo Homepage";

        RepositoryModel? createdRepo = await _githubService.PostCreateRepoAsync(repo);

        if (createdRepo == null) return null;

        // Add Site data to github created repo response
        createdRepo.Site = site;
        createdRepo.SiteId = site.Id;

        List<RepositoryModel>? response = await _repositoryService.AddRepositoriesAsync(new List<RepositoryModel> { createdRepo });

        return response?.SingleOrDefault();
    }

    public async Task<List<GithubSecret>?> AddSecretsAsync(RepositoryModel repo)
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

        List<GithubSecret>? secrets = await _githubService.CreateSecretAsync(new GithubSecret[]
        {
                new GithubSecret { Name = "AWS_S3_BUCKET", Value = s3BucketName },
                new GithubSecret { Name = "AWS_ACCESS_KEY_ID", Value = awsAccessKeyId },
                new GithubSecret { Name = "AWS_SECRET_ACCESS_KEY", Value = awsSecretAccessKey },
                new GithubSecret { Name = "AWS_REGION", Value = awsRegion }
        }, repo.RepoName);

        return secrets;
    }

    public async Task CreateCommitAsync(IBrowserFile file, RepositoryModel repo)
    {
        string fileAsString = await FileHelper.ReadFileAsync(file);
        await CreateCommitAsync(fileAsString, file.Name, repo);
    }

    public async Task CreateCommitAsync(string content, string fileName, RepositoryModel repo)
    {
        GithubCreateCommitRequest request = new()
        {
            Message = $"Add file {fileName}",
            Files = new List<NewFile>
            {
                new NewFile
                {
                    Content = content,
                    Path = $"{fileName.Replace(' ', '_')}",
                    FileType = FileType.File
                }
            }
        };

        await _githubService.CreateCommitAsync(request, repo.Name);
    }

    public async Task CreateTemplateRepoAsync(ProjectTemplateType projectTemplateType, RepositoryModel repositoryModel) => 
        await _githubService.PostCopyRepoAsync(new GithubCopyRepoRequest
        {
            ClonedRepoName = $"{projectTemplateType}-Template",
            NewRepoName = repositoryModel.Name,
            Path = null
        });
}
