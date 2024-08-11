using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients;

public class ScriptClient(HttpClient httpClient) : ClientBase<ScriptModel>(httpClient, "script"), IScriptClient
{
    public async Task<ScriptModel> AddScriptAsync(ScriptModel script) => await AddAsync(script);
    public async Task<IEnumerable<ScriptModel>> AddRangeScriptAsync(IEnumerable<ScriptModel> scripts) => await AddRangeAsync(scripts);
    public async Task<ScriptModel> GetScriptByIdAsync(long id) => await GetSingleAsync(id);
    public async Task<IEnumerable<ScriptModel>> GetScriptsAsync(Dictionary<string, string>? filter = null) => await GetAsync(filter: filter);
    public async Task<ScriptModel> SoftDeleteScriptAsync(ScriptModel script) => await SoftDeleteAsync(script);
    public async Task<IEnumerable<ScriptModel>> SoftDeleteRangeScriptAsync(IEnumerable<ScriptModel> scripts) => await SoftDeleteRangeAsync(scripts);
    public async Task<ScriptModel> UpdateScriptAsync(ScriptModel script) => await UpdateAsync(script);
}
