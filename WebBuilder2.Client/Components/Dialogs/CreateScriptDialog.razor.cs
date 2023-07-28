using MudBlazor;
using static System.Runtime.InteropServices.JavaScript.JSType;
using WebBuilder2.Client.Managers;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Models;
using Microsoft.AspNetCore.Components;
using WebBuilder2.Client.Services;
using WebBuilder2.Client.Services.Contracts;
using Blace.Components;
using WebBuilder2.Client.Models;
using Blace.Editing;

namespace WebBuilder2.Client.Components.Dialogs;

public partial class CreateScriptDialog
{
    [Inject] public IScriptService ScriptService { get; set; } = default!;

    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = default!;

    private ScriptModel _script = new();

    private List<ApiError> _errors = new();

    public void OnFileChanged(ScriptEditorFile file)
    {
        _script.Data = file.Content;
    }

    public async Task OnValidSubmit()
    {

        var script = await ScriptService.AddScriptAsync(_script);

        if (script == null)
        {
            _errors.Add(new ApiError("Failed to create script"));
            StateHasChanged();
            return;
        }

        MudDialog.Close(DialogResult.Ok(true));
    }
}
