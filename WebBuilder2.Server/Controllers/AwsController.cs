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

        [HttpGet("/aws/buckets")]
        public async Task<IEnumerable<Bucket>> GetBucketsAsync()
        {
            return await _awsS3Service.GetBucketsAsync();
        }

        [HttpPut("/aws/buckets")]
        public async Task<ValidationResponse> CreateBucketsAsync([FromBody] AwsCreateBucketRequest request)
        {
            return await _awsS3Service.CreateBucketAsync(request);
        }

        [HttpGet("/aws/hostedzones")]
        public async Task<IEnumerable<HostedZone>> GetHostedZonesAsync()
        {
            return await _awsRoute53Service.GetHostedZonesAsync();
        }

        [HttpGet("/aws/cost")]
        public async Task<string> GetForecastedCostAsync()
        {
            return await _awsCostExplorerService.GetForecastedCostAsync();
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
