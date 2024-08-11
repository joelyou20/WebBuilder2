using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients.Contracts;

public interface IScriptClient
{
    Task<ScriptModel> AddScriptAsync(ScriptModel site);
    Task<IEnumerable<ScriptModel>> AddRangeScriptAsync(IEnumerable<ScriptModel> sites);
    Task<ScriptModel> GetScriptByIdAsync(long id);
    Task<IEnumerable<ScriptModel>> GetScriptsAsync(Dictionary<string, string>? filter = null);
    Task<ScriptModel> SoftDeleteScriptAsync(ScriptModel site);
    Task<IEnumerable<ScriptModel>> SoftDeleteRangeScriptAsync(IEnumerable<ScriptModel> sites);
    Task<ScriptModel> UpdateScriptAsync(ScriptModel script);
}
