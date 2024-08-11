using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebBuilder2.Server.Repositories.Contracts;
using WebBuilder2.Server.Utils;
using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class SiteRepositoryController(ISiteRepositoryRepository siteRepositoryRepository) : CustomControllerBase
{
    private readonly ISiteRepositoryRepository _siteRepositoryRepository = siteRepositoryRepository;

    [HttpGet("/siteRepository/{id?}")]
    public IActionResult Get([FromRoute] long? id, [FromQuery] IEnumerable<long>? exclude = null)
    {
        var result = _siteRepositoryRepository.Get(exclude);
        if (id != null) result = result?.Where(x => x.Id == id);

        if (result == null) throw new Exception(id == null ?
            "Failed to get site data from database." :
            $"Failed to retrieve site data with ID value of: {id}");

        var listResult = result.ToList();

        return Ok(listResult);
    }

    [HttpPut("/siteRepository")]
    public IActionResult Put([FromBody] IEnumerable<SiteRepositoryModel> siteRepositories)
    {
        ValidateRequest(siteRepositories);

        var result = _siteRepositoryRepository.UpsertRange(siteRepositories);
        return Ok(result);
    }

    [HttpPost("/siteRepository/delete")]
    public IActionResult SoftDelete([FromBody] IEnumerable<SiteRepositoryModel> siteRepositories)
    {
        ValidateRequest(siteRepositories);

        var result = _siteRepositoryRepository.SoftDeleteRange(siteRepositories);
        return Ok(result);
    }

    [HttpPost("/siteRepository/update")]
    public IActionResult Update([FromBody] IEnumerable<SiteRepositoryModel> siteRepositories)
    {
        ValidateRequest(siteRepositories);

        var result = _siteRepositoryRepository.UpdateRange(siteRepositories);
        return Ok(result);
    }
}
