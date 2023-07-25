using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using WebBuilder2.Client.Managers.Contracts;
using WebBuilder2.Client.Services;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Components.Dialogs;

public partial class CreateSiteDialog
{
    [Inject] public ISiteManager SiteManager { get; set; } = default!;
    [Inject] public IRepositoryManager RepositoryManager { get; set; } = default!;
    [Inject] public IRepositoryService RepositoryService { get; set; } = default!;

    [Parameter] public List<Repository> TemplateRepositories { get; set; } = new();
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = default!;

    private CreateSiteRequest _createSiteRequest = new();
    private readonly Func<Repository, string> _templateSelectConverter = r => r.Name;
    private Repository? _repoModel;
    private List<Repository> _templateRepositories = new();

    protected async override Task OnInitializedAsync()
    {
        _templateRepositories = (await RepositoryService.GetRepositoriesAsync()).Where(x => x.IsTemplate).ToList();
    }

    public void OnTemplateSelected(Repository? templateRepository = null)
    {
        if (templateRepository == null) return;

        _repoModel = new Repository
        {
            AllowAutoMerge = templateRepository.AllowAutoMerge,
            AllowRebaseMerge = templateRepository.AllowRebaseMerge,
            AllowMergeCommit = templateRepository.AllowMergeCommit,
            DeleteBranchOnMerge = templateRepository.DeleteBranchOnMerge,
            AutoInit = templateRepository.AutoInit,
            Visibility = templateRepository.Visibility,
            IsPrivate = templateRepository.IsPrivate,
            LicenseTemplate = templateRepository.LicenseTemplate,
            TeamId = templateRepository.TeamId,
            GitIgnoreTemplate = templateRepository.GitIgnoreTemplate,
            HasDownloads = templateRepository.HasDownloads,
            HasIssues = templateRepository.HasIssues,
            HasWiki = templateRepository.HasWiki,
            HasProjects = templateRepository.HasProjects,
            UseSquashPrTitleAsDefault = templateRepository.UseSquashPrTitleAsDefault
        };

        _createSiteRequest.TemplateRepository = _repoModel;

        StateHasChanged();
    }

    public void OnCreateBtnClick() => InvokeAsync(async () =>
    {
        var repo = _createSiteRequest.TemplateRepository;
        repo.RepoName = $"{_createSiteRequest.SiteName}-repo";
        repo.Description = $"{_createSiteRequest.SiteName}-repo description";

        await RepositoryManager.CreateRepoAsync(repo);

        await SiteManager.CreateSiteAsync(_createSiteRequest);
        MudDialog.Close(DialogResult.Ok(true));
    });
}
