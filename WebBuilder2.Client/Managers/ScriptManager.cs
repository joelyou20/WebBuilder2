using WebBuilder2.Client.Managers.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Client.Managers;

public class ScriptManager(IGithubService githubService, IScriptService scriptService) : IScriptManager
{
    private readonly IGithubService _githubService = githubService;
    private readonly IScriptService _scriptService = scriptService;

    public async Task PushScriptToRepo(ScriptModel script, long externalRepoId, string path, string repoName)
    {
        var message = $"push script to {repoName}";

        await _githubService.CreateCommitAsync(new GithubCreateCommitRequest
        {
            Files = [ ScriptToFile(script, path) ],
            Message = message,
            Branch = message.ToLower().Replace(' ', '-')
        }, externalRepoId);
    }

    public NewFile ScriptToFile(ScriptModel script, string? path) => new()
    {
        Content = script.Data,
        IsImage = false,
        FileType = FileType.File,
        Path = path == null ? script.Name : $"{path}/{script.Name}"
    };
}
