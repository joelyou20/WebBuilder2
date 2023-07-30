using Microsoft.AspNetCore.Components;
using WebBuilder2.Client.Services;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Components.Widgets;

public partial class ScriptsWidget
{
    [Inject] public IScriptService ScriptService { get; set; } = default!;

    private List<ScriptModel> _scripts = new();

    protected override async Task OnInitializedAsync()
    {
        await UpdateScriptsAsync();
    }

    private async Task UpdateScriptsAsync()
    {
        _scripts = await ScriptService.GetScriptsAsync();
        StateHasChanged();
    }
}
