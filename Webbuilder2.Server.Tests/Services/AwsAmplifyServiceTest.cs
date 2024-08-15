using Amazon.Amplify;
using Amazon.Amplify.Model;
using FizzWare.NBuilder;
using Microsoft.Extensions.Configuration;
using Moq;
using WebBuilder2.Server.Services;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Server.Settings;
using WebBuilder2.Shared.Models.Dtos;

namespace Webbuilder2.Server.Tests.Services;

public class AwsAmplifyServiceTest
{
    private AwsAmplifyService _awsAmplifyService;
    private Mock<AmazonAmplifyClient> _awsAmplifyClientMock;
    private Mock<IAwsSecretsManagerService> _awsSecretsManagerServiceMock;

    private string _testAccessToken = "test_accessToken";

    [SetUp]
    public void Setup()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { AwsAmplifySettingsStore.AccessToken, _testAccessToken }
            })
            .Build();
        _awsAmplifyClientMock = new Mock<AmazonAmplifyClient>();
        _awsSecretsManagerServiceMock = new Mock<IAwsSecretsManagerService>();
        _awsAmplifyService = new AwsAmplifyService(_awsAmplifyClientMock.Object, _awsSecretsManagerServiceMock.Object, configuration);
    }

    [Test]
    public async Task CreateAppFromRepoAsync_Succeeds()
    {
        // Arrange
        SiteRepositoryModel siteRepository = Builder<SiteRepositoryModel>.CreateNew()
            .With(x => x.Site, Builder<SiteModel>.CreateNew()
                .With(x => x.Name, "test_siteName")
                .Build())
            .Build();

        RepositoryModel repositoryModel = Builder<RepositoryModel>.CreateNew()
            .With(x => x.SiteRepository, siteRepository)
            .With(x => x.HtmlUrl, "test_htmlUrl")
            .Build();

        CreateAppResponse createAppResponse = Builder<CreateAppResponse>.CreateNew().Build();
        
        CreateAppRequest createAppRequest = new()
        {
            Name = repositoryModel.SiteRepository?.Site?.Name,
            AccessToken = _testAccessToken,
            Repository = repositoryModel.HtmlUrl
        };

        string token = "test_token";

        _awsSecretsManagerServiceMock.Setup(x => x.GetSecretAsync(It.IsAny<string>())).ReturnsAsync(token);
        _awsAmplifyClientMock.Setup(x => x.CreateAppAsync(It.IsAny<CreateAppRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(createAppResponse);

        // Act
        var response = await _awsAmplifyService.CreateAppFromRepoAsync(repositoryModel);

        // Assert
        _awsAmplifyClientMock.Verify(x => x.CreateAppAsync(
            It.Is<CreateAppRequest>(x => 
                x.AccessToken == createAppRequest.AccessToken &&
                x.Name == createAppRequest.Name &&
                x.Repository == createAppRequest.Repository), default), 
            Times.Once);
        Assert.That(response, Is.Not.Null);
        Assert.That(response, Is.EqualTo(createAppResponse));
    }

    [TestCase("")]
    [TestCase(null!)]
    public void CreateAppFromRepoAsync_Fails_WhenTokenIsInvalid(string token)
    {
        // Arrange
        _awsSecretsManagerServiceMock.Setup(x => x.GetSecretAsync(It.IsAny<string>())).ReturnsAsync(token);

        // Act & Assert
        Assert.ThrowsAsync<AmazonAmplifyException>(async () => await _awsAmplifyService.CreateAppFromRepoAsync(new RepositoryModel()));
    }
}
