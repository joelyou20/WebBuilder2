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

        public AwsCertificateManagerService(AmazonCertificateManagerClient client)
        {
            _client = client;
        }

        public async Task<ValidationResponse<AwsNewSSLCertificateResponse>> ProvisionNewCertificateAsync(AwsNewSSLCertificateRequest request)
        {
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
    }
}
