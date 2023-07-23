using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebBuilder2.Client.Managers;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Components.Dialogs;

public partial class CreateGithubRepoDialog
{
    [Inject] public IGithubService GithubService { get; set; } = default!;

    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = default!;

    private GithubCreateRepoRequest _model = new();
    private List<string> _gitIgnoretemplates = new();
    private List<GithubProjectLicense> _licenses = new();

    private List<ApiError> _errors = new();

    protected override async Task OnInitializedAsync()
    {
        _gitIgnoretemplates = (await GithubService.GetGitIgnoreTemplatesAsync()).GetValues().SelectMany(x => x.Templates).ToList();
        _licenses = (await GithubService.GetGithubProjectLicensesAsync()).GetValues();
    }

    public void OnValidSubmit() => InvokeAsync(async () =>
    {
        var createRepoResponse = await GithubService.PostCreateRepoAsync(_model);

        _errors.AddRange(createRepoResponse.Errors);

        if (!_errors.Any()) MudDialog.Close(DialogResult.Ok(true));
        StateHasChanged();
    });
}
