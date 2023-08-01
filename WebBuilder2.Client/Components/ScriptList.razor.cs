using Blace.Components;
using Blace.Editing;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using WebBuilder2.Client.Components.Dialogs;
using WebBuilder2.Client.Models;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebBuilder2.Client.Components;

public partial class ScriptList
{
    [Inject] public IScriptService ScriptService { get; set; } = default!;
    [Inject] public IJSRuntime JSRuntime { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;

    [Parameter] public List<ScriptModel> Scripts { get; set; } = new();
    [Parameter] public EventCallback ScriptsChanged { get; set; } = default!;

    private List<ApiError> _errors = new();

    public async Task OnExpandBtnClick(int index)
    {
        await JSRuntime.InvokeVoidAsync("expandScript", index);
    }

    public async Task OnFileChanged(ScriptEditorFile file, ScriptModel script)
    {
        script.Data = file.Content;

        await ScriptsChanged.InvokeAsync();
        StateHasChanged();
    }

    public async Task OnSaveFileClick(ScriptModel script)
    {
        ScriptModel? result = await ScriptService.UpdateScriptAsync(script);

        if (result == null) _errors.Add(new ApiError("Failed to create script"));

        await ScriptsChanged.InvokeAsync();
        StateHasChanged();
    }

    public async Task OnDeleteFileClick(ScriptModel script)
    {
        ScriptModel? result = await ScriptService.SoftDeleteScriptAsync(script);

        if (result == null) _errors.Add(new ApiError("Failed to delete script"));

        await ScriptsChanged.InvokeAsync();
        StateHasChanged();
    }

    public async Task OnFullscreenBtnClick(ScriptModel script)
    {
        DialogOptions options = new()
        {
            CloseOnEscapeKey = true,
            CloseButton = true,
            Position = DialogPosition.Center,
            FullScreen = true
        };

        DialogParameters dialogParameters = new()
        {
            { "Script", script }
        };

        var dialog = await DialogService.ShowAsync<EditScriptDialog>(
            title: script.Name,
            options: options,
            parameters: dialogParameters
        );

        await dialog.Result;

        StateHasChanged();
    }

    private Syntax? ConvertSyntax(string syntax)
    {
        Array enums = Enum.GetValues(typeof(Syntax));
        IEnumerable<Syntax> enumsAsSyntax = enums.Cast<Syntax>();
        return enumsAsSyntax.Where(x => x.ToString().Equals(syntax)).SingleOrDefault();
    }
}
