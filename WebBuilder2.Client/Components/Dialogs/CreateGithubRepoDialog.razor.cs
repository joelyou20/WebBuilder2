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
    private GithubCreateRepoResponse _response = new();

    protected override async Task OnInitializedAsync()
    {
        _gitIgnoretemplates = (await GithubService.GetGitIgnoreTemplatesAsync()).ToList();
        _licenses = (await GithubService.GetGithubProjectLicensesAsync()).ToList();
    }

    public void OnValidSubmit() => InvokeAsync(async () =>
    {
        _response = await GithubService.PostCreateRepoAsync(_model);
        if(!_response.Errors.Any()) MudDialog.Close(DialogResult.Ok(true));
        StateHasChanged();
    });
}
