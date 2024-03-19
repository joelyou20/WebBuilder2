using Microsoft.AspNetCore.Components.Forms;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Managers.Contracts;

public interface IRepositoryManager
{
    Task<RepositoryModel?> CreateRepositoryAsync(RepositoryModel repo, SiteModel? site = null);
    Task<List<GithubSecret>?> AddSecretsAsync(RepositoryModel repo);
    Task CreateCommitAsync(IBrowserFile file, RepositoryModel repo);
    Task CreateCommitAsync(string content, string fileName, RepositoryModel repo);
    Task CreateTemplateRepoAsync(ProjectTemplateType projectTemplateType, RepositoryModel repositoryModel);
}
