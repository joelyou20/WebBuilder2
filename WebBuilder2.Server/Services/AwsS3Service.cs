using Amazon.S3;
using Amazon.S3.Model;
using System.Net.Sockets;
using WebBuilder2.Server.Repositories;
using WebBuilder2.Server.Repositories.Contracts;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services;

public class AwsS3Service : IAwsS3Service
{
    private AmazonS3Client _client;
    private const string _indexDocumentSuffix = "index.html";
    private const string _errorDocument = "error.html";

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

    public async Task<ValidationResponse> CreateBucketAsync(AwsCreateBucketRequest request)
    {
        foreach(var bucket in request.Buckets)
        {
            // Create bucket
            var putBucketRequest = BuildPutBucketRequest(bucket);
            var putBucketResponse = await _client.PutBucketAsync(putBucketRequest);
            if (putBucketResponse == null) return ValidationResponse.Failure($"Failed to create bucket {bucket.Name}.");

            if (bucket.ConfigureForWebsiteHosting)
            {
                // Apply website configuration to existing bucket
                var putWebsiteBucketRequest = BuildPutBucketWebsiteRequest(bucket);
                var putWebsiteBucketResponse = await _client.PutBucketWebsiteAsync(putWebsiteBucketRequest);
                if (putWebsiteBucketResponse == null) return ValidationResponse.Failure($"Failed to create website bucket {bucket.Name}.");
            }

            if(bucket.ConfigureForLogging)
            {
                // Apply website configuration to existing bucket
                var putObjectRequest = BuildPutObjectRequest(bucket, "logs/");
                var putObjectResponse = await _client.PutObjectAsync(putObjectRequest);
                if (putObjectResponse == null) return ValidationResponse.Failure($"Failed to create website bucket {bucket.Name}.");
            }
        }

        return ValidationResponse.Success();
    }

    public async Task<ValidationResponse> ConfigureLoggingAsync(AwsConfigureLoggingRequest request)
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
        var response = await _client.PutBucketLoggingAsync(putBucketLoggingRequest);

        if (response == null) return ValidationResponse.Failure($"Failed to enable logging for {request.Bucket.Name}.");

        return ValidationResponse.Success();
    }

    public async Task<ValidationResponse> AddBucketPolicyAsync(AwsAddBucketPolicyRequest request)
    {
        PutBucketPolicyResponse response = await _client.PutBucketPolicyAsync(new PutBucketPolicyRequest
        {
            BucketName = request.Bucket.Name,
            Policy = request.Policy,
        });

        if (response == null) return ValidationResponse.Failure($"Failed to add bucket policy for {request.Bucket.Name}.");

        return ValidationResponse.Success();
    }

    public async Task<ValidationResponse> ConfigurePublicAccessBlockAsync(AwsPublicAccessBlockRequest request)
    {
        PutPublicAccessBlockResponse response = await _client.PutPublicAccessBlockAsync(new PutPublicAccessBlockRequest
        {
            BucketName = request.Bucket.Name,
            PublicAccessBlockConfiguration = new PublicAccessBlockConfiguration
            {
                BlockPublicAcls = request.BlockPublicAcls
            }
        });

        if (response == null) return ValidationResponse.Failure($"Failed to update public access block for {request.Bucket.Name}.");

        return ValidationResponse.Success();
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
