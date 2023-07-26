using Microsoft.AspNetCore.Mvc;
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
    public ActionResult<ValidationResponse<ScriptModel>> Get([FromRoute] long? id, [FromQuery] IEnumerable<long>? exclude = null)
    {
        try
        {
            var result = _scriptRepository.Get(exclude);
            if (id != null) result = result?.Where(x => x.Id == id);


            if (result == null) throw new Exception(id == null ?
                "Failed to get script data from database." :
                $"Failed to retrieve script data with ID value of: {id}");

            List<ScriptModel> resultList = result.ToList();

            return Ok(ValidationResponse<ScriptModel>.Success(resultList));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidationResponseHelper<ScriptModel>.BuildFailedResponse(ex));
        }
    }

    [HttpPut("/script")]
    public ActionResult<ValidationResponse<ScriptModel>> Put([FromBody] IEnumerable<ScriptModel> repos)
    {
        try
        {
            var result = _scriptRepository.UpsertRange(repos);
            return Ok(ValidationResponse<ScriptModel>.Success(result));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidationResponseHelper<ScriptModel>.BuildFailedResponse(repos, ex));
        }
    }

    [HttpPost("/script/delete")]
    public ActionResult<ValidationResponse<ScriptModel>> SoftDelete([FromBody] IEnumerable<ScriptModel> repos)
    {
        try
        {
            var result = _scriptRepository.SoftDeleteRange(repos);
            return Ok(ValidationResponse<ScriptModel>.Success(result));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidationResponseHelper<ScriptModel>.BuildFailedResponse(repos, ex));
        }
    }

    [HttpPost("/script/update")]
    public ActionResult<ValidationResponse<ScriptModel>> Update([FromBody] IEnumerable<ScriptModel> repos)
    {
        try
        {
            var result = _scriptRepository.UpdateRange(repos);
            return Ok(ValidationResponse<ScriptModel>.Success(result));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidationResponseHelper<ScriptModel>.BuildFailedResponse(repos, ex));
        }
    }
}
