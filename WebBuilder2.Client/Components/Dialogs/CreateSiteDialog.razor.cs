using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using WebBuilder2.Client.Managers.Contracts;
using WebBuilder2.Client.Models;
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
    private ObservableCollection<Job> _jobList = new(); 

    private readonly Func<RepositoryModel, string> _templateSelectConverter = t => t.Name;
    private readonly Func<ProjectTemplateType, string> _projectTemplateSelectConverter = p => p.ToString();
    private readonly Func<Domain, string> _domainSelectConverter = d => d.Name;
    private RepositoryModel? _repoModel;
    private List<RepositoryModel> _templateRepositories = new();
    private bool _useNewDomain = false;

    private List<ApiError> _errors = new();

    protected async override Task OnInitializedAsync()
    {
        _jobList = SiteManager.BuildCreateSiteJobList();
        _jobList.CollectionChanged += _jobList_CollectionChanged;
        _templateRepositories = (await RepositoryService.GetRepositoriesAsync()).Where(x => x.IsTemplate).ToList();
    }

    private void _jobList_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        StateHasChanged();
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

    public void OnValidSubmit() => InvokeAsync(async () =>
    {
        await SiteManager.CreateSiteAsync(_createSiteRequest, _jobList);

        //MudDialog.Close(DialogResult.Ok(true));
    });

    public Color GetStatusColor(JobStatus status) => status switch
    {
        JobStatus.Success => Color.Success,
        JobStatus.Failure => Color.Warning,
        JobStatus.Pending => Color.Info,
        _ => Color.Default,
    };
}
