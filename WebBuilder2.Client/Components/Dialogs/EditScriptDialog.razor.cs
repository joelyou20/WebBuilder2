using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebBuilder2.Client.Models;
using WebBuilder2.Client.Services;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebBuilder2.Client.Components.Dialogs;

public partial class EditScriptDialog
{
    [Inject] public IScriptService ScriptService { get; set; } = default!;

    [Parameter] public ScriptModel Script { get; set; } = new();
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = default!;


    private List<ApiError> _errors = new();

    public void OnFileChanged(ScriptEditorFile file)
    {
        Script.Data = file.Content;
    }

    public async Task OnSaveBtnClick()
    {
        var script = await ScriptService.UpdateScriptAsync(Script);

        if (script == null)
        {
            _errors.Add(new ApiError("Failed to update script"));
            StateHasChanged();
            return;
        }

        MudDialog.Close(DialogResult.Ok(true));
    }
}
