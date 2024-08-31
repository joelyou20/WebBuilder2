using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Model.Internal.MarshallTransformations;
using FizzWare.NBuilder;
using Moq;
using WebBuilder2.Server.Services;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;

namespace Webbuilder2.Server.Tests.Services;

[TestFixture]
public class AwsS3ServiceTest
{
    private AwsS3Service _awsS3Service;
    private Mock<AmazonS3Client> _awsS3ClientMock;

    private Bucket _testBucket = new()
    {
        ConfigureForLogging = true,
        ConfigureForWebsiteHosting = true,
        Name = "test_bucketName",
        RedirectTarget = "test_redirectTarget",
        Region = Region.USEast1
    };

    [SetUp]
    public void Setup()
    {
        _awsS3ClientMock = new Mock<AmazonS3Client>();
        _awsS3Service = new AwsS3Service(_awsS3ClientMock.Object);
    }

    [Test]
    public async Task GetSingleBucketAsync_Succeeds()
    {
        // Arrange
        ListBucketsResponse response = new()
        {
            Buckets = [
                Builder<S3Bucket>.CreateNew()
                    .With(x => x.BucketName, _testBucket.Name)
                    .Build()
            ],
            ContentLength = 10,
            HttpStatusCode = System.Net.HttpStatusCode.OK,
            Owner = Builder<Owner>.CreateNew().Build(),
            ResponseMetadata = Builder<ResponseMetadata>.CreateNew().Build()
        };

        _awsS3ClientMock.Setup(x => x.ListBucketsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(response);

        // Act
        Bucket bucket = await _awsS3Service.GetSingleBucketAsync(_testBucket.Name);

        // Assert
        Assert.That(bucket, Is.Not.Null);
        Assert.That(bucket.Name, Is.EqualTo(response.Buckets.Single().BucketName));
    }

    [Test]
    public void GetSingleBucketAsync_Fails_WhenResponseIsNull()
    {
        // Arrange
        ListBucketsResponse response = null!;

        _awsS3ClientMock.Setup(x => x.ListBucketsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(response);

        // Act & Assert
        Assert.ThrowsAsync<AmazonS3Exception>(async () => await _awsS3Service.GetSingleBucketAsync(_testBucket.Name));
    }

    [Test]
    public void GetSingleBucketAsync_Fails_WhenResponseStatusCodeIsNot200OK()
    {
        // Arrange
        ListBucketsResponse response = new()
        {
            HttpStatusCode = System.Net.HttpStatusCode.BadRequest,
        };

        _awsS3ClientMock.Setup(x => x.ListBucketsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(response);

        // Act & Assert
        Assert.ThrowsAsync<AmazonS3Exception>(async () => await _awsS3Service.GetSingleBucketAsync(_testBucket.Name));
    }

    [Test]
    public void GetSingleBucketAsync_Fails_WhenResponseReturnsMoreThanOneBucket()
    {
        // Arrange
        ListBucketsResponse response = new()
        {
            Buckets = [.. Builder<S3Bucket>.CreateListOfSize(2).Build()],
            ContentLength = 10,
            HttpStatusCode = System.Net.HttpStatusCode.OK,
            Owner = Builder<Owner>.CreateNew().Build(),
            ResponseMetadata = Builder<ResponseMetadata>.CreateNew().Build()
        };

        _awsS3ClientMock.Setup(x => x.ListBucketsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(response);

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _awsS3Service.GetSingleBucketAsync(_testBucket.Name));
    }

    [Test]
    public async Task GetBucketsAsync_Succeeds()
    {
        // Arrange
        ListBucketsResponse response = new()
        {
            Buckets = Builder<S3Bucket>.CreateListOfSize(3).Build().ToList(),
            ContentLength = 10,
            HttpStatusCode = System.Net.HttpStatusCode.OK,
            Owner = Builder<Owner>.CreateNew().Build(),
            ResponseMetadata = Builder<ResponseMetadata>.CreateNew().Build()
        };

        _awsS3ClientMock.Setup(x => x.ListBucketsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(response);

        // Act
        var buckets = (await _awsS3Service.GetBucketsAsync()).ToArray();

        // Assert
        Assert.That(buckets, Is.Not.Empty);
        Assert.That(buckets.Count, Is.EqualTo(response.Buckets.Count));

        Assert.Multiple(() =>
        {
            Assert.That(buckets[0].Name, Is.EqualTo(response.Buckets[0].BucketName));
            Assert.That(buckets[1].Name, Is.EqualTo(response.Buckets[1].BucketName));
            Assert.That(buckets[2].Name, Is.EqualTo(response.Buckets[2].BucketName));
        });
    }

    [Test]
    public async Task CreateBucketAsync_Succeeds()
    {
        // Arrange
        int numOfBuckets = 3;

        AwsCreateBucketRequest request = new()
        {
            Buckets = Builder<Bucket>.CreateListOfSize(numOfBuckets)
                .All()
                .With(x => x.ConfigureForWebsiteHosting, true)
                .With(x => x.ConfigureForLogging, true)
                .Build()
                .ToList()
        };

        PutBucketResponse putBucketResponse = new()
        {
            ContentLength = 10,
            HttpStatusCode = System.Net.HttpStatusCode.OK,
            ResponseMetadata = Builder<ResponseMetadata>.CreateNew().Build()
        };

        PutBucketWebsiteResponse putBucketWebsiteResponse = new()
        {
            ContentLength = 10,
            HttpStatusCode = System.Net.HttpStatusCode.OK,
            ResponseMetadata = Builder<ResponseMetadata>.CreateNew().Build()
        };

        PutObjectResponse putObjectResponse = Builder<PutObjectResponse>.CreateNew()
            .With(x => x.HttpStatusCode, System.Net.HttpStatusCode.OK)
            .Build();

        _awsS3ClientMock.Setup(x => x.PutBucketAsync(It.IsAny<PutBucketRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(putBucketResponse);
        _awsS3ClientMock.Setup(x => x.PutBucketWebsiteAsync(It.IsAny<PutBucketWebsiteRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(putBucketWebsiteResponse);
        _awsS3ClientMock.Setup(x => x.PutObjectAsync(It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(putObjectResponse);

        // Act
        await _awsS3Service.CreateBucketAsync(request);

        // Assert
        _awsS3ClientMock.Verify(x => x.PutBucketAsync(It.IsAny<PutBucketRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(numOfBuckets));
        _awsS3ClientMock.Verify(x => x.PutBucketWebsiteAsync(It.IsAny<PutBucketWebsiteRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(numOfBuckets));
        _awsS3ClientMock.Verify(x => x.PutObjectAsync(It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(numOfBuckets));
    }

    [Test]
    public void CreateBucketAsync_Fails_WhenPutBucketResponseIsNull()
    {
        // Arrange
        int numOfBuckets = 3;

        AwsCreateBucketRequest request = new()
        {
            Buckets = Builder<Bucket>.CreateListOfSize(numOfBuckets)
                .All()
                .With(x => x.ConfigureForWebsiteHosting, true)
                .With(x => x.ConfigureForLogging, true)
                .Build()
                .ToList()
        };

        PutBucketResponse putBucketResponse = null!;

        _awsS3ClientMock.Setup(x => x.PutBucketAsync(It.IsAny<PutBucketRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(putBucketResponse);

        // Act & Assert
        Assert.ThrowsAsync<AmazonS3Exception>(async () => await _awsS3Service.CreateBucketAsync(request));
    }

    [Test]
    public void CreateBucketAsync_Fails_WhenPutBucketWebsiteResponseIsNull()
    {
        // Arrange
        int numOfBuckets = 3;

        AwsCreateBucketRequest request = new()
        {
            Buckets = Builder<Bucket>.CreateListOfSize(numOfBuckets)
                .All()
                .With(x => x.ConfigureForWebsiteHosting, true)
                .With(x => x.ConfigureForLogging, true)
                .Build()
                .ToList()
        };

        PutBucketResponse putBucketResponse = new()
        {
            ContentLength = 10,
            HttpStatusCode = System.Net.HttpStatusCode.OK,
            ResponseMetadata = Builder<ResponseMetadata>.CreateNew().Build()
        };

        PutBucketWebsiteResponse putBucketWebsiteResponse = null!;

        _awsS3ClientMock.Setup(x => x.PutBucketAsync(It.IsAny<PutBucketRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(putBucketResponse);
        _awsS3ClientMock.Setup(x => x.PutBucketWebsiteAsync(It.IsAny<PutBucketWebsiteRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(putBucketWebsiteResponse);

        // Act & Assert
        Assert.ThrowsAsync<AmazonS3Exception>(async () => await _awsS3Service.CreateBucketAsync(request));
    }

    [Test]
    public void CreateBucketAsync_Fails_WhenPutObjectResponseIsNull()
    {
        // Arrange
        int numOfBuckets = 3;

        AwsCreateBucketRequest request = new()
        {
            Buckets = Builder<Bucket>.CreateListOfSize(numOfBuckets)
                .All()
                .With(x => x.ConfigureForWebsiteHosting, true)
                .With(x => x.ConfigureForLogging, true)
                .Build()
                .ToList()
        };

        PutBucketResponse putBucketResponse = new()
        {
            ContentLength = 10,
            HttpStatusCode = System.Net.HttpStatusCode.OK,
            ResponseMetadata = Builder<ResponseMetadata>.CreateNew().Build()
        };


        PutBucketWebsiteResponse putBucketWebsiteResponse = new()
        {
            ContentLength = 10,
            HttpStatusCode = System.Net.HttpStatusCode.OK,
            ResponseMetadata = Builder<ResponseMetadata>.CreateNew().Build()
        };

        PutObjectResponse putObjectResponse = null!;

        _awsS3ClientMock.Setup(x => x.PutBucketAsync(It.IsAny<PutBucketRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(putBucketResponse);
        _awsS3ClientMock.Setup(x => x.PutBucketWebsiteAsync(It.IsAny<PutBucketWebsiteRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(putBucketWebsiteResponse);
        _awsS3ClientMock.Setup(x => x.PutObjectAsync(It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(putObjectResponse);

        // Act & Assert
        Assert.ThrowsAsync<AmazonS3Exception>(async () => await _awsS3Service.CreateBucketAsync(request));
    }

    [Test]
    public void CreateBucketAsync_Fails_WhenBucketWebsiteStatusCodeIsNot200OK()
    {
        // Arrange
        int numOfBuckets = 3;

        AwsCreateBucketRequest request = new()
        {
            Buckets = Builder<Bucket>.CreateListOfSize(numOfBuckets)
                .All()
                .With(x => x.ConfigureForWebsiteHosting, true)
                .With(x => x.ConfigureForLogging, false)
                .Build()
                .ToList()
        };

        PutBucketResponse putBucketResponse = new()
        {
            ContentLength = 10,
            HttpStatusCode = System.Net.HttpStatusCode.OK,
            ResponseMetadata = Builder<ResponseMetadata>.CreateNew().Build()
        };

        PutBucketWebsiteResponse putBucketWebsiteResponse = new()
        {
            HttpStatusCode = System.Net.HttpStatusCode.BadRequest,
        };

        _awsS3ClientMock.Setup(x => x.PutBucketAsync(It.IsAny<PutBucketRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(putBucketResponse);

        // Act & Assert
        Assert.ThrowsAsync<AmazonS3Exception>(async () => await _awsS3Service.CreateBucketAsync(request));
    }

    [Test]
    public async Task ConfigureLoggingAsync_Succeeds()
    {
        // Arrange
        AwsConfigureLoggingRequest request = new()
        {
            Bucket = _testBucket,
            LogBucket = new Bucket
            {
                ConfigureForLogging = true,
                ConfigureForWebsiteHosting = true,
                Name = "test_logBucketName",
                RedirectTarget = "test_redirectTarget",
                Region = Region.USEast1
            },
            LogObjectKeyPrefix = "test_objectKeyPrefix"
        };

        PutBucketLoggingResponse response = new()
        {
            ContentLength = 10,
            HttpStatusCode = System.Net.HttpStatusCode.OK,
            ResponseMetadata = Builder<ResponseMetadata>.CreateNew().Build()
        };

        _awsS3ClientMock.Setup(x => x.PutBucketLoggingAsync(It.IsAny<PutBucketLoggingRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        // Act
        PutBucketLoggingResponse result = await _awsS3Service.ConfigureLoggingAsync(request);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(response));
    }

    [Test]
    public void ConfigureLoggingAsync_Fails_WhenResponseStatusCodeIsNot200OK()
    {
        // Arrange
        AwsConfigureLoggingRequest request = new()
        {
            Bucket = _testBucket,
            LogBucket = new Bucket
            {
                ConfigureForLogging = true,
                ConfigureForWebsiteHosting = true,
                Name = "test_logBucketName",
                RedirectTarget = "test_redirectTarget",
                Region = Region.USEast1
            },
            LogObjectKeyPrefix = "test_objectKeyPrefix"
        };

        PutBucketLoggingResponse response = new()
        {
            HttpStatusCode = System.Net.HttpStatusCode.BadRequest
        };

        _awsS3ClientMock.Setup(x => x.PutBucketLoggingAsync(It.IsAny<PutBucketLoggingRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        // Act & Assert
        Assert.ThrowsAsync<AmazonS3Exception>(async () => await _awsS3Service.ConfigureLoggingAsync(request));
    }

    [Test]
    public void ConfigureLoggingAsync_Fails_WhenResponseIsNull()
    {
        // Arrange
        AwsConfigureLoggingRequest request = new()
        {
            Bucket = _testBucket,
            LogBucket = new Bucket
            {
                ConfigureForLogging = true,
                ConfigureForWebsiteHosting = true,
                Name = "test_logBucketName",
                RedirectTarget = "test_redirectTarget",
                Region = Region.USEast1
            },
            LogObjectKeyPrefix = "test_objectKeyPrefix"
        };

        PutBucketLoggingResponse response = null!;

        _awsS3ClientMock.Setup(x => x.PutBucketLoggingAsync(It.IsAny<PutBucketLoggingRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        // Act & Assert
        Assert.ThrowsAsync<AmazonS3Exception>(async () => await _awsS3Service.ConfigureLoggingAsync(request));
    }

    [Test]
    public async Task AddBucketPolicyAsync_Succeeds()
    {
        // Arrange
        AwsAddBucketPolicyRequest request = new()
        {
            Bucket = _testBucket,
            Policy = "test_Policy"
        };

        PutBucketPolicyResponse putBucketPolicyResponse = new()
        {
            ContentLength = 10,
            HttpStatusCode = System.Net.HttpStatusCode.OK,
            ResponseMetadata = Builder<ResponseMetadata>.CreateNew().Build()
        };

        _awsS3ClientMock.Setup(x => x.PutBucketPolicyAsync(It.IsAny<PutBucketPolicyRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(putBucketPolicyResponse);

        // Act
        PutBucketPolicyResponse response = await _awsS3Service.AddBucketPolicyAsync(request);

        // Assert
        Assert.That(response, Is.Not.Null);
    }

    [Test]
    public void AddBucketPolicyAsync_Fails_WhenResponseStatusCodeIsNot200OK()
    {
        // Arrange
        AwsAddBucketPolicyRequest request = new()
        {
            Bucket = _testBucket,
            Policy = "test_Policy"
        };

        PutBucketPolicyResponse putBucketPolicyResponse = new()
        {
            ContentLength = 10,
            HttpStatusCode = System.Net.HttpStatusCode.BadRequest,
            ResponseMetadata = Builder<ResponseMetadata>.CreateNew().Build()
        };

        _awsS3ClientMock.Setup(x => x.PutBucketPolicyAsync(It.IsAny<PutBucketPolicyRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(putBucketPolicyResponse);

        // Act & Assert
        Assert.ThrowsAsync<AmazonS3Exception>(async () => await _awsS3Service.AddBucketPolicyAsync(request));
    }

    [Test]
    public void AddBucketPolicyAsync_Fails_WhenResponseIsNull()
    {
        // Arrange
        AwsAddBucketPolicyRequest request = new()
        {
            Bucket = _testBucket,
            Policy = "test_Policy"
        };

        PutBucketPolicyResponse putBucketPolicyResponse = null!;

        _awsS3ClientMock.Setup(x => x.PutBucketPolicyAsync(It.IsAny<PutBucketPolicyRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(putBucketPolicyResponse);

        // Act & Assert
        Assert.ThrowsAsync<AmazonS3Exception>(async () => await _awsS3Service.AddBucketPolicyAsync(request));
    }

    [Test]
    public async Task ConfigurePublicAccessBlockAsync_Succeeds()
    {
        // Arrange
        AwsPublicAccessBlockRequest request = new()
        {
            Bucket = _testBucket,
            BlockPublicAcls = true
        };

        PutPublicAccessBlockResponse putPublicAccessBlockResponse = new()
        {
            ContentLength = 10,
            HttpStatusCode = System.Net.HttpStatusCode.OK,
            ResponseMetadata = Builder<ResponseMetadata>.CreateNew().Build()
        };

        _awsS3ClientMock.Setup(x => x.PutPublicAccessBlockAsync(It.IsAny<PutPublicAccessBlockRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(putPublicAccessBlockResponse);

        // Act
        var response = await _awsS3Service.ConfigurePublicAccessBlockAsync(request);

        // Assert
        Assert.That(response, Is.Not.Null);
    }

    [Test]
    public void ConfigurePublicAccessBlockAsync_Fails_WhenResponseStatusCodeIsNot200OK()
    {
        // Arrange
        AwsPublicAccessBlockRequest request = new()
        {
            Bucket = _testBucket,
            BlockPublicAcls = true
        };

        PutPublicAccessBlockResponse putPublicAccessBlockResponse = new()
        {
            ContentLength = 10,
            HttpStatusCode = System.Net.HttpStatusCode.BadRequest,
            ResponseMetadata = Builder<ResponseMetadata>.CreateNew().Build()
        };

        _awsS3ClientMock.Setup(x => x.PutPublicAccessBlockAsync(It.IsAny<PutPublicAccessBlockRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(putPublicAccessBlockResponse);

        // Act & Assert
        Assert.ThrowsAsync<AmazonS3Exception>(async () => await _awsS3Service.ConfigurePublicAccessBlockAsync(request));
    }

    [Test]
    public void ConfigurePublicAccessBlockAsync_Fails_WhenResponseIsNull()
    {
        // Arrange
        AwsPublicAccessBlockRequest request = new()
        {
            Bucket = _testBucket,
            BlockPublicAcls = true
        };

        PutPublicAccessBlockResponse putPublicAccessBlockResponse = null!;

        _awsS3ClientMock.Setup(x => x.PutPublicAccessBlockAsync(It.IsAny<PutPublicAccessBlockRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(putPublicAccessBlockResponse);

        // Act & Assert
        Assert.ThrowsAsync<AmazonS3Exception>(async () => await _awsS3Service.ConfigurePublicAccessBlockAsync(request));
    }
}
