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
    [Inject] public IRepositoryService RepositoryService { get; set; } = default!;

    [Parameter] public List<Domain> RegisteredDomains { get; set; } = new();
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = default!;

    private CreateSiteRequest _createSiteRequest = new();
    private readonly Func<RepositoryModel, string> _templateSelectConverter = t => t.Name;
    private readonly Func<Domain, string> _domainSelectConverter = d => d.Name;
    private RepositoryModel? _repoModel;
    private List<RepositoryModel> _templateRepositories = new();
    private bool _useNewDomain = false;

    private List<ApiError> _errors = new();

    protected async override Task OnInitializedAsync()
    {
        _templateRepositories = (await RepositoryService.GetRepositoriesAsync()).Where(x => x.IsTemplate).ToList();
    }

    public void OnTemplateSelected(RepositoryModel? templateRepository = null)
    {
        if (templateRepository == null) return;

        _repoModel = new RepositoryModel
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
        var createSiteResponse = await SiteManager.CreateSiteAsync(_createSiteRequest);

        if (createSiteResponse == null)
        {
            _errors.Add(new ApiError("Failed to create site"));
            StateHasChanged();
            return;
        }

        if (createSiteResponse.IsSuccessful)
        {
            if (createSiteResponse.Values == null || !createSiteResponse.Values.Any())
            {
                throw new Exception("ERROR: Repo response reports successful, but no values were returned.");
            }
            MudDialog.Close(DialogResult.Ok(true));
        }
        else
        {
            _errors.AddRange(createSiteResponse.Errors);
            StateHasChanged();
        }
    });
}
