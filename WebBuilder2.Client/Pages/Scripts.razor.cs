using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebBuilder2.Client.Components.Dialogs;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Pages;

public partial class Scripts
{
    [Inject] public IDialogService DialogService { get; set; } = default!;
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

    public async Task OnCreateScriptBtnClick()
    {
        DialogOptions options = new()
        {
            CloseOnEscapeKey = true,
            CloseButton = true,
            Position = DialogPosition.Center,
            FullWidth = true
        };

        var dialog = await DialogService.ShowAsync<CreateScriptDialog>(
            title: "Create New Script",
            options: options
        );

        await dialog.Result;

        await UpdateScriptsAsync();
    }
}
