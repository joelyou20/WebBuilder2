using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebBuilder2.Server.Repositories;
using WebBuilder2.Server.Repositories.Contracts;
using WebBuilder2.Server.Utils;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Controllers;

public class LogController : ControllerBase
{
    private ILogRepository _logRepository;

    public LogController(ILogRepository logRepository)
    {
        _logRepository = logRepository;
    }

    [HttpGet("/log/{id?}")]
    public IActionResult Get([FromRoute] long? id, [FromQuery] IEnumerable<long>? exclude = null)
    {
        try
        {
            var result = _logRepository.Get(exclude);
            if (id != null) result = result?.Where(x => x.Id == id);


            if (result == null) throw new Exception(id == null ?
                "Failed to get repository data from database." :
                $"Failed to retrieve repository data with ID value of: {id}");

            List<LogModel> resultList = result.ToList();

            return Ok(JsonConvert.SerializeObject(ValidationResponse<LogModel>.Success(resultList)));
        }
        catch (Exception ex)
        {
            return BadRequest(JsonConvert.SerializeObject(ValidationResponseHelper<LogModel>.BuildFailedResponse(ex)));
        }
    }

    [HttpPut("/log")]
    public IActionResult Put([FromBody] IEnumerable<LogModel> repos)
    {
        try
        {
            var result = _logRepository.UpsertRange(repos);
            return Ok(JsonConvert.SerializeObject(ValidationResponse<LogModel>.Success(result)));
        }
        catch (Exception ex)
        {
            return BadRequest(JsonConvert.SerializeObject(ValidationResponseHelper<LogModel>.BuildFailedResponse(repos, ex)));
        }
    }

    [HttpPost("/log/delete")]
    public IActionResult SoftDelete([FromBody] IEnumerable<LogModel> repos)
    {
        try
        {
            var result = _logRepository.SoftDeleteRange(repos);
            return Ok(JsonConvert.SerializeObject(ValidationResponse<LogModel>.Success(result)));
        }
        catch (Exception ex)
        {
            return BadRequest(JsonConvert.SerializeObject(ValidationResponseHelper<LogModel>.BuildFailedResponse(repos, ex)));
        }
    }

    [HttpPost("/log/update")]
    public IActionResult Update([FromBody] IEnumerable<LogModel> repos)
    {
        try
        {
            var result = _logRepository.UpdateRange(repos);
            return Ok(JsonConvert.SerializeObject(ValidationResponse<LogModel>.Success(result)));
        }
        catch (Exception ex)
        {
            return BadRequest(JsonConvert.SerializeObject(ValidationResponseHelper<LogModel>.BuildFailedResponse(repos, ex)));
        }
    }
}
