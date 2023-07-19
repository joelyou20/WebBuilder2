using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebBuilder2.Server.Services;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GithubController : ControllerBase
    {
        private IGithubService _githubConnectionService;

        public GithubController(IGithubService githubConnectionService)
        {
            _githubConnectionService = githubConnectionService;
        }

        [HttpGet("/github/repos")]
        public async Task<RespositoryResponse> Get()
        {
            return await _githubConnectionService.GetRespositoriesAsync();
        }

        [HttpPost("/github/auth")]
        public async Task<GithubAuthenticationResponse> Authenticate([FromBody] GithubAuthenticationRequest request)
        {
            return await _githubConnectionService.AuthenticateUserAsync(request);
        }
    }
}
