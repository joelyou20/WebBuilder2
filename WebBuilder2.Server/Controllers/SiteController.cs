using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using WebBuilder2.Server.Repositories;
using WebBuilder2.Server.Repositories.Contracts;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Server.Utils;
using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class SiteController(ISiteRepository siteRepository) : CustomControllerBase
{
    private readonly ISiteRepository _siteRepository = siteRepository;

    [HttpGet("/site/{id?}")]
    public IActionResult Get([FromRoute] long? id, [FromQuery] IEnumerable<long>? exclude = null)
    {
            var result = _siteRepository.Get(exclude);
        if (id != null) result = result?.Where(x => x.Id == id);

        if (result == null) throw new Exception(id == null ? 
            "Failed to get site data from database." :
            $"Failed to retrieve site data with ID value of: {id}");

        var listResult = result.ToList();

        return Ok(listResult);
    }

    [HttpPut("/site")]
    public IActionResult Put([FromBody] IEnumerable<SiteModel> sites)
    {
        ValidateRequest(sites);

        var result = _siteRepository.UpsertRange(sites);
        return Ok(result);
    }

    [HttpPost("/site/delete")]
    public IActionResult SoftDelete([FromBody] IEnumerable<SiteModel> sites)
    {
        ValidateRequest(sites);

        var result = _siteRepository.SoftDeleteRange(sites);
        return Ok(result);
    }

    [HttpPost("/site/update")]
    public IActionResult Update([FromBody] IEnumerable<SiteModel> sites)
    {
        ValidateRequest(sites);

        var result = _siteRepository.UpdateRange(sites);
        return Ok(result);
    }
}
