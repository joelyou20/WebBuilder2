using Microsoft.AspNetCore.Components.Forms;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Managers.Contracts;

public interface IRepositoryManager
{
    Task<ValidationResponse<RepositoryModel>> CreateRepositoryAsync(RepositoryModel repo, SiteModel site);
    Task<ValidationResponse<GithubSecret>> AddSecretsAsync(RepositoryModel repo);
    Task<ValidationResponse> CreateCommitAsync(IBrowserFile file, RepositoryModel repo);
    Task<ValidationResponse> CreateCommitAsync(string content, string fileName, RepositoryModel repo);
    Task<ValidationResponse?> CreateTemplateRepoAsync(ProjectTemplateType projectTemplateType, RepositoryModel repositoryModel);
}
