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

        [HttpPost("/github/repos/create")]
        public async Task<GithubCreateRepoResponse> Create([FromBody] GithubCreateRepoRequest request)
        {
            return await _githubService.CreateRepoAsync(request);
        }

        [HttpPost("/github/auth")]
        public async Task<GithubAuthenticationResponse> Authenticate([FromBody] GithubAuthenticationRequest request)
        {
            return await _githubService.AuthenticateUserAsync(request);
        }
    }
}
