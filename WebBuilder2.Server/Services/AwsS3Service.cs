using Amazon.S3;
using Amazon.S3.Model;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services;

public class AwsS3Service : IAwsS3Service
{
    private AmazonS3Client _client;

    public AwsS3Service(AmazonS3Client client)
    {
        _client = client;
    }

    public async Task<Bucket> GetSingleBucketAsync(string name)
    {
        ListBucketsResponse bucketResponse = await _client.ListBucketsAsync(); 

        var bucket = bucketResponse.Buckets.Single(bucket => bucket.BucketName.Equals(name));

        return new Bucket
        {
            Name = bucket.BucketName
        };
    }

    public async Task<IEnumerable<Bucket>> GetBucketsAsync()
    {
        ListBucketsResponse bucketResponse = await _client.ListBucketsAsync();
        IEnumerable<Bucket> buckets = bucketResponse.Buckets.Select(x => new Bucket
        {
            Name = x.BucketName
        });

        return buckets;
    }

    public async Task<ValidationResponse> CreateBucketAsync(AwsCreateBucketRequest request)
    {
        foreach(var bucket in request.Buckets)
        {
            var putBucketRequest = new PutBucketRequest
            {
                BucketName = bucket.Name,
                BucketRegion = bucket.Region switch
                {
                    Region.USEast1 => "us-east-1",
                    Region.USEast2 => "us-east-2",
                    Region.USWest1 => "us-west-1",
                    Region.USWest2 => "us-west-2",
                    _ => throw new InvalidOperationException($"Region {bucket.Region} not recognized.")
                }
            };

            var response = await _client.PutBucketAsync(putBucketRequest);

            if (response == null) return ValidationResponse.Failure($"Failed to create bucket {bucket.Name}.");
        }

        return ValidationResponse.Success();
    }
}
