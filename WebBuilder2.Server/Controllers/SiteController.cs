using Microsoft.AspNetCore.Mvc;
using WebBuilder2.Server.Services.Contracts;
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

    [HttpGet("/site")]
    public async Task<ValidationResponse<Site>> Get()
    {
        return await _siteService.GetAllAsync();
    }

    [HttpGet("/site/{long}")]
    public async Task<ValidationResponse<Site>> Get([FromRoute] long id)
    {
        return await _siteService.GetSingleAsync(id);
    }

    [HttpPut("/site")]
    public async Task<ValidationResponse<Site>> Put([FromBody] Site site)
    {
        return await _siteService.UpsertAsync(site);
    } 
}
