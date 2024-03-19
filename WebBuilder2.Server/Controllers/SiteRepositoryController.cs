using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebBuilder2.Server.Repositories.Contracts;
using WebBuilder2.Server.Utils;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class SiteRepositoryController : ControllerBase
{
    private ISiteRepositoryRepository _siteRepositoryRepository;

    public SiteRepositoryController(ISiteRepositoryRepository siteRepositoryRepository)
    {
        _siteRepositoryRepository = siteRepositoryRepository;
    }

    [HttpGet("/siteRepository/{id?}")]
    public IActionResult Get([FromRoute] long? id, [FromQuery] IEnumerable<long>? exclude = null)
    {
        try
        {
            var result = _siteRepositoryRepository.Get(exclude);
            if (id != null) result = result?.Where(x => x.Id == id);

            if (result == null) throw new Exception(id == null ?
                "Failed to get site data from database." :
                $"Failed to retrieve site data with ID value of: {id}");

            var listResult = result.ToList();

            return Ok(JsonConvert.SerializeObject(ValidationResponse<SiteRepositoryModel>.Success(listResult)));
        }
        catch (Exception ex)
        {
            return BadRequest(JsonConvert.SerializeObject(ValidationResponseHelper<SiteRepositoryModel>.BuildFailedResponse(ex)));
        }
    }

    [HttpPut("/siteRepository")]
    public IActionResult Put([FromBody] IEnumerable<SiteRepositoryModel> sites)
    {
        try
        {
            var result = _siteRepositoryRepository.UpsertRange(sites);
            return Ok(ValidationResponse<SiteRepositoryModel>.Success(result));
        }
        catch (Exception ex)
        {
            return BadRequest(JsonConvert.SerializeObject(ValidationResponseHelper<SiteRepositoryModel>.BuildFailedResponse(sites, ex)));
        }
    }

    [HttpPost("/siteRepository/delete")]
    public IActionResult SoftDelete([FromBody] IEnumerable<SiteRepositoryModel> sites)
    {
        try
        {
            var result = _siteRepositoryRepository.SoftDeleteRange(sites);
            return Ok(JsonConvert.SerializeObject(ValidationResponse<SiteRepositoryModel>.Success(result)));
        }
        catch (Exception ex)
        {
            return BadRequest(JsonConvert.SerializeObject(ValidationResponseHelper<SiteRepositoryModel>.BuildFailedResponse(sites, ex)));
        }
    }

    [HttpPost("/siteRepository/update")]
    public IActionResult Update([FromBody] IEnumerable<SiteRepositoryModel> sites)
    {
        try
        {
            var result = _siteRepositoryRepository.UpdateRange(sites);
            return Ok(JsonConvert.SerializeObject(ValidationResponse<SiteRepositoryModel>.Success(result)));
        }
        catch (Exception ex)
        {
            return BadRequest(JsonConvert.SerializeObject(ValidationResponseHelper<SiteRepositoryModel>.BuildFailedResponse(sites, ex)));
        }
    }
}
