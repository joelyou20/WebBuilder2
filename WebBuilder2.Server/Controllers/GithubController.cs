using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Octokit;
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

        #region GitIgnore

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

        #endregion

        #region License

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

        #endregion

        #region User

        [HttpGet("/github/user")]
        public async Task<IActionResult> GetUserAsync()
        {
            try
            {
                return Ok(JsonConvert.SerializeObject(await _githubService.GetUserAsync()));
            }
            catch (AuthorizationException ex)
            {
                return BadRequest(JsonConvert.SerializeObject(ValidationResponseHelper<string>.BuildFailedResponse(ex)));
            }
        }

        #endregion

        #region Repos

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

        [HttpPost("/github/repos/{owner}/{repoName}")]
        public async Task<IActionResult> PostRepositoryContent([FromRoute] string owner, [FromRoute] string repoName, [FromBody] string? path = null)
        {
            try
            {
                return Ok(JsonConvert.SerializeObject(await _githubService.GetRepositoryContentAsync(owner, repoName, path)));
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

        [HttpPost("/github/repos/copy")]
        public async Task<IActionResult> PostCopyRepoAsync([FromBody] GithubCopyRepoRequest githubCopyRepoRequest)
        {
            try
            {
                return Ok(JsonConvert.SerializeObject(await _githubService.CopyRepoAsync(githubCopyRepoRequest.ClonedRepoName, githubCopyRepoRequest.NewRepoName, githubCopyRepoRequest.Path)));
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

        #endregion

        #region Git

        [HttpGet("/github/git/tree/{owner}/{repoName}")]
        public async Task<IActionResult> GetGitTree([FromRoute] string owner, [FromRoute] string repoName)
        {
            try
            {
                return Ok(JsonConvert.SerializeObject(await _githubService.GetGitTreeAsync(owner, repoName)));
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

        #endregion

        #region Auth

        [HttpPost("/github/auth")]
        public async Task<IActionResult> Authenticate()
        {
            try
            {
                return Ok(JsonConvert.SerializeObject(await _githubService.AuthenticateUserAsync()));
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(ValidationResponseHelper.BuildFailedResponse(ex)));
            }
        }

        #endregion

        #region Secrets

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

        [HttpPut("/github/secrets/{owner}/{repoName}")]
        public async Task<IActionResult> CreateSecret([FromRoute] string owner, [FromRoute] string repoName, [FromBody] IEnumerable<GithubSecret> secret)
        {
            try
            {
                return Created($"github/secrets/{owner}/{repoName}", JsonConvert.SerializeObject(await _githubService.CreateSecretAsync(secret, owner, repoName)));
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

        #endregion

        #region Commit

        [HttpPut("/github/commit/{owner}/{repoName}")]
        public async Task<IActionResult> CreateCommit([FromRoute] string owner, [FromRoute] string repoName, [FromBody] GithubCreateCommitRequest commit)
        {
            try
            {
                return Created($"github/commit/{owner}/{repoName}", JsonConvert.SerializeObject(await _githubService.CreateCommitAsync(owner, repoName, commit)));
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

        #endregion
    }
}
