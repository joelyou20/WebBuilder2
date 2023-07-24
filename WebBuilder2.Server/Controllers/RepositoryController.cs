using Microsoft.AspNetCore.Mvc;
using WebBuilder2.Server.Repositories.Contracts;
using WebBuilder2.Server.Services;
using WebBuilder2.Server.Utils;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class RepositoryController : ControllerBase
{
    private IRepositoryRepository _repositoryRepository;

    public RepositoryController(IRepositoryRepository repositoryRepository)
    {
        _repositoryRepository = repositoryRepository;
    }

    [HttpGet("/repository/{id?}")]
    public ActionResult<ValidationResponse<Repository>> Get([FromRoute] long? id, [FromQuery] IEnumerable<long>? exclude = null)
    {
        try
        {
            var result = _repositoryRepository.Get(exclude);
            if (id != null) result = result?.Where(x => x.Id == id);

            if (result == null) throw new Exception(id == null ?
                "Failed to get repository data from database." :
                $"Failed to retrieve repository data with ID value of: {id}");

            return Ok(ValidationResponse<Repository>.Success(result.ToList()));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidationResponseHelper<Repository>.BuildFailedResponse(ex));
        }
    }

    [HttpPut("/repository")]
    public ActionResult<ValidationResponse<Repository>> Put([FromBody] IEnumerable<Repository> repos)
    {
        try
        {
            _repositoryRepository.UpsertRange(repos);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ValidationResponseHelper<Repository>.BuildFailedResponse(repos, ex));
        }
    }

    [HttpPost("/repository/delete")]
    public ActionResult<ValidationResponse<Repository>> SoftDelete([FromBody] IEnumerable<Repository> repos)
    {
        try
        {
            _repositoryRepository.SoftDeleteRange(repos);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ValidationResponseHelper<Repository>.BuildFailedResponse(repos, ex));
        }
    }
}
