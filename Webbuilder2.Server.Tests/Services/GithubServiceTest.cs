using Moq;
using Octokit;
using WebBuilder2.Server.Services;
using WebBuilder2.Server.Services.Contracts;

namespace Webbuilder2.Server.Tests.Services;

public class GithubServiceTest
{
    private Mock<IGitHubClient> _githubClientMock;
    private Mock<IAwsSecretsManagerService> _awsSecretsManagerServiceMock;
    private GithubService _githubService;

    [SetUp]
    public void Setup()
    {
        _githubClientMock = new Mock<IGitHubClient>();
        _awsSecretsManagerServiceMock = new Mock<IAwsSecretsManagerService>();
        _githubService = new GithubService(_githubClientMock.Object, _awsSecretsManagerServiceMock.Object);
    }

    [TearDown] 
    public void Teardown()
    {

    }

    [Test]
    public async Task GetRepositoriesAsync_Succeeds()
    {
        IReadOnlyList<Repository> repos = new List<Repository>()
        {
            new(1234567),
            new(7654321),
            new(1212121)
        };

        // Arrange
        _githubClientMock.Setup(x => x.Repository.GetAllForCurrent()).ReturnsAsync(repos);

        // Act
        var result = await _githubService.GetRepositoriesAsync();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(3));
    }

    [Test]
    public void GetRepositoriesAsync_Fails_WhenGithubClientReturnsNull()
    {
        IReadOnlyList<Repository> repos = null!;

        // Arrange
        _githubClientMock.Setup(x => x.Repository.GetAllForCurrent()).ReturnsAsync(repos);

        // Act
        Assert.ThrowsAsync<NullReferenceException>(async () => await _githubService.GetRepositoriesAsync());

        // Assert
    }

    [Test]
    public async Task CreateRepoAsync_Succeeds()
    {

    }
}
