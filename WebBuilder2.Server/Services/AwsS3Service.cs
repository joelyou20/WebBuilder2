using Amazon.CostExplorer;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Azure;
using System.Net;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Server.Utils;
using WebBuilder2.Server.Utils.Extensions;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;

namespace WebBuilder2.Server.Services;

public class AwsS3Service(AmazonS3Client client) : IAwsS3Service
{
    private readonly AmazonS3Client _client = client;
    private readonly string _indexDocumentSuffix = "index.html";
    private readonly string _errorDocument = "error.html";

    public async Task<Bucket> GetSingleBucketAsync(string name)
    {
        ListBucketsResponse response = await _client.ListBucketsAsync();

        AmazonServiceResponseValidator<AmazonS3Exception>.Validate(response, $"Failed to get bucket with name: {name}");

        var bucket = response.Buckets.Single(bucket => bucket.BucketName.Equals(name));

        return new Bucket
        {
            Name = bucket.BucketName,
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

    public async Task CreateBucketAsync(AwsCreateBucketRequest request)
    {
        foreach(var bucket in request.Buckets)
        {
            // Create bucket
            var putBucketRequest = BuildPutBucketRequest(bucket);
            var putBucketResponse = await _client.PutBucketAsync(putBucketRequest);
            if (putBucketResponse == null || 
                (putBucketResponse != null && 
                putBucketResponse.HttpStatusCode != HttpStatusCode.OK &&
                putBucketResponse.HttpStatusCode != HttpStatusCode.Created))
            {
                throw new AmazonS3Exception($"Failed to create bucket {bucket.Name}.");
            }

            if (bucket.ConfigureForWebsiteHosting)
            {
                // Apply website configuration to existing bucket
                var putWebsiteBucketRequest = BuildPutBucketWebsiteRequest(bucket);
                var putWebsiteBucketResponse = await _client.PutBucketWebsiteAsync(putWebsiteBucketRequest);
                if (putWebsiteBucketResponse == null ||
                    (putWebsiteBucketResponse != null && 
                    putWebsiteBucketResponse.HttpStatusCode != HttpStatusCode.OK &&
                    putWebsiteBucketResponse.HttpStatusCode != HttpStatusCode.Created))
                {
                    throw new AmazonS3Exception($"Failed to create website bucket {bucket.Name}.");
                }
            }

            if(bucket.ConfigureForLogging)
            {
                // Apply logging configuration to existing bucket
                var putObjectRequest = BuildPutObjectRequest(bucket, "logs/");
                var putObjectResponse = await _client.PutObjectAsync(putObjectRequest);
                if (putObjectResponse == null ||
                    (putObjectResponse != null && 
                    putObjectResponse.HttpStatusCode != HttpStatusCode.OK &&
                    putObjectResponse.HttpStatusCode != HttpStatusCode.Created))
                {
                    throw new AmazonS3Exception($"Failed to create logging bucket {bucket.Name}.");
                }
            }
        }
    }

    public async Task<PutBucketLoggingResponse> ConfigureLoggingAsync(AwsConfigureLoggingRequest request)
    {
        var loggingConfig = new S3BucketLoggingConfig
        {
            TargetBucketName = request.LogBucket.Name,
            TargetPrefix = request.LogObjectKeyPrefix,
        };

        var putBucketLoggingRequest = new PutBucketLoggingRequest
        {
            BucketName = request.Bucket.Name,
            LoggingConfig = loggingConfig,
        };

        PutBucketLoggingResponse response = await _client.PutBucketLoggingAsync(putBucketLoggingRequest);

        AmazonServiceResponseValidator<AmazonS3Exception>.Validate(response, "Failed to create logging bucket.");

        return response;
    }

    public async Task<PutBucketPolicyResponse> AddBucketPolicyAsync(AwsAddBucketPolicyRequest request)
    {
        PutBucketPolicyResponse response = await _client.PutBucketPolicyAsync(new PutBucketPolicyRequest
        {
            BucketName = request.Bucket.Name,
            Policy = request.Policy,
        });

        AmazonServiceResponseValidator<AmazonS3Exception>.Validate(response, "Failed to add bucket policy.");

        return response;
    }

    public async Task<PutPublicAccessBlockResponse> ConfigurePublicAccessBlockAsync(AwsPublicAccessBlockRequest request)
    {
        PutPublicAccessBlockResponse response = await _client.PutPublicAccessBlockAsync(new PutPublicAccessBlockRequest
        {
            BucketName = request.Bucket.Name,
            PublicAccessBlockConfiguration = new PublicAccessBlockConfiguration
            {
                BlockPublicAcls = request.BlockPublicAcls
            }
        });

        AmazonServiceResponseValidator<AmazonS3Exception>.Validate(response, "Failed to configure public access block.");

        return response;
    }

    private PutObjectRequest BuildPutObjectRequest(Bucket bucket, string path) => new()
    {
        BucketName = bucket.Name,
        StorageClass = S3StorageClass.Standard,
        Key = path,
        ContentBody = string.Empty
    };

    private PutBucketRequest BuildPutBucketRequest(Bucket bucket) => new()
    {
        
        BucketName = bucket.Name,
        BucketRegion = bucket.Region switch
        {
            Region.USEast1 => S3Region.USEast1,
            Region.USEast2 => S3Region.USEast2,
            Region.USWest1 => S3Region.USWest1,
            Region.USWest2 => S3Region.USWest2,
            _ => throw new InvalidOperationException($"Region {bucket.Region} not recognized.")
        }
    };

    private PutBucketWebsiteRequest BuildPutBucketWebsiteRequest(Bucket bucket)
    {
        WebsiteConfiguration config = bucket.RedirectTarget == null ?
            new()
            {
                IndexDocumentSuffix = _indexDocumentSuffix,
                ErrorDocument = _errorDocument,
            } :
            new()
            {
                RedirectAllRequestsTo = new RoutingRuleRedirect
                {
                    HostName = bucket.RedirectTarget,
                    Protocol = nameof(Protocol.HTTP).ToLower()
                }
            };

        var request = new PutBucketWebsiteRequest
        {
            BucketName = bucket.Name,
            WebsiteConfiguration = config
        };

        return request;
    }
}
