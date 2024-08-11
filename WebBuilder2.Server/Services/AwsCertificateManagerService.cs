using Amazon.CertificateManager;
using Amazon.CertificateManager.Model;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Server.Services
{
    public class AwsCertificateManagerService(AmazonCertificateManagerClient client, IAwsRoute53Service awsRoute53Service) : IAwsCertificateManagerService
    {
        private readonly AmazonCertificateManagerClient _client = client;
        private readonly IAwsRoute53Service _awsRoute53Service = awsRoute53Service;

        // WHAT I DID TO GET SSL TO WORK
        // 1. Set Cloudfront Distribution Alternate Domain Names to the root and sub root (example.com and www.example.com)
        // 2. Updated the Alias records in Route53 to use "Alias to Cloudfront distribution"
        // 3. Edited the Cloudfront distribution Behaviour to Redirect Http to Https (This is optional)
        // TODO: Create DNS Records in Route 53 automatically
        // Would be lovely to have this alllll automated
        public async Task<AwsNewSSLCertificateResponse?> ProvisionNewCertificateAsync(AwsNewSSLCertificateRequest request)
        {
            var validationResult = await ValidateDomainAsync(request.AlternativeNames.First());

            if (!validationResult) return null;

            RequestCertificateRequest awsRequest = new()
            {
                DomainName = request.DomainName,
                ValidationMethod = ValidationMethod.DNS,
                SubjectAlternativeNames = request.AlternativeNames
            };

            var awsResponse =  await _client.RequestCertificateAsync(awsRequest);

            if (awsResponse.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception(message: $"{awsResponse.HttpStatusCode}: Failed to request SSL certificate for {request.DomainName}");
            }

            AwsNewSSLCertificateResponse response = new()
            {
                Arn = awsResponse.CertificateArn,
            };

            return response;
        }

        private async Task<bool> ValidateDomainAsync(string domainName)
        {
            IEnumerable<HostedZone> hostedZones = await _awsRoute53Service.GetHostedZonesAsync();
            bool? result = hostedZones.Any(x => x.Name.Equals($"{domainName}."));

            return result.HasValue && result.Value;
        }
    }
}
