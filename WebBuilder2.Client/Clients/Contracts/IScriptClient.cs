﻿using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Client.Clients.Contracts;

public interface IScriptClient
{
    Task<ValidationResponse<ScriptModel>> AddScriptAsync(ScriptModel site);
    Task<ValidationResponse<ScriptModel>> AddRangeScriptAsync(IEnumerable<ScriptModel> sites);
    Task<ValidationResponse<ScriptModel>> GetScriptByIdAsync(long id);
    Task<ValidationResponse<ScriptModel>> GetScriptsAsync(Dictionary<string, string>? filter = null);
    Task<ValidationResponse<ScriptModel>> SoftDeleteScriptAsync(ScriptModel site);
    Task<ValidationResponse<ScriptModel>> SoftDeleteRangeScriptAsync(IEnumerable<ScriptModel> sites);
    Task<ValidationResponse<ScriptModel>> UpdateScriptAsync(ScriptModel script);
}
