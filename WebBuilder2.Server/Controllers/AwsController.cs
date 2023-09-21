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
    public class AwsController : ControllerBase
    {
        private IAwsS3Service _awsS3Service;
        private IAwsRoute53Service _awsRoute53Service;
        private IAwsRoute53DomainsService _awsRoute53DomainsService;
        private IAwsCostExplorerService _awsCostExplorerService;
        private IAwsAmplifyService _awsAmplifyService;

        public AwsController(IAwsS3Service awsS3Service, IAwsRoute53Service awsRoute53Service, 
            IAwsCostExplorerService awsCostExplorerService, IAwsAmplifyService awsAmplifyService,
            IAwsRoute53DomainsService awsRoute53DomainsService)
        {
            _awsS3Service = awsS3Service;
            _awsRoute53Service = awsRoute53Service;
            _awsRoute53DomainsService = awsRoute53DomainsService;
            _awsCostExplorerService = awsCostExplorerService;
            _awsAmplifyService = awsAmplifyService;
        }

        [HttpGet("/aws/bucket/{name}")]
        public async Task<IActionResult> GetSingleBucketAsync([FromRoute] string name)
        {
            try
            {
                return Ok(JsonConvert.SerializeObject(await _awsS3Service.GetSingleBucketAsync(name)));
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

        [HttpGet("/aws/buckets")]
        public async Task<IActionResult> GetBucketsAsync()
        {
            try
            {
                return Ok(JsonConvert.SerializeObject(await _awsS3Service.GetBucketsAsync()));
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

        [HttpPut("/aws/buckets")]
        public async Task<IActionResult> CreateBucketsAsync([FromBody] AwsCreateBucketRequest request)
        {
            try
            {
                return Ok(JsonConvert.SerializeObject(await _awsS3Service.CreateBucketAsync(request)));
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

        [HttpPost("/aws/buckets/logging")]
        public async Task<IActionResult> PostLoggingConfigAsync([FromBody] AwsConfigureLoggingRequest request)
        {
            try
            {
                return Ok(JsonConvert.SerializeObject(await _awsS3Service.ConfigureLoggingAsync(request)));
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

        [HttpPost("/aws/buckets/policy")]
        public async Task<IActionResult> PostBucketPolicyAsync([FromBody] AwsAddBucketPolicyRequest request)
        {
            try
            {
                return Ok(JsonConvert.SerializeObject(await _awsS3Service.AddBucketPolicyAsync(request)));
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

        [HttpPost("/aws/buckets/access")]
        public async Task<IActionResult> PostConfigurePublicAccessBlockAsync([FromBody] AwsPublicAccessBlockRequest request)
        {
            try
            {
                return Ok(JsonConvert.SerializeObject(await _awsS3Service.ConfigurePublicAccessBlockAsync(request)));
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

        [HttpGet("/aws/hostedzones")]
        public async Task<IActionResult> GetHostedZonesAsync()
        {
            try
            {
                return Ok(JsonConvert.SerializeObject(await _awsRoute53Service.GetHostedZonesAsync()));
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

        [HttpGet("/aws/cost")]
        public async Task<IActionResult> GetForecastedCostAsync()
        {
            try
            {
                return Ok(JsonConvert.SerializeObject(await _awsCostExplorerService.GetForecastedCostAsync()));
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

        [HttpPost("/aws/app")]
        public async Task<IActionResult> PostAppAsync([FromBody] RepositoryModel repo)
        {
            try
            {
                return Ok(JsonConvert.SerializeObject(await _awsAmplifyService.CreateAppFromRepoAsync(repo)));
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

        [HttpGet("/aws/route53/domain/suggest/{domain}")]
        public async Task<IActionResult> GetSuggestedDomainNamesAsync([FromRoute] string domain)
        {
            try
            {
                return Ok(JsonConvert.SerializeObject(await _awsRoute53DomainsService.GetDomainSuggestionsAsync(domain, true)));
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

        [HttpGet("/aws/route53/domain/")]
        public async Task<IActionResult> GetRegisteredDomains()
        {
            try
            {
                return Ok(JsonConvert.SerializeObject(await _awsRoute53DomainsService.GetRegisteredDomainsAsync()));
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

        [HttpPost("/aws/route53/domain/register")]
        public async Task<IActionResult> GetRegisteredDomains([FromBody] string domainName)
        {
            try
            {
                return Ok(JsonConvert.SerializeObject(await _awsRoute53DomainsService.RegisterDomainAsync(domainName)));
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
