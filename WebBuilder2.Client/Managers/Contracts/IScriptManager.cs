using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Managers.Contracts;

public interface IScriptManager
{
    Task PushScriptToRepo(ScriptModel script, long externalRepoId, string path, string repoName);
    NewFile ScriptToFile(ScriptModel script, string path);
}
