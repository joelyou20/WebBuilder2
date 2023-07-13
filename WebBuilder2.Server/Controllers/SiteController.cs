using Microsoft.AspNetCore.Mvc;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class SiteController : ControllerBase
{
    private readonly ILogger<SiteController> _logger;
    private IAwsS3Service _awsS3Service;

    public SiteController(ILogger<SiteController> logger, IAwsS3Service awsS3Service)
    {
        _logger = logger;
        _awsS3Service = awsS3Service;
    }

    [HttpGet("/site")]
    public async Task<IEnumerable<Site>> Get()
    {
        return await _awsS3Service.GetSitesAsync();
    }

    [HttpGet("/site/{id}")]
    public async Task<Site> Get([FromBody] int id)
    {
        return await _awsS3Service.GetSingleSiteAsync(id);
    }
}
