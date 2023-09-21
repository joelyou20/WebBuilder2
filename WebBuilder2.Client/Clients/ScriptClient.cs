using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients;

public class ScriptClient : ClientBase<ScriptModel>, IScriptClient
{
    public ScriptClient(HttpClient httpClient) : base(httpClient, "script") { }

    public async Task<ValidationResponse<ScriptModel>> AddScriptAsync(ScriptModel script) => await AddAsync(script);
    public async Task<ValidationResponse<ScriptModel>> AddRangeScriptAsync(IEnumerable<ScriptModel> scripts) => await AddRangeAsync(scripts);
    public async Task<ValidationResponse<ScriptModel>> GetScriptByIdAsync(long id) => await GetSingleAsync(id);
    public async Task<ValidationResponse<ScriptModel>> GetScriptsAsync(Dictionary<string, string>? filter = null) => await GetAsync(filter: filter);
    public async Task<ValidationResponse<ScriptModel>> SoftDeleteScriptAsync(ScriptModel script) => await SoftDeleteAsync(script);
    public async Task<ValidationResponse<ScriptModel>> SoftDeleteRangeScriptAsync(IEnumerable<ScriptModel> scripts) => await SoftDeleteRangeAsync(scripts);
    public async Task<ValidationResponse<ScriptModel>> UpdateScriptAsync(ScriptModel script) => await UpdateAsync(script);
}
