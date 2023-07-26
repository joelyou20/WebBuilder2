using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebBuilder2.Server.Repositories.Contracts;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Server.Utils;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class SiteController : ControllerBase
{
    private ISiteRepository _siteRepository;

    public SiteController(ISiteRepository siteRepository)
    {
        _siteRepository = siteRepository;
    }

    [HttpGet("/site/{id?}")]
    public ActionResult<ValidationResponse<SiteModel>> Get([FromRoute] long? id, [FromQuery] IEnumerable<long>? exclude = null)
    {
        try
        {
            var result = _siteRepository.Get(exclude);
            if (id != null) result = result?.Where(x => x.Id == id);

            if (result == null) throw new Exception(id == null ? 
                "Failed to get site data from database." :
                $"Failed to retrieve site data with ID value of: {id}");

            var listResult = result.ToList();

            return Ok(ValidationResponse<SiteModel>.Success(listResult));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidationResponseHelper<SiteModel>.BuildFailedResponse(ex));
        }
    }

    [HttpPut("/site")]
    public ActionResult<ValidationResponse<SiteModel>> Put([FromBody] IEnumerable<SiteModel> sites)
    {
        try
        {
            var result = _siteRepository.UpsertRange(sites);
            return Ok(ValidationResponse<SiteModel>.Success(result));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidationResponseHelper<SiteModel>.BuildFailedResponse(sites, ex));
        }
    }

    [HttpPost("/site/delete")]
    public ActionResult<ValidationResponse<SiteModel>> SoftDelete([FromBody] IEnumerable<SiteModel> sites)
    {
        try
        {
            var result = _siteRepository.SoftDeleteRange(sites);
            return Ok(ValidationResponse<SiteModel>.Success(result));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidationResponseHelper<SiteModel>.BuildFailedResponse(sites, ex));
        }
    }
}
