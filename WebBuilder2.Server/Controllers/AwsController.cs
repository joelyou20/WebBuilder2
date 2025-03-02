using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Server.Utils;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AwsController(IAwsS3Service awsS3Service, IAwsRoute53Service awsRoute53Service,
        IAwsCostExplorerService awsCostExplorerService, IAwsRoute53DomainsService awsRoute53DomainsService, 
        IAwsCertificateManagerService awsCertificateManagerService) : CustomControllerBase
    {
        private readonly IAwsS3Service _awsS3Service = awsS3Service;
        private readonly IAwsRoute53Service _awsRoute53Service = awsRoute53Service;
        private readonly IAwsRoute53DomainsService _awsRoute53DomainsService = awsRoute53DomainsService;
        private readonly IAwsCostExplorerService _awsCostExplorerService = awsCostExplorerService;
        private readonly IAwsCertificateManagerService _awsCertificateManagerService = awsCertificateManagerService;

        [HttpGet("/aws/bucket/{name}")]
        public async Task<IActionResult> GetSingleBucketAsync([FromRoute] string name)
        {
            Bucket result = await _awsS3Service.GetSingleBucketAsync(name);

            return Ok(result);
        }

        [HttpGet("/aws/buckets")]
        public async Task<IActionResult> GetBucketsAsync()
        {
            var result = await _awsS3Service.GetBucketsAsync();

            return Ok(result);
        }

        [HttpPut("/aws/buckets")]
        public async Task<IActionResult> CreateBucketsAsync([FromBody] AwsCreateBucketRequest request)
        {
            await _awsS3Service.CreateBucketAsync(request);
            return Created();
        }

        [HttpPost("/aws/buckets/logging")]
        public async Task<IActionResult> PostLoggingConfigAsync([FromBody] AwsConfigureLoggingRequest request)
        {
            await _awsS3Service.ConfigureLoggingAsync(request);

            return Ok();
        }

        [HttpPost("/aws/buckets/policy")]
        public async Task<IActionResult> PostBucketPolicyAsync([FromBody] AwsAddBucketPolicyRequest request)
        {
            await _awsS3Service.AddBucketPolicyAsync(request);

            return Ok();
        }

        [HttpPost("/aws/buckets/access")]
        public async Task<IActionResult> PostConfigurePublicAccessBlockAsync([FromBody] AwsPublicAccessBlockRequest request)
        {
            await _awsS3Service.ConfigurePublicAccessBlockAsync(request);

            return Ok();
        }

        [HttpGet("/aws/hostedzones")]
        public async Task<IActionResult> GetHostedZonesAsync()
        {
            IEnumerable<HostedZone> result = await _awsRoute53Service.GetHostedZonesAsync();

            return Ok(result);
        }

        [HttpGet("/aws/cost")]
        public async Task<IActionResult> GetForecastedCostAsync()
        {
            string result = await _awsCostExplorerService.GetForecastedCostAsync();

            return Ok(result);
        }

        [HttpGet("/aws/route53/domain/suggest/{domain}")]
        public async Task<IActionResult> GetSuggestedDomainNamesAsync([FromRoute] string domain)
        {
            IEnumerable<DomainInquiry> result = await _awsRoute53DomainsService.GetDomainSuggestionsAsync(domain, true);

            return Ok(result);
        }

        [HttpGet("/aws/route53/domain/")]
        public async Task<IActionResult> GetRegisteredDomains()
        {
            IEnumerable<Domain> result = await _awsRoute53DomainsService.GetRegisteredDomainsAsync();

            return Ok(result);
        }

        [HttpPost("/aws/route53/domain/register")]
        public async Task<IActionResult> GetRegisteredDomains([FromBody] string domainName)
        {
            await _awsRoute53DomainsService.RegisterDomainAsync(domainName);

            return Ok();
        }

        [HttpPost("/aws/cert")]
        public async Task<IActionResult> PostNewSSLCertificate([FromBody] AwsNewSSLCertificateRequest request)
        {
            AwsNewSSLCertificateResponse? result = await _awsCertificateManagerService.ProvisionNewCertificateAsync(request);

            if(result == null) return BadRequest(result);

            return Ok(result);
        }
    }
}
