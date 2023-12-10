using Amazon.S3.Model;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services.Contracts
{
    public interface IAwsS3Service
    {
        Task<ValidationResponse<Bucket>> GetSingleBucketAsync(string name);
        Task<ValidationResponse<Bucket>> GetBucketsAsync();
        Task<ValidationResponse> CreateBucketAsync(AwsCreateBucketRequest request);
        Task<ValidationResponse> ConfigureLoggingAsync(AwsConfigureLoggingRequest request);
        Task<ValidationResponse> AddBucketPolicyAsync(AwsAddBucketPolicyRequest request);
        Task<ValidationResponse> ConfigurePublicAccessBlockAsync(AwsPublicAccessBlockRequest request);
    }
}
