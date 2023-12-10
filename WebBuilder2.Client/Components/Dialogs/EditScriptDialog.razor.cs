using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebBuilder2.Client.Models;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Components.Dialogs;

public partial class EditScriptDialog
{
    [Inject] public IScriptService ScriptService { get; set; } = default!;

    [Parameter] public ScriptModel Script { get; set; } = new();
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = default!;

    public void OnFileChanged(ScriptEditorFile file)
    {
        Script.Data = file.Content;
    }

    public async Task OnSaveBtnClick()
    {
        await ScriptService.UpdateScriptAsync(Script);

        MudDialog.Close(DialogResult.Ok(true));
    }
}
