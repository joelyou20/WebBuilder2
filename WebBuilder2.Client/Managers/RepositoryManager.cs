using MudBlazor;
using WebBuilder2.Client.Managers.Contracts;
using WebBuilder2.Client.Services;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
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

    public async Task<ValidationResponse<Repository>> CreateRepoAsync(Repository repo)
    {
        var createRepoResponse = await _githubService.PostCreateRepoAsync(repo);

        if (!createRepoResponse.Errors.Any() && createRepoResponse.Values != null && createRepoResponse.Values.Any())
        {
            await _repositoryService.AddRepositoriesAsync(createRepoResponse.Values);
        }
        
        return createRepoResponse;
    }
}
