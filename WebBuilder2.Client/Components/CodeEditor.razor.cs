using Blace.Components;
using Blace.Editing;
using Microsoft.AspNetCore.Components;
using WebBuilder2.Client.Models;

namespace WebBuilder2.Client.Components;

public partial class CodeEditor
{
    [Parameter] public string Value { get; set; } = string.Empty;
    [Parameter] public EventCallback<ScriptEditorFile> FileChanged { get; set; }

    private Editor<ScriptEditorFile>? _editor;
    private ScriptEditorFile? _file;

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            if (_editor == null) throw new ArgumentNullException("Editor reference value is null.");

            _file = new ScriptEditorFile("", Value);
            EditorOptions editorOptions = new()
            {
                Syntax = Syntax.Yaml,
                Theme = Theme.Eclipse,
            };
            if (_file == null) return;
            await _editor.Open(_file, editorOptions);
            StateHasChanged();
        }
    }

    public void OnFileChanged() => InvokeAsync(async () => await FileChanged.InvokeAsync(_file));
}
