using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Services.Contracts;

public interface IScriptService
{
    Task<List<ScriptModel>?> GetScriptsAsync(Dictionary<string, string>? filter = null);
    Task<ScriptModel?> GetScriptByIdAsync(long id);
    Task<ScriptModel?> AddScriptAsync(ScriptModel script);
    Task<ScriptModel?> UpdateScriptAsync(ScriptModel script);
    Task<ScriptModel?> SoftDeleteScriptAsync(ScriptModel script);
    Task<ScriptModel?> GetScriptByNameAsync(string v);
}
