﻿using Amazon.Runtime.Internal.Transform;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebBuilder2.Client.Components;
using WebBuilder2.Client.Components.Dialogs;
using WebBuilder2.Client.Managers;
using WebBuilder2.Client.Services;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Pages;

public partial class GithubConnection
{
    [Inject] public IGithubService GithubService { get; set; } = default!;
    [Inject] public IRepositoryService RepositoryService { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;

    private List<RepositoryModel> _repositories { get; set; } = new List<RepositoryModel>();
    private List<RepositoryModel> _templates => _repositories.Where(x => x.IsTemplate).ToList();

    private bool _isTableLoading = true;

    protected override async Task OnInitializedAsync()
    {
        await UpdateReposAsync();
    }

    public async Task OnCreateRepoBtnClick()
    {
        DialogOptions options = new()
        {
            CloseOnEscapeKey = true,
            CloseButton = true,
            Position = DialogPosition.Center,
            FullWidth = true
        };
        DialogParameters dialogParameters = new()
        {
            { "TemplateRepositories", _repositories.Where(x => x.IsTemplate).ToList() }
        };

        var dialog = await DialogService.ShowAsync<CreateGithubRepoDialog>(
            title: "Create New Repo",
            options: options,
            parameters: dialogParameters
        );

        await dialog.Result;

        await UpdateReposAsync();
    }

    public async Task OnImportRepoBtnClick()
    {
        DialogOptions options = new()
        {
            CloseOnEscapeKey = true,
            CloseButton = true,
            Position = DialogPosition.Center,
            FullWidth = true
        };
        DialogParameters dialogParameters = new()
        {
            { "ExistingIds", _repositories.Select(x => x.Id).ToList() }
        };

        var dialog = await DialogService.ShowAsync<ImportGithubRepoDialog>(
            title: "Import Repository from Github Account",
            options: options,
            parameters: dialogParameters
        );

        await dialog.Result;

        await UpdateReposAsync();
    }

    public async Task UpdateReposAsync()
    {
        var response = await RepositoryService.GetRepositoriesAsync();
        _repositories = response;
        _isTableLoading = false;
        StateHasChanged();
    }

    public void OnRepoTableValueChanged() => InvokeAsync(UpdateReposAsync);
}
