﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<ValidationResponse<RepositoryModel>>> Get()
        {
            try
            {
                return Ok(await _githubService.GetRepositoriesAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ValidationResponseHelper<RepositoryModel>.BuildFailedResponse(ex));
            }
        }

        [HttpGet("/github/gitignore")]
        public async Task<ActionResult<ValidationResponse<string>>> GetGitIgnoreTemplates()
        {
            try
            {
                return Ok(await _githubService.GetGitIgnoreTemplatesAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ValidationResponseHelper<string>.BuildFailedResponse(ex));
            }
        }

        [HttpGet("/github/license")]
        public async Task<ActionResult<ValidationResponse<GithubProjectLicense>>> GetLicenseTemplates()
        {
            try
            {
                return Ok(await _githubService.GetLicenseTemplatesAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ValidationResponseHelper<GithubProjectLicense>.BuildFailedResponse(ex));
            }
        }

        [HttpPost("/github/repos/create")]
        public async Task<ActionResult<RepositoryModel>> Create([FromBody] RepositoryModel repository)
        {
            try
            {
                return Ok(await _githubService.CreateRepoAsync(repository));
            }
            catch (Exception ex)
            {
                return BadRequest(ValidationResponseHelper<RepositoryModel>.BuildFailedResponse(repository, ex));
            }
        }

        [HttpPost("/github/auth")]
        public async Task<ActionResult<ValidationResponse>> Authenticate([FromBody] GithubAuthenticationRequest request)
        {
            try
            {
                return Ok(await _githubService.AuthenticateUserAsync(request));
            }
            catch (Exception ex)
            {
                return BadRequest(ValidationResponseHelper<GithubAuthenticationRequest>.BuildFailedResponse(request, ex));
            }
        }
    }
}
