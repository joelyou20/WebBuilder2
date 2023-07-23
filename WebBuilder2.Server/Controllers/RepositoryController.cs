using Microsoft.AspNetCore.Mvc;
using WebBuilder2.Server.Services;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Server.Utils;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class RepositoryController : ControllerBase
{
    private IRepositoryDbService _repositoryDbService;

    public RepositoryController(IRepositoryDbService repositoryDbService)
    {
        _repositoryDbService = repositoryDbService;
    }

    [HttpGet("/repository/{id?}")]
    public async Task<ActionResult<ValidationResponse<Repository>>> Get([FromRoute] long? id)
    {
        try
        {
            return Ok(id == null ? await _repositoryDbService.GetAllAsync() : await _repositoryDbService.GetSingleAsync(id.Value));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidationResponseHelper<Repository>.BuildFailedResponse(ex));
        }
    }

    [HttpPut("/repository")]
    public async Task<ActionResult<ValidationResponse<Repository>>> Put([FromBody] Repository site)
    {
        try
        {
            return Ok(await _repositoryDbService.UpsertAsync(site));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidationResponseHelper<Repository>.BuildFailedResponse(site, ex));
        }
    }

    [HttpPost("/repository/delete")]
    public async Task<ActionResult<ValidationResponse<Repository>>> SoftDelete([FromBody] Repository site)
    {
        try
        {
            return Ok(await _repositoryDbService.SoftDeleteAsync(site));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidationResponseHelper<Repository>.BuildFailedResponse(site, ex));
        }
    }
}
