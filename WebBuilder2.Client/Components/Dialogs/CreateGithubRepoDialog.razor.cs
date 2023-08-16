using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebBuilder2.Client.Managers;
using WebBuilder2.Client.Managers.Contracts;
using WebBuilder2.Client.Services;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Components.Dialogs;

public partial class CreateGithubRepoDialog
{
    [Inject] public IGithubService GithubService { get; set; } = default!;
    [Inject] public IRepositoryManager RepositoryManager { get; set; } = default!;
    [Inject] public ISiteService SiteService { get; set; } = default!;

    [Parameter] public List<RepositoryModel> TemplateRepositories { get; set; } = new();
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = default!;

    private RepositoryModel _model = new();
    private List<string> _gitIgnoreTemplates = new();
    private List<GithubProjectLicense> _licenses = new();
    private List<SiteModel> _disconnectedSites = new();
    private readonly Func<RepositoryModel, string> _templateSelectConverter = r => r.Name;
    private readonly Func<SiteModel, string> _siteSelectConverter = r => r.Name;
    private List<ApiError> _errors = new();
    private SiteModel? _selectedSite;

    protected override async Task OnInitializedAsync()
    {
        _gitIgnoreTemplates = (await GithubService.GetGitIgnoreTemplatesAsync()).GetValues().SelectMany(x => x.Templates).ToList();
        _licenses = (await GithubService.GetGithubProjectLicensesAsync()).GetValues();
        _disconnectedSites = (await SiteService.GetSitesAsync()).Where(x => x.Repository == null).ToList();
    }

    public void OnTemplateSelected(RepositoryModel? templateRepository = null)
    {
        if (templateRepository == null) return;

        _model.AllowAutoMerge = templateRepository.AllowAutoMerge;
        _model.AllowRebaseMerge = templateRepository.AllowRebaseMerge;
        _model.AllowMergeCommit = templateRepository.AllowMergeCommit;
        _model.DeleteBranchOnMerge = templateRepository.DeleteBranchOnMerge;
        _model.AutoInit = templateRepository.AutoInit;
        _model.Visibility = templateRepository.Visibility;
        _model.IsPrivate = templateRepository.IsPrivate;
        _model.LicenseTemplate = templateRepository.LicenseTemplate;
        _model.TeamId = templateRepository.TeamId;
        _model.GitIgnoreTemplate = templateRepository.GitIgnoreTemplate;
        _model.HasDownloads = templateRepository.HasDownloads;
        _model.HasIssues = templateRepository.HasIssues;
        _model.HasWiki = templateRepository.HasWiki;
        _model.HasProjects = templateRepository.HasProjects;
        _model.UseSquashPrTitleAsDefault = templateRepository.UseSquashPrTitleAsDefault;

        StateHasChanged();
    }

    public void OnSiteSelected(SiteModel? siteModel = null)
    {
        if (siteModel == null) return;

        _selectedSite = siteModel;
    }

    public void OnValidSubmit() => InvokeAsync(async () =>
    {
        if (_selectedSite == null) throw new InvalidOperationException("No site selected.");

        ValidationResponse<RepositoryModel> response = await RepositoryManager.CreateRepositoryAsync(_model, _selectedSite);

        if (response.IsSuccessful)
        {
            MudDialog.Close(DialogResult.Ok(true));
        }
        else
        {
            _errors.AddRange(response.Errors);
        }

        StateHasChanged();
    });
}
