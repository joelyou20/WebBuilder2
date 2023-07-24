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
    public ActionResult<ValidationResponse<Site>> Get([FromRoute] long? id, [FromQuery] IEnumerable<long>? exclude = null)
    {
        try
        {
            var result = _siteRepository.Get(exclude);
            if (id != null) result = result?.Where(x => x.Id == id);

            if (result == null) throw new Exception(id == null ? 
                "Failed to get site data from database." :
                $"Failed to retrieve site data with ID value of: {id}");

            return Ok(ValidationResponse<Site>.Success(result.ToList()));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidationResponseHelper<Site>.BuildFailedResponse(ex));
        }
    }

    [HttpPut("/site")]
    public ActionResult<ValidationResponse<Site>> Put([FromBody] IEnumerable<Site> sites)
    {
        try
        {
            _siteRepository.UpsertRange(sites);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ValidationResponseHelper<Site>.BuildFailedResponse(sites, ex));
        }
    }

    [HttpPost("/site/delete")]
    public ActionResult<ValidationResponse<Site>> SoftDelete([FromBody] IEnumerable<Site> sites)
    {
        try
        {
            _siteRepository.SoftDeleteRange(sites);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ValidationResponseHelper<Site>.BuildFailedResponse(sites, ex));
        }
    }
}
