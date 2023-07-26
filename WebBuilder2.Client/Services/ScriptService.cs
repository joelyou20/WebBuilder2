using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Services;

public class ScriptService : IScriptService
{
    private IScriptClient _scriptClient;

    public ScriptService(IScriptClient scriptClient)
    {
        _scriptClient = scriptClient;
    }

    public async Task<List<ScriptModel>> GetScriptsAsync(IEnumerable<long>? exclude = null)
    {
        ValidationResponse<ScriptModel> response = await _scriptClient.GetScriptsAsync(exclude);

        if (response == null || !response.IsSuccessful)
        {
            throw new Exception(response?.Message ?? "Failed to get script Data");
        }

        return response.GetValues();
    }

    public async Task<ScriptModel?> GetSingleScriptAsync(long id)
    {
        ValidationResponse<ScriptModel>? response = await _scriptClient.GetSingleScriptAsync(id);

        if (response == null) return null;

        if (!response.IsSuccessful)
        {
            throw new Exception(response?.Message ?? "Failed to get script Data");
        }

        return response.GetValues().SingleOrDefault();
    }

    public async Task<ScriptModel?> AddScriptAsync(ScriptModel script)
    {
        ValidationResponse<ScriptModel> response = await _scriptClient.AddScriptAsync(script);

        if (response == null) return null;

        if (!response.IsSuccessful)
        {
            throw new Exception(response?.Message ?? "Failed to add script Data");
        }

        return response.GetValues().SingleOrDefault();
    }

    public async Task<ScriptModel?> SoftDeleteScriptAsync(ScriptModel script)
    {
        ValidationResponse<ScriptModel> response = await _scriptClient.SoftDeleteScriptAsync(script);

        if (response == null) return null;

        if (!response.IsSuccessful)
        {
            throw new Exception(response?.Message ?? "Failed to delete script");
        }

        return response.GetValues().SingleOrDefault();
    }
}
