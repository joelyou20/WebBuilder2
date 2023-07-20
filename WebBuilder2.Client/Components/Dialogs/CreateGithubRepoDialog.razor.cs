using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebBuilder2.Client.Managers;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Components.Dialogs;

public partial class CreateGithubRepoDialog
{
    [Inject] public IGithubService GithubService { get; set; } = default!;

    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = default!;

    private GithubCreateRepoRequest _model = new();
    private List<string> _gitIgnoretemplates = new();

    protected override async Task OnInitializedAsync()
    {
        _gitIgnoretemplates = (await GithubService.GetGitIgnoreTemplatesAsync()).ToList();
    }

    public void OnValidSubmit() => InvokeAsync(async () =>
    {
        var response = await GithubService.PostCreateRepoAsync(_model);
        MudDialog.Close(DialogResult.Ok(true));
    });
}
