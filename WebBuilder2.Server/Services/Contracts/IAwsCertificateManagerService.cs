using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services.Contracts
{
    public interface IAwsCertificateManagerService
    {
        Task<ValidationResponse<AwsNewSSLCertificateResponse>> ProvisionNewCertificateAsync(AwsNewSSLCertificateRequest request);
    }
}
