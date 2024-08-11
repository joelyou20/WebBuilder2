using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models.Dtos;

namespace WebBuilder2.Client.Services;

public class ScriptService(IScriptClient scriptClient) : IScriptService
{
    private readonly IScriptClient _scriptClient = scriptClient;

    public async Task<List<ScriptModel>> GetScriptsAsync(Dictionary<string, string>? filter = null)
    {
        IEnumerable<ScriptModel> result = await _scriptClient.GetScriptsAsync(filter);

        return result.ToList();
    }

    public async Task<ScriptModel> GetScriptByIdAsync(long id)
    {
        ScriptModel result = await _scriptClient.GetScriptByIdAsync(id);

        return result;
    }

    public async Task<ScriptModel> GetScriptByNameAsync(string name)
    {
        IEnumerable<ScriptModel> result = await _scriptClient.GetScriptsAsync(new Dictionary<string, string> { { nameof(name), name } });

        return result.Single();
    }

    public async Task<ScriptModel> AddScriptAsync(ScriptModel script)
    {
        ScriptModel result = await _scriptClient.AddScriptAsync(script);

        return result;
    }

    public async Task<ScriptModel> UpdateScriptAsync(ScriptModel script)
    {
        ScriptModel result = await _scriptClient.UpdateScriptAsync(script);

        return result;
    }

    public async Task<ScriptModel> SoftDeleteScriptAsync(ScriptModel script)
    {
        ScriptModel result = await _scriptClient.SoftDeleteScriptAsync(script);

        return result;
    }
}
