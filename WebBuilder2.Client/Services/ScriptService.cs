using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Observers.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Services;

public class ScriptService : ServiceBase, IScriptService
{
    private IScriptClient _scriptClient;

    public ScriptService(IScriptClient scriptClient, IErrorObserver errorObserver, ILogService logService) : base(errorObserver, logService)
    {
        _scriptClient = scriptClient;
    }

    public async Task<List<ScriptModel>?> GetScriptsAsync(Dictionary<string, string>? filter = null)
    {
        IEnumerable<ScriptModel>? result = await ExecuteAsync(() => _scriptClient.GetScriptsAsync(filter));

        return result?.ToList();
    }

    public async Task<ScriptModel?> GetScriptByIdAsync(long id)
    {
        IEnumerable<ScriptModel>? result = await ExecuteAsync(() => _scriptClient.GetScriptByIdAsync(id));

        return result?.SingleOrDefault();
    }

    public async Task<ScriptModel?> GetScriptByNameAsync(string name)
    {
        IEnumerable<ScriptModel>? result = await ExecuteAsync(() => _scriptClient.GetScriptsAsync(new Dictionary<string, string> { { nameof(name), name } }));

        return result?.SingleOrDefault();
    }

    public async Task<ScriptModel?> AddScriptAsync(ScriptModel script)
    {
        IEnumerable<ScriptModel>? result = await ExecuteAsync(() => _scriptClient.AddScriptAsync(script));

        return result?.SingleOrDefault();
    }

    public async Task<ScriptModel?> UpdateScriptAsync(ScriptModel script)
    {
        IEnumerable<ScriptModel>? result = await ExecuteAsync(() => _scriptClient.UpdateScriptAsync(script));

        return result?.SingleOrDefault();
    }

    public async Task<ScriptModel?> SoftDeleteScriptAsync(ScriptModel script)
    {
        IEnumerable<ScriptModel>? result = await ExecuteAsync(() => _scriptClient.SoftDeleteScriptAsync(script));

        return result?.SingleOrDefault();
    }
}
