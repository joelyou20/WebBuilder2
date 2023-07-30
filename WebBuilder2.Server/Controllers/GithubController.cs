using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using WebBuilder2.Server.Services;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Server.Utils;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

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
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(JsonConvert.SerializeObject(await _githubService.GetRepositoriesAsync()));
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(ValidationResponseHelper<RepositoryModel>.BuildFailedResponse(ex)));
            }
        }

        [HttpGet("/github/gitignore")]
        public async Task<IActionResult> GetGitIgnoreTemplates()
        {
            try
            {
                return Ok(JsonConvert.SerializeObject(await _githubService.GetGitIgnoreTemplatesAsync()));
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(ValidationResponseHelper<string>.BuildFailedResponse(ex)));
            }
        }

        [HttpGet("/github/license")]
        public async Task<IActionResult> GetLicenseTemplates()
        {
            try
            {
                return Ok(JsonConvert.SerializeObject(await _githubService.GetLicenseTemplatesAsync()));
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(ValidationResponseHelper<GithubProjectLicense>.BuildFailedResponse(ex)));
            }
        }

        [HttpGet("/github/user")]
        public async Task<IActionResult> GetUserAsync()
        {
            try
            {
                return Ok(JsonConvert.SerializeObject(await _githubService.GetUserAsync()));
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(ValidationResponseHelper<string>.BuildFailedResponse(ex)));
            }
        }

        [HttpPost("/github/repos/create")]
        public async Task<IActionResult> Create([FromBody] RepositoryModel repository)
        {
            try
            {
                return Ok(JsonConvert.SerializeObject(await _githubService.CreateRepoAsync(repository)));
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(ValidationResponseHelper<RepositoryModel>.BuildFailedResponse(repository, ex)));
            }
        }

        [HttpPost("/github/auth")]
        public async Task<IActionResult> Authenticate([FromBody] GithubAuthenticationRequest request)
        {
            try
            {
                return Ok(JsonConvert.SerializeObject(await _githubService.AuthenticateUserAsync(request)));
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(ValidationResponseHelper<GithubAuthenticationRequest>.BuildFailedResponse(request, ex)));
            }
        }

        [HttpGet("/github/secrets/{userName}/{repoName}")]
        public async Task<IActionResult> GetSecrets([FromRoute] string userName, [FromRoute] string repoName)
        {
            try
            {
                return Ok(JsonConvert.SerializeObject(await _githubService.GetSecretsAsync(userName, repoName)));
            }
            catch(HttpRequestException ex)
            {
                return (ex.StatusCode) switch
                {
                    HttpStatusCode.NoContent => NoContent(),
                    HttpStatusCode.Unauthorized => Unauthorized(JsonConvert.SerializeObject(ValidationResponseHelper<GithubSecretResponse>.BuildFailedResponse(ex))),
                    HttpStatusCode.Forbidden => Forbid(JsonConvert.SerializeObject(ValidationResponseHelper<GithubSecretResponse>.BuildFailedResponse(ex))),
                    HttpStatusCode.NotFound => NotFound(JsonConvert.SerializeObject(ValidationResponseHelper<GithubSecretResponse>.BuildFailedResponse(ex))),
                    _ => BadRequest(JsonConvert.SerializeObject(ValidationResponseHelper<GithubSecretResponse>.BuildFailedResponse(ex)))
                };
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(ValidationResponseHelper<GithubSecretResponse>.BuildFailedResponse(ex)));
            }
        }

        [HttpPut("/github/secrets/{userName}/{repoName}")]
        public async Task<IActionResult> CreateSecret([FromRoute] string userName, [FromRoute] string repoName, [FromBody] GithubSecret secret)
        {
            try
            {
                return Created($"github/secrets/{userName}/{repoName}", JsonConvert.SerializeObject(await _githubService.CreateSecretAsync(secret, userName, repoName)));
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
}
