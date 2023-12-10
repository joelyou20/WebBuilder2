using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using WebBuilder2.Server.Services;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Server.Utils;

namespace WebBuilder2.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GoogleController : ControllerBase
{
    private IGoogleAdSenseService _googleAdSenseService;

    public GoogleController(IGoogleAdSenseService googleAdSenseService)
    {
        _googleAdSenseService = googleAdSenseService;
    }

    [HttpGet("/google/accounts/{name?}")]
    public async Task<IActionResult> GetAccountsAsync([FromRoute] string? name)
    {
        try
        {
            return Ok(JsonConvert.SerializeObject(await _googleAdSenseService.GetAccountsAsync(name)));
        }
        catch (HttpRequestException ex)
        {
            return (ex.StatusCode) switch
            {
                HttpStatusCode.NoContent => NoContent(),
                HttpStatusCode.UnprocessableEntity => UnprocessableEntity(JsonConvert.SerializeObject(ValidationResponseHelper.BuildFailedResponse(ex))),
                HttpStatusCode.Unauthorized => Unauthorized(JsonConvert.SerializeObject(ValidationResponseHelper.BuildFailedResponse(ex))),
                HttpStatusCode.Forbidden => Forbid(JsonConvert.SerializeObject(ValidationResponseHelper.BuildFailedResponse(ex))),
                HttpStatusCode.NotFound => NotFound(JsonConvert.SerializeObject(ValidationResponseHelper.BuildFailedResponse(ex))),
                _ => BadRequest(JsonConvert.SerializeObject(ValidationResponseHelper.BuildFailedResponse(ex)))
            };
        }
        catch (Exception ex)
        {
            return BadRequest(JsonConvert.SerializeObject(ValidationResponseHelper.BuildFailedResponse(ex)));
        }
    }

    [HttpGet("/google/payments")]
    public async Task<IActionResult> GetPaymentsAsync()
    {
        var result = await _googleAdSenseService.GetPaymentsAsync();

        return result.IsSuccessful ? Ok(JsonConvert.SerializeObject(result)) : BadRequest(JsonConvert.SerializeObject(result));
    }

    [HttpGet("/google/adclients")]
    public async Task<IActionResult> GetClientsAsync()
    {
        try
        {
            return Ok(JsonConvert.SerializeObject(await _googleAdSenseService.GetClientsAsync()));
        }
        catch (HttpRequestException ex)
        {
            return (ex.StatusCode) switch
            {
                HttpStatusCode.NoContent => NoContent(),
                HttpStatusCode.UnprocessableEntity => UnprocessableEntity(JsonConvert.SerializeObject(ValidationResponseHelper.BuildFailedResponse(ex))),
                HttpStatusCode.Unauthorized => Unauthorized(JsonConvert.SerializeObject(ValidationResponseHelper.BuildFailedResponse(ex))),
                HttpStatusCode.Forbidden => Forbid(JsonConvert.SerializeObject(ValidationResponseHelper.BuildFailedResponse(ex))),
                HttpStatusCode.NotFound => NotFound(JsonConvert.SerializeObject(ValidationResponseHelper.BuildFailedResponse(ex))),
                _ => BadRequest(JsonConvert.SerializeObject(ValidationResponseHelper.BuildFailedResponse(ex)))
            };
        }
        catch (Exception ex)
        {
            return BadRequest(JsonConvert.SerializeObject(ValidationResponseHelper.BuildFailedResponse(ex)));
        }
    }
}
