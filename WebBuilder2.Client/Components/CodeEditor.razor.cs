using Blace.Components;
using Blace.Editing;
using Microsoft.AspNetCore.Components;
using WebBuilder2.Client.Models;

namespace WebBuilder2.Client.Components;

public partial class CodeEditor
{
    [Parameter] public string Value { get; set; } = string.Empty;
    [Parameter] public EventCallback<string> ValueChanged { get; set; }
    [Parameter] public EventCallback<ScriptEditorFile> FileChanged { get; set; }

    private Editor<ScriptEditorFile>? _editor;
    private ScriptEditorFile? _file;

    private const int MAX_LINES = 80;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender) await OpenFileAsync();
    }

    public async Task OpenFileAsync(string? content = null, int numLines = 20)
    {
        if (_editor == null) throw new ArgumentNullException("Editor reference value is null.");

        _file = new ScriptEditorFile("", content ?? Value);
        EditorOptions editorOptions = new()
        {
            Syntax = Syntax.Yaml,
            Theme = Theme.Eclipse,
            MinLines = MAX_LINES,
            MaxLines = MAX_LINES
        };
        if (_file == null) return;
        await _editor.Open(_file, editorOptions);
        await _editor.Reload();
        StateHasChanged();
    }

    public async Task Refresh(string content, int numLines) => await OpenFileAsync(content, numLines);

    public void OnFileChanged() => InvokeAsync(async () => await FileChanged.InvokeAsync(_file));
}
