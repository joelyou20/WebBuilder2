using Microsoft.AspNetCore.Components;
using WebBuilder2.Client.Managers.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Components.Dialogs;

public partial class PushScriptToRepoDialog
{
    [Parameter] public List<ScriptModel> Scripts { get; set; } = default!;

    [Inject] public IRepositoryService RepositoryService { get; set; } = default!;
    [Inject] public IScriptManager ScriptManager { get; set; } = default!;
    
    private RepositoryModel _selectedRepository { get; set; } = default!;
    private ScriptModel _selectedScript { get; set; } = default!;
    private List<RepositoryModel>? _repositories { get; set; } = default!;
    private string _path { get; set; } = default!;

    private readonly Func<RepositoryModel, string> _repoSelectConverter = r => r.Name;
    private readonly Func<ScriptModel, string> _scriptSelectConverter = r => r.Name;

    protected async override Task OnInitializedAsync()
    {
        _repositories = await RepositoryService.GetRepositoriesAsync();
    }

    public async Task OnSaveBtnClick()
    {
        if (string.IsNullOrEmpty(_selectedScript.Name)) throw new NullReferenceException("Select script name is null or empty");

        await ScriptManager.PushScriptToRepo(_selectedScript, _selectedRepository.ExternalId, _path, _selectedRepository.Name);
    }
}
