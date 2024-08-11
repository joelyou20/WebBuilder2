using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebBuilder2.Server.Repositories;
using WebBuilder2.Server.Repositories.Contracts;
using WebBuilder2.Server.Utils;
using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Controllers;

public class LogController(ILogRepository logRepository) : CustomControllerBase
{
    private readonly ILogRepository _logRepository = logRepository;

    [HttpGet("/log/{id?}")]
    public IActionResult Get([FromRoute] long? id, [FromQuery] IEnumerable<long>? exclude = null)
    {
        var result = _logRepository.Get(exclude);
        if (id != null) result = result?.Where(x => x.Id == id);

        if (result == null) throw new Exception(id == null ?
            "Failed to get repository data from database." :
            $"Failed to retrieve repository data with ID value of: {id}");

        IEnumerable<LogModel> resultList = result.ToList();

        return Ok(resultList);
    }

    [HttpPut("/log")]
    public IActionResult Put([FromBody] IEnumerable<LogModel> logs)
    {
        ValidateRequest(logs);

        var result = _logRepository.UpsertRange(logs);
        return Ok(result);
    }

    [HttpPost("/log/delete")]
    public IActionResult SoftDelete([FromBody] IEnumerable<LogModel> logs)
    {
        ValidateRequest(logs);

        var result = _logRepository.SoftDeleteRange(logs);
        return Ok(result);
    }

    [HttpPost("/log/update")]
    public IActionResult Update([FromBody] IEnumerable<LogModel> logs)
    {
        ValidateRequest(logs);

        var result = _logRepository.UpdateRange(logs);
        return Ok(result);
    }
}
