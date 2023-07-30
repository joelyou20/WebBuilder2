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

    public async Task<ValidationResponse<RepositoryModel>> CreateRepoAsync(RepositoryModel repo, SiteModel site)
    {
        var createRepoResponse = await _githubService.PostCreateRepoAsync(repo);


        if (!createRepoResponse.Errors.Any() && createRepoResponse.Values != null && createRepoResponse.Values.Any())
        {
            // Add Site data to github created repo response
            var createdRepo = createRepoResponse.Values.Single();
            createdRepo.Site = site;
            createdRepo.SiteId = site.Id;

            var addRepoResponse = await _repositoryService.AddRepositoriesAsync(new List<RepositoryModel> { createdRepo });

            return addRepoResponse != null ? 
                ValidationResponse<RepositoryModel>.Success(addRepoResponse) :
                ValidationResponse<RepositoryModel>.Failure();
        }
        
        return createRepoResponse;
    }
}
