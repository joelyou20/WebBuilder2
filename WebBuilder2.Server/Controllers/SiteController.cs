using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Server.Utils;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class SiteController : ControllerBase
{
    private readonly ILogger<SiteController> _logger;
    private IAwsS3Service _awsS3Service;
    private ISiteDbService _siteService;

    public SiteController(ILogger<SiteController> logger, IAwsS3Service awsS3Service, ISiteDbService siteService)
    {
        _logger = logger;
        _awsS3Service = awsS3Service;
        _siteService = siteService;
    }

    [HttpGet("/site/{id?}")]
    public async Task<ActionResult<ValidationResponse<Site>>> Get([FromRoute] long? id)
    {
        try
        {
            return Ok(id == null ? await _siteService.GetAllAsync() : await _siteService.GetSingleAsync(id.Value));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidationResponseHelper<Site>.BuildFailedResponse(ex));
        }
    }

    [HttpPut("/site")]
    public async Task<ActionResult<ValidationResponse<Site>>> Put([FromBody] Site site)
    {
        try
        {
            return Ok(await _siteService.UpsertAsync(site));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidationResponseHelper<Site>.BuildFailedResponse(site, ex));
        }
    }

    [HttpPost("/site/delete")]
    public async Task<ActionResult<ValidationResponse<Site>>> SoftDelete([FromBody] Site site)
    {
        try
        {
            return Ok(await _siteService.SoftDeleteAsync(site));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidationResponseHelper<Site>.BuildFailedResponse(site, ex));
        }
    }
}
