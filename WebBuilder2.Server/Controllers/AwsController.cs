using Microsoft.AspNetCore.Mvc;
using WebBuilder2.Server.Services;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AwsController : ControllerBase
    {
        private IAwsS3Service _awsS3Service;
        private IAwsRoute53Service _awsRoute53Service;

        public AwsController(IAwsS3Service awsS3Service, IAwsRoute53Service awsRoute53Service)
        {
            _awsS3Service = awsS3Service;
            _awsRoute53Service = awsRoute53Service;
        }

        [HttpGet("/aws/buckets")]
        public async Task<IEnumerable<Bucket>> GetBucketsAsync()
        {
            return await _awsS3Service.GetBucketsAsync();
        }

        [HttpGet("/aws/hostedzones")]
        public async Task<IEnumerable<HostedZone>> GetHostedZonesAsync()
        {
            return await _awsRoute53Service.GetHostedZonesAsync();
        }
    }
}
