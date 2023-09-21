using Blace.Components;
using Blace.Editing;
using Microsoft.AspNetCore.Components;
using WebBuilder2.Client.Models;

namespace WebBuilder2.Client.Components;

public partial class CodeEditor
{
    [Parameter] public string Value { get; set; } = string.Empty;
    [Parameter] public Syntax Syntax { get; set; }
    [Parameter] public EventCallback<string> ValueChanged { get; set; }
    [Parameter] public EventCallback<ScriptEditorFile> FileChanged { get; set; }
    [Parameter] public int? MinLines { get; set; }
    [Parameter] public int? MaxLines { get; set; }

    private Editor<ScriptEditorFile>? _editor;
    private ScriptEditorFile? _file;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender) await OpenFileAsync();
    }

    public async Task OpenFileAsync(string? content = null, Syntax? syntax = null)
    {
        if (_editor == null) throw new ArgumentNullException("Editor reference value is null.");

        _file = new ScriptEditorFile("", content ?? Value);
        EditorOptions editorOptions = new()
        {
            Syntax = syntax ?? Syntax,
            Theme = Theme.Eclipse,
            MinLines = MinLines ?? GetNumberOfLines(content ?? Value),
            MaxLines = MaxLines ?? GetNumberOfLines(content ?? Value)
        };
        if (_file == null) return;
        await _editor.Open(_file, editorOptions);
        await _editor.Reload();
        StateHasChanged();
    }

    private int GetNumberOfLines(string content) => content.Split('\n').Length;

    public async Task Refresh(string content, Syntax syntax) => await OpenFileAsync(content, syntax);

    public void OnFileChanged() => InvokeAsync(async () => await FileChanged.InvokeAsync(_file));
}
