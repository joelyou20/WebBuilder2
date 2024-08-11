using Amazon.S3;
using Amazon.S3.Model;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;

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

    public async Task CreateBucketAsync(AwsCreateBucketRequest request)
    {
        foreach(var bucket in request.Buckets)
        {
            // Create bucket
            var putBucketRequest = BuildPutBucketRequest(bucket);
            var putBucketResponse = await _client.PutBucketAsync(putBucketRequest);
            if (putBucketResponse == null || 
                (putBucketResponse != null && putBucketResponse.HttpStatusCode != System.Net.HttpStatusCode.OK) ||
                (putBucketResponse != null && putBucketResponse.HttpStatusCode != System.Net.HttpStatusCode.Created))
            {
                throw new AmazonS3Exception($"Failed to create bucket {bucket.Name}.");
            }


            if (bucket.ConfigureForWebsiteHosting)
            {
                // Apply website configuration to existing bucket
                var putWebsiteBucketRequest = BuildPutBucketWebsiteRequest(bucket);
                var putWebsiteBucketResponse = await _client.PutBucketWebsiteAsync(putWebsiteBucketRequest);
                if (putWebsiteBucketResponse == null ||
                    (putWebsiteBucketResponse != null && putWebsiteBucketResponse.HttpStatusCode != System.Net.HttpStatusCode.OK) ||
                    (putWebsiteBucketResponse != null && putWebsiteBucketResponse.HttpStatusCode != System.Net.HttpStatusCode.Created))
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
                    (putObjectResponse != null && putObjectResponse.HttpStatusCode != System.Net.HttpStatusCode.OK) ||
                    (putObjectResponse != null && putObjectResponse.HttpStatusCode != System.Net.HttpStatusCode.Created))
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

        if (response == null) throw new AmazonS3Exception($"Failed to enable logging for {request.Bucket.Name}.");

        return response;
    }

    public async Task<PutBucketPolicyResponse> AddBucketPolicyAsync(AwsAddBucketPolicyRequest request)
    {
        PutBucketPolicyResponse response = await _client.PutBucketPolicyAsync(new PutBucketPolicyRequest
        {
            BucketName = request.Bucket.Name,
            Policy = request.Policy,
        });

        if (response == null) throw new AmazonS3Exception($"Failed to add bucket policy for {request.Bucket.Name}.");

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

        if (response == null) throw new AmazonS3Exception($"Failed to update public access block for {request.Bucket.Name}.");

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
