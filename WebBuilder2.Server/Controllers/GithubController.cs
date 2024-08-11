using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GithubController(IGithubService githubService) : ControllerBase
    {
        private readonly IGithubService _githubService = githubService;

        #region GitIgnore

        [HttpGet("/github/gitignore")]
        public async Task<IActionResult> GetGitIgnoreTemplates()
        {
            GitIgnoreTemplateResponse result = await _githubService.GetGitIgnoreTemplatesAsync();

            return Ok(result);
        }

        #endregion

        #region License

        [HttpGet("/github/license")]
        public async Task<IActionResult> GetLicenseTemplates()
        {
            IEnumerable<GithubProjectLicense> result = await _githubService.GetLicenseTemplatesAsync();

            return Ok(result);
        }

        #endregion

        #region User

        [HttpGet("/github/user")]
        public async Task<IActionResult> GetUserAsync()
        {
            string result = await _githubService.GetUserAsync();
            string json = JsonConvert.SerializeObject(result);

            return Ok(json);
        }

        #endregion

        #region Repos

        [HttpGet("/github/repos")]
        public async Task<IActionResult> Get()
        {
            IEnumerable<RepositoryModel> result = await _githubService.GetRepositoriesAsync();

            return Ok(result);
        }

        [HttpPost("/github/repos/create")]
        public async Task<IActionResult> Create([FromBody] RepositoryModel repository)
        {
            RepositoryModel result = await _githubService.CreateRepoAsync(repository);

            return Ok(result);
        }

        [HttpPost("/github/repos/{owner}/{repoName}")]
        public async Task<IActionResult> PostRepositoryContent([FromRoute] string owner, [FromRoute] string repoName, [FromBody] string? path = null)
        {
            IEnumerable<RepoContent> result = await _githubService.GetRepositoryContentAsync(owner, repoName, path);

            return Ok(result);
        }

        [HttpPost("/github/repos/copy")]
        public async Task<IActionResult> PostCopyRepoAsync([FromBody] GithubCopyRepoRequest githubCopyRepoRequest)
        {
            if (githubCopyRepoRequest == null || githubCopyRepoRequest?.Path == null)
            {
                return BadRequest(githubCopyRepoRequest);
            }

            await _githubService.CopyRepoAsync(githubCopyRepoRequest.ClonedRepoName, githubCopyRepoRequest.NewRepoName, githubCopyRepoRequest.Path);

            return Ok(githubCopyRepoRequest);
        }

        #endregion

        #region Git

        [HttpGet("/github/git/tree/{owner}/{repoName}")]
        public async Task<IActionResult> GetGitTree([FromRoute] string owner, [FromRoute] string repoName)
        {
            IEnumerable<GitTreeItem> result = await _githubService.GetGitTreeAsync(owner, repoName);

            return Ok(result);
        }

        #endregion

        #region Auth

        [HttpPost("/github/auth")]
        public async Task<IActionResult> Authenticate()
        {
            await _githubService.AuthenticateUserAsync();

            return Ok();
        }

        #endregion

        #region Secrets

        [HttpGet("/github/secrets/{userName}/{repoName}")]
        public async Task<IActionResult> GetSecrets([FromRoute] string userName, [FromRoute] string repoName)
        {
            IEnumerable<GithubSecret> result = await _githubService.GetSecretsAsync(userName, repoName);

            return Ok(result);
        }

        [HttpPut("/github/secrets/{owner}/{repoName}")]
        public async Task<IActionResult> CreateSecret([FromRoute] string owner, [FromRoute] string repoName, [FromBody] IEnumerable<GithubSecret> secret)
        {
            var result = await _githubService.CreateSecretAsync(secret, owner, repoName);

            return Created($"github/secrets/{owner}/{repoName}", result);
        }

        #endregion

        #region Commit

        [HttpPut("/github/commit/{owner}/{repoId:long}")]
        public async Task<IActionResult> CreateCommitV2([FromRoute] string owner, [FromRoute] long repoId, [FromBody] GithubCreateCommitRequest commit)
        {
            await _githubService.CreateCommitAsync(owner, repoId, commit);

            return Created();
        }

        #endregion
    }
}
