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
    public ActionResult<ValidationResponse<RepositoryModel>> Get([FromRoute] long? id, [FromQuery] IEnumerable<long>? exclude = null)
    {
        try
        {
            var result = _repositoryRepository.Get(exclude);
            if (id != null) result = result?.Where(x => x.Id == id);


            if (result == null) throw new Exception(id == null ?
                "Failed to get repository data from database." :
                $"Failed to retrieve repository data with ID value of: {id}");

            List<RepositoryModel> resultList = result.ToList();

            return Ok(ValidationResponse<RepositoryModel>.Success(resultList));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidationResponseHelper<RepositoryModel>.BuildFailedResponse(ex));
        }
    }

    [HttpPut("/repository")]
    public ActionResult<ValidationResponse<RepositoryModel>> Put([FromBody] IEnumerable<RepositoryModel> repos)
    {
        try
        {
            var result = _repositoryRepository.UpsertRange(repos);
            return Ok(ValidationResponse<RepositoryModel>.Success(result));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidationResponseHelper<RepositoryModel>.BuildFailedResponse(repos, ex));
        }
    }

    [HttpPost("/repository/delete")]
    public ActionResult<ValidationResponse<RepositoryModel>> SoftDelete([FromBody] IEnumerable<RepositoryModel> repos)
    {
        try
        {
            var result = _repositoryRepository.SoftDeleteRange(repos);
            return Ok(ValidationResponse<RepositoryModel>.Success(result));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidationResponseHelper<RepositoryModel>.BuildFailedResponse(repos, ex));
        }
    }

    [HttpPost("/repository/update")]
    public ActionResult<ValidationResponse<RepositoryModel>> Update([FromBody] IEnumerable<RepositoryModel> repos)
    {
        try
        {
            var result = _repositoryRepository.UpdateRange(repos);
            return Ok(ValidationResponse<RepositoryModel>.Success(result));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidationResponseHelper<RepositoryModel>.BuildFailedResponse(repos, ex));
        }
    }
}
