using Amazon.S3.Model;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services.Contracts
{
    public interface IAwsS3Service
    {
        Task<Bucket> GetSingleBucketAsync(string name);
        Task<IEnumerable<Bucket>> GetBucketsAsync();
        Task CreateBucketAsync(AwsCreateBucketRequest request);
        Task<PutBucketLoggingResponse> ConfigureLoggingAsync(AwsConfigureLoggingRequest request);
        Task<PutBucketPolicyResponse> AddBucketPolicyAsync(AwsAddBucketPolicyRequest request);
        Task<PutPublicAccessBlockResponse> ConfigurePublicAccessBlockAsync(AwsPublicAccessBlockRequest request);
    }
}
