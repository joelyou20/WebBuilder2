using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebBuilder2.Server.Repositories;
using WebBuilder2.Server.Repositories.Contracts;
using WebBuilder2.Server.Utils;
using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class ScriptController(IScriptRepository scriptRepository) : CustomControllerBase
{
    private readonly IScriptRepository _scriptRepository = scriptRepository;

    [HttpGet("/script/{id?}")]
    public IActionResult Get([FromQuery] long? id, [FromQuery] string? name, [FromQuery] IEnumerable<long>? exclude = null)
    {
        IQueryable<ScriptModel>? result = _scriptRepository.Get(exclude);
        if (id != null) result = result?.Where(x => x.Id == id);
        if (name != null) result = result?.Where(x => x.Name.Equals(name));

        if (result == null) throw new Exception(id == null ?
            "Failed to get script data from database." :
            $"Failed to retrieve script data with ID value of: {id}");

        List<ScriptModel> resultList = result.ToList();

        return Ok(resultList);
    }

    [HttpPut("/script")]
    public IActionResult Put([FromBody] IEnumerable<ScriptModel> scripts)
    {
        ValidateRequest(scripts);

        var result = _scriptRepository.UpsertRange(scripts);
        return Ok(result);
    }

    [HttpPost("/script/delete")]
    public IActionResult SoftDelete([FromBody] IEnumerable<ScriptModel> scripts)
    {
        ValidateRequest(scripts);

        var result = _scriptRepository.SoftDeleteRange(scripts);
        return Ok(result);
    }

    [HttpPost("/script/update")]
    public IActionResult Update([FromBody] IEnumerable<ScriptModel> scripts)
    {
        ValidateRequest(scripts);

        var result = _scriptRepository.UpdateRange(scripts);
        return Ok(result);
    }
}
