using Amazon.CertificateManager;
using Amazon.CertificateManager.Model;
using Amazon.CostExplorer;
using System.ComponentModel.DataAnnotations;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services
{
    public class AwsCertificateManagerService : IAwsCertificateManagerService
    {
        private AmazonCertificateManagerClient _client;
        private IAwsRoute53Service _awsRoute53Service;

        public AwsCertificateManagerService(AmazonCertificateManagerClient client, IAwsRoute53Service awsRoute53Service)
        {
            _client = client;
            _awsRoute53Service = awsRoute53Service;
        }

        // TODO: Create DNS Records in Route 53 automatically
        public async Task<ValidationResponse<AwsNewSSLCertificateResponse>> ProvisionNewCertificateAsync(AwsNewSSLCertificateRequest request)
        {
            var validationResult = await ValidateDomainAsync<AwsNewSSLCertificateResponse>(request.AlternativeNames.First());

            if (validationResult != null) return validationResult;

            RequestCertificateRequest awsRequest = new()
            {
                DomainName = request.DomainName,
                ValidationMethod = ValidationMethod.DNS,
                SubjectAlternativeNames = request.AlternativeNames
            };

            var awsResponse =  await _client.RequestCertificateAsync(awsRequest);

            if (awsResponse.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                return ValidationResponse<AwsNewSSLCertificateResponse>.Failure(message: $"Failed to request SSL certificate for {request.DomainName}");
            }

            AwsNewSSLCertificateResponse response = new()
            {
                Arn = awsResponse.CertificateArn,
            };

            return ValidationResponse<AwsNewSSLCertificateResponse>.Success(response);
        }

        private async Task<ValidationResponse<T>?> ValidateDomainAsync<T>(string domainName) where T : class
        {
            var hostedZonesResponse = await _awsRoute53Service.GetHostedZonesAsync();
            if(hostedZonesResponse.IsSuccessful)
            {
                bool? result = hostedZonesResponse.Values?.Any(x => x.Name.Equals($"{domainName}."));

                if (!result.HasValue || !result.Value)
                {
                    return ValidationResponse<T>.Failure(message: "Hosted Zone does not exist.");
                }

                return null;
            }

            return ValidationResponse<T>.Failure(message: hostedZonesResponse.Message);
        }
    }
}
