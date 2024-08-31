using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using FizzWare.NBuilder;
using Moq;
using Newtonsoft.Json;
using WebBuilder2.Server.Services;

namespace Webbuilder2.Server.Tests.Services;

[TestFixture]
public class AwsSecretsManagerServiceTest
{
    private AwsSecretsManagerService _awsSecretsManagerService;
    private Mock<AmazonSecretsManagerClient> _awsSecretsManagerClient;

    [SetUp]
    public void Setup()
    {
        _awsSecretsManagerClient = new Mock<AmazonSecretsManagerClient>();
        _awsSecretsManagerService = new AwsSecretsManagerService(_awsSecretsManagerClient.Object);
    }

    [Test]
    public async Task GetSecretAsync_Succeeds()
    {
        // Arrange
        var secretkey = "SecretKey";
        var secretValue = "SecretValue";
        object secretObj = new
        {
            SecretKey = secretValue
        };

        string secretJson = JsonConvert.SerializeObject(secretObj);

        GetSecretValueResponse response = Builder<GetSecretValueResponse>.CreateNew()
            .With(x => x.SecretString, secretJson)
            .With(x => x.HttpStatusCode, System.Net.HttpStatusCode.OK)
            .Build();

        _awsSecretsManagerClient.Setup(x => x.GetSecretValueAsync(It.IsAny<GetSecretValueRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        // Act
        var secret = await _awsSecretsManagerService.GetSecretAsync(secretkey);

        // Assert
        Assert.That(secret, Is.Not.Null);
        Assert.That(secret, Is.EqualTo(secretValue));
    }

    [Test]
    public void GetSecretAsync_Fails_WhenStatusCodeIsNot200OK()
    {
        // Arrange
        var secretKey = "SecretKey";
        var secretValue = "SecretValue";
        object secretObj = new
        {
            SecretKey = secretValue
        };

        string secretJson = JsonConvert.SerializeObject(secretObj);

        GetSecretValueResponse response = Builder<GetSecretValueResponse>.CreateNew()
            .With(x => x.SecretString, secretJson)
            .With(x => x.HttpStatusCode, System.Net.HttpStatusCode.BadRequest)
            .Build();

        _awsSecretsManagerClient.Setup(x => x.GetSecretValueAsync(It.IsAny<GetSecretValueRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        // Act & Assert
        Assert.ThrowsAsync<AmazonSecretsManagerException>(async () => await _awsSecretsManagerService.GetSecretAsync(secretKey));
    }

    [Test]
    public void GetSecretAsync_Fails_WhenResponseIsNull()
    {
        // Arrange
        var secretKey = "SecretKey";
        var secretValue = "SecretValue";
        object secretObj = new
        {
            SecretKey = secretValue
        };

        string secretJson = JsonConvert.SerializeObject(secretObj);

        GetSecretValueResponse response = null!;

        _awsSecretsManagerClient.Setup(x => x.GetSecretValueAsync(It.IsAny<GetSecretValueRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        // Act & Assert
        Assert.ThrowsAsync<AmazonSecretsManagerException>(async () => await _awsSecretsManagerService.GetSecretAsync(secretKey));
    }

    [Test]
    public void GetSecretAsync_Fails_WhenCannotParseSecret()
    {
        // Arrange
        var secretKey = "SecretKey";
        var secretValue = "NOT JSON";

        GetSecretValueResponse response = Builder<GetSecretValueResponse>.CreateNew()
            .With(x => x.SecretString, secretValue)
            .With(x => x.HttpStatusCode, System.Net.HttpStatusCode.OK)
            .Build();

        _awsSecretsManagerClient.Setup(x => x.GetSecretValueAsync(It.IsAny<GetSecretValueRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        // Act & Assert
        Assert.ThrowsAsync<JsonReaderException>(async () => await _awsSecretsManagerService.GetSecretAsync(secretKey));
    }

    [Test]
    public void GetSecretAsync_Fails_WhenResponseIsSuccessful_ButKeyDoesNotExist()
    {
        // Arrange
        var secretKey = "SecretKey";
        var secretValue = "SecretValue";
        object secretObj = new
        {
            WRONGKEY = secretValue
        };

        string secretJson = JsonConvert.SerializeObject(secretObj);

        GetSecretValueResponse response = Builder<GetSecretValueResponse>.CreateNew()
            .With(x => x.SecretString, secretJson)
            .With(x => x.HttpStatusCode, System.Net.HttpStatusCode.OK)
            .Build();

        _awsSecretsManagerClient.Setup(x => x.GetSecretValueAsync(It.IsAny<GetSecretValueRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _awsSecretsManagerService.GetSecretAsync(secretKey));
    }
}
