using Microsoft.AspNetCore.Mvc;
using WebBuilder2.Server.Repositories.Contracts;
using WebBuilder2.Shared.Models.Dtos;

namespace WebBuilder2.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class RepositoryController(IRepositoryRepository repositoryRepository) : CustomControllerBase
{
    private readonly IRepositoryRepository _repositoryRepository = repositoryRepository;

    [HttpGet("/repository/{id?}")]
    public IActionResult Get([FromRoute] long? id, [FromQuery] IEnumerable<long>? exclude = null)
    {
        var result = _repositoryRepository.Get(exclude);
        if (id != null) result = result?.Where(x => x.Id == id);

        if (result == null) throw new Exception(id == null ?
            "Failed to get repository data from database." :
            $"Failed to retrieve repository data with ID value of: {id}");

        List<RepositoryModel> resultList = result.ToList();

        return Ok(resultList);
    }

    [HttpPut("/repository")]
    public IActionResult Put([FromBody] IEnumerable<RepositoryModel> repos)
    {
        ValidateRequest(repos);

        var result = _repositoryRepository.UpsertRange(repos);
        return Ok(result);
    }

    [HttpPost("/repository/delete")]
    public IActionResult SoftDelete([FromBody] IEnumerable<RepositoryModel> repos)
    {
        ValidateRequest(repos);

        var result = _repositoryRepository.SoftDeleteRange(repos);
        return Ok(result);
    }

    [HttpPost("/repository/update")]
    public IActionResult Update([FromBody] IEnumerable<RepositoryModel> repos)
    {
        ValidateRequest(repos);

        var result = _repositoryRepository.UpdateRange(repos);
        return Ok(result);
    }
}
