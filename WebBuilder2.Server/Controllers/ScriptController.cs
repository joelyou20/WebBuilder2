using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebBuilder2.Server.Repositories;
using WebBuilder2.Server.Repositories.Contracts;
using WebBuilder2.Server.Utils;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class ScriptController : ControllerBase
{
    private IScriptRepository _scriptRepository;

    public ScriptController(IScriptRepository scriptRepository)
    {
        _scriptRepository = scriptRepository;
    }

    [HttpGet("/script/{id?}")]
    public IActionResult Get([FromQuery] long? id, [FromQuery] string? name, [FromQuery] IEnumerable<long>? exclude = null)
    {
        try
        {
            IQueryable<ScriptModel>? result = _scriptRepository.Get(exclude);
            if (id != null) result = result?.Where(x => x.Id == id);
            if (name != null) result = result?.Where(x => x.Name.Equals(name));

            if (result == null) throw new Exception(id == null ?
                "Failed to get script data from database." :
                $"Failed to retrieve script data with ID value of: {id}");

            List<ScriptModel> resultList = result.ToList();

            return Ok(JsonConvert.SerializeObject(ValidationResponse<ScriptModel>.Success(resultList)));
        }
        catch (Exception ex)
        {
            return BadRequest(JsonConvert.SerializeObject(ValidationResponseHelper<ScriptModel>.BuildFailedResponse(ex)));
        }
    }

    [HttpPut("/script")]
    public IActionResult Put([FromBody] IEnumerable<ScriptModel> scripts)
    {
        try
        {
            var result = _scriptRepository.UpsertRange(scripts);
            return Ok(JsonConvert.SerializeObject(ValidationResponse<ScriptModel>.Success(result)));
        }
        catch (Exception ex)
        {
            return BadRequest(JsonConvert.SerializeObject(ValidationResponseHelper<ScriptModel>.BuildFailedResponse(scripts, ex)));
        }
    }

    [HttpPost("/script/delete")]
    public IActionResult SoftDelete([FromBody] IEnumerable<ScriptModel> scripts)
    {
        try
        {
            var result = _scriptRepository.SoftDeleteRange(scripts);
            return Ok(JsonConvert.SerializeObject(ValidationResponse<ScriptModel>.Success(result)));
        }
        catch (Exception ex)
        {
            return BadRequest(JsonConvert.SerializeObject(ValidationResponseHelper<ScriptModel>.BuildFailedResponse(scripts, ex)));
        }
    }

    [HttpPost("/script/update")]
    public IActionResult Update([FromBody] IEnumerable<ScriptModel> scripts)
    {
        try
        {
            var result = _scriptRepository.UpdateRange(scripts);
            return Ok(JsonConvert.SerializeObject(ValidationResponse<ScriptModel>.Success(result)));
        }
        catch (Exception ex)
        {
            return BadRequest(JsonConvert.SerializeObject(ValidationResponseHelper<ScriptModel>.BuildFailedResponse(scripts, ex)));
        }
    }
}
