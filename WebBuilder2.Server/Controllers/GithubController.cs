using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebBuilder2.Server.Services;
using WebBuilder2.Server.Services.Contracts;

namespace WebBuilder2.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GithubController : ControllerBase
    {
        private IGithubConnectionService _githubConnectionService;

        public GithubController(IGithubConnectionService githubConnectionService)
        {
            _githubConnectionService = githubConnectionService;
        }

        [HttpPost("/github")]
        public async Task Post()
        {
            await _githubConnectionService.ConnectAsync();
        }
    }
}
