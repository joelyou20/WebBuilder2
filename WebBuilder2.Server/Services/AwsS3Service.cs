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

    public async Task<Site> GetSingleSiteAsync(int id)
    {
        ListBucketsResponse bucketResponse = await _client.ListBucketsAsync(); 

        var bucket = bucketResponse.Buckets.Single(bucket => bucket.BucketName.GetHashCode() == id);

        return new Site
        {
            Name = bucket.BucketName,
            CreationDate = bucket.CreationDate
        };
    }

    public async Task<IEnumerable<Site>> GetSitesAsync()
    {
        var sites = new List<Site>();

        ListBucketsResponse bucketResponse = await _client.ListBucketsAsync();
        foreach(S3Bucket bucket in bucketResponse.Buckets)
        {
            sites.Add(new Site
            {
                Name = bucket.BucketName,
                CreationDate = bucket.CreationDate
            });
        }

        return sites;
    }

}
