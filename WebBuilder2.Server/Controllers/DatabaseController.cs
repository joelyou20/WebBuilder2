using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Server.Utils;

namespace WebBuilder2.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DatabaseController(ISqlService sqlService) : ControllerBase
{
    private readonly ISqlService _sqlService = sqlService;

    [HttpPost("/database/create")]
    public async Task<IActionResult> CreateDatabase([FromBody] string databaseName)
    {
        await _sqlService.CreateDatabaseAsync(databaseName);

        return Ok();
    }
}
