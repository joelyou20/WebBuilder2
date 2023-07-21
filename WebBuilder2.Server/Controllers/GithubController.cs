using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebBuilder2.Server.Services;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GithubController : ControllerBase
    {
        private IGithubService _githubService;

        public GithubController(IGithubService githubService)
        {
            _githubService = githubService;
        }

        [HttpGet("/github/repos")]
        public async Task<RespositoryResponse> Get()
        {
            return await _githubService.GetRespositoriesAsync();
        }

        [HttpGet("/github/gitignore")]
        public async Task<IEnumerable<string>> GetGitIgnoreTemplates()
        {
            return await _githubService.GetGitIgnoreTemplatesAsync();
        }

        [HttpGet("/github/license")]
        public async Task<IEnumerable<GithubProjectLicense>> GetLicenseTemplates()
        {
            return await _githubService.GetLicenseTemplatesAsync();
        }

        [HttpPost("/github/repos/create")]
        public async Task<ActionResult<GithubCreateRepoResponse>> Create([FromBody] GithubCreateRepoRequest request)
        {
            var result = await _githubService.CreateRepoAsync(request);
            return result.Errors.Any() ? BadRequest(result) : Ok(result);
        }

        [HttpPost("/github/auth")]
        public async Task<GithubAuthenticationResponse> Authenticate([FromBody] GithubAuthenticationRequest request)
        {
            return await _githubService.AuthenticateUserAsync(request);
        }
    }
}
