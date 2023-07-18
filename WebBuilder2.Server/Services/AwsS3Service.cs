using Amazon.S3;
using Amazon.S3.Model;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Shared.Models;

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
        var buckets = new List<Bucket>();

        ListBucketsResponse bucketResponse = await _client.ListBucketsAsync();
        foreach(S3Bucket bucket in bucketResponse.Buckets)
        {
            buckets.Add(new Bucket
            {
                Name = bucket.BucketName
            });
        }

        return buckets;
    }

}
