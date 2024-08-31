using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GoogleController(IGoogleAdSenseService googleAdSenseService) : ControllerBase
{
    private readonly IGoogleAdSenseService _googleAdSenseService = googleAdSenseService;

    [HttpGet("/google/accounts/{name}")]
    public async Task<IActionResult> GetSingleAccountByNameAsync([FromRoute] string name)
    {
        var result = await _googleAdSenseService.GetSingleAccountByNameAsync(name);

        return Ok(result);
    }

    [HttpGet("/google/accounts")]
    public async Task<IActionResult> GetAccountsAsync()
    {
        var result = await _googleAdSenseService.GetAccountsAsync();

        return Ok(result);
    }

    [HttpGet("/google/payments")]
    public async Task<IActionResult> GetPaymentsAsync()
    {
        IEnumerable<GooglePayment> result = await _googleAdSenseService.GetPaymentsAsync();

        return Ok(result);
    }

    [HttpGet("/google/adclients")]
    public async Task<IActionResult> GetClientsAsync()
    {
        IEnumerable<GoogleAdClient> result = await _googleAdSenseService.GetClientsAsync();

        return Ok(result);
    }
}
