using FizzWare.NBuilder;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Octokit;
using Webbuilder2.Server.Tests.Utils;
using WebBuilder2.Server.Clients.Contracts;
using WebBuilder2.Server.Services;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Models.Projections;
using static Azure.Core.HttpHeader;

namespace Webbuilder2.Server.Tests.Services;

public class GithubServiceTest
{
    private Mock<IGitHubClient> _githubClientMock = default!;
    private Mock<IAwsSecretsManagerService> _awsSecretsManagerServiceMock = default!;
    private Mock<IGitHubCustomClient> _githubCustomClientMock = default!;
    private GithubService _githubService = default!;

    [SetUp]
    public void Setup()
    {
        _githubClientMock = new Mock<IGitHubClient>();
        _awsSecretsManagerServiceMock = new Mock<IAwsSecretsManagerService>();
        _githubCustomClientMock = new Mock<IGitHubCustomClient>();
        _githubService = new GithubService(_githubClientMock.Object, _githubCustomClientMock.Object, _awsSecretsManagerServiceMock.Object);
    }

    [Test]
    public async Task GetRepositoriesAsync_Succeeds()
    {
        // Arrange
        IReadOnlyList<Repository> repos = new List<Repository>()
        {
            new(1234567),
            new(7654321),
            new(1212121)
        };
        _githubClientMock.Setup(x => x.Repository.GetAllForCurrent()).ReturnsAsync(repos);

        // Act
        var result = await _githubService.GetRepositoriesAsync();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(3));
    }

    [Test]
    public void GetRepositoriesAsync_Fails_WhenGithubClientReturnsNullRepos()
    {
        // Arrange
        IReadOnlyList<Repository> repos = null!;
        _githubClientMock.Setup(x => x.Repository.GetAllForCurrent()).ReturnsAsync(repos);

        // Act & Assert
        Assert.ThrowsAsync<NullReferenceException>(async () => await _githubService.GetRepositoriesAsync());
    }

    [Test]
    public async Task CreateRepoAsync_Succeeds()
    {
        // Arrange
        var repository = Builder<Repository>.CreateNew().Build();
        var repositoryModel = Builder<RepositoryModel>.CreateNew()
            .With(x => x.RepoName = "test_repoName")
            .With(x => x.AllowAutoMerge = repository.AllowAutoMerge ?? false)
            .With(x => x.AllowMergeCommit = repository.AllowMergeCommit ?? false)
            .With(x => x.AllowRebaseMerge = repository.AllowRebaseMerge ?? false)
            .With(x => x.AllowSquashMerge = repository.AllowSquashMerge ?? false)
            .With(x => x.DeleteBranchOnMerge = repository.DeleteBranchOnMerge ?? false)
            .Build();

        _githubClientMock.Setup(x => x.Repository.Create(It.IsAny<NewRepository>())).ReturnsAsync(repository);

        // Act
        RepositoryModel response = await _githubService.CreateRepoAsync(repositoryModel);

        // Assert
        Compare.AreEqual(repository, response);
    }

    [Test]
    public void CreateRepoAsync_Fails_WhenGithubClientReturnsNullRepo()
    {
        // Arrange
        Repository nullRepository = null!;
        var repositoryModel = Builder<RepositoryModel>.CreateNew()
            .With(x => x.RepoName = "test_repoName")
            .Build();

        _githubClientMock.Setup(x => x.Repository.Create(It.IsAny<NewRepository>())).ReturnsAsync(nullRepository);

        // Act & Assert
        Assert.ThrowsAsync<NullReferenceException>(async () => await _githubService.CreateRepoAsync(repositoryModel));
    }

    [Test]
    public async Task GetGitIgnoreTemplatesAsync_Succeeds()
    {
        string template1Name = "test_template1";
        string template2Name = "test_template2";
        string template3Name = "test_template3";

        // Arrange
        IReadOnlyList<string> templates = new List<string>()
        {
            template1Name,
            template2Name,
            template3Name,
        };
        _githubClientMock.Setup(x => x.GitIgnore.GetAllGitIgnoreTemplates()).ReturnsAsync(templates);

        // Act
        GitIgnoreTemplateResponse response = await _githubService.GetGitIgnoreTemplatesAsync();

        var templateArray = response.Templates.ToArray();

        // Assert
        Assert.That(response.Templates.Count(), Is.EqualTo(templates.Count));
        Assert.Multiple(() =>
        {
            Assert.That(templateArray[0], Is.EqualTo(template1Name));
            Assert.That(templateArray[1], Is.EqualTo(template2Name));
            Assert.That(templateArray[2], Is.EqualTo(template3Name));
        });
    }

    [Test]
    public void GetGitIgnoreTemplatesAsync_Fails_WhenGithubClientReturnsNullRepo()
    {
        // Arrange
        IReadOnlyList<string> templates = null!;
        _githubClientMock.Setup(x => x.GitIgnore.GetAllGitIgnoreTemplates()).ReturnsAsync(templates);

        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(async () => await _githubService.GetGitIgnoreTemplatesAsync());
    }

    [Test]
    public async Task GetLicenseTemplatesAsync_Succeeds()
    {
        // Arrange
        IReadOnlyList<LicenseMetadata> templates = new List<LicenseMetadata>()
        {
            new("licenseKey1", "licenseNodeId1", "licenseName1", "licenseSpdxId1", "licenseUrl1", true),
            new("licenseKey2", "licenseNodeId2", "licenseName2", "licenseSpdxId2", "licenseUrl2", true),
            new("licenseKey3", "licenseNodeId3", "licenseName3", "licenseSpdxId3", "licenseUrl3", true),
        };
        _githubClientMock.Setup(x => x.Licenses.GetAllLicenses()).ReturnsAsync(templates);

        // Act
        IEnumerable<GithubProjectLicense> response = await _githubService.GetLicenseTemplatesAsync();

        var reponseArray = response.ToArray();

        // Assert
        Assert.That(response.Count, Is.EqualTo(templates.Count));
        Assert.Multiple(() =>
        {
            Compare.AreEqual(templates[0], reponseArray[0]);
            Compare.AreEqual(templates[1], reponseArray[1]);
            Compare.AreEqual(templates[2], reponseArray[2]);
        });
    }

    [Test]
    public void GetLicenseTemplatesAsync_Fails_WhenGithubClientReturnsNullRepo()
    {
        // Arrange
        IReadOnlyList<LicenseMetadata> templates = null!;
        _githubClientMock.Setup(x => x.Licenses.GetAllLicenses()).ReturnsAsync(templates);

        // Act && Assert
        Assert.ThrowsAsync<ArgumentNullException>(async () => await _githubService.GetLicenseTemplatesAsync());
    }

    [Test]
    public void CreateSecretAsync_Fails_WhenSecretValueIsNull()
    {
        // Arrange
        string userName = "test_userName";
        string repoName = "test_repoName";
        string pat = "12345";

        List<GithubSecret> secrets =
        [
            new GithubSecret
            {
                Name = "testName",
                Value = null
            }
        ];

        _awsSecretsManagerServiceMock.Setup(x => x.GetSecretAsync(It.IsAny<string>())).ReturnsAsync(pat);
        _githubCustomClientMock.Setup(x => x.CreateSecretAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<GithubSecret>()));

        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(async () => await _githubService.CreateSecretAsync(secrets, userName, repoName));
    }

    [Test]
    public void CreateSecretAsync_Fails_WhenSecretsListIsNull()
    {
        // Arrange
        string userName = "test_userName";
        string repoName = "test_repoName";
        string pat = "12345";

        List<GithubSecret> secrets = null!;

        _awsSecretsManagerServiceMock.Setup(x => x.GetSecretAsync(It.IsAny<string>())).ReturnsAsync(pat);
        _githubCustomClientMock.Setup(x => x.CreateSecretAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<GithubSecret>()));

        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(async () => await _githubService.CreateSecretAsync(secrets, userName, repoName));
    }

    [Test]
    public void CreateSecretAsync_Fails_WhenSecretsListIsEmpty()
    {
        // Arrange
        string userName = "test_userName";
        string repoName = "test_repoName";
        string pat = "12345";

        List<GithubSecret> secrets = [];

        _awsSecretsManagerServiceMock.Setup(x => x.GetSecretAsync(It.IsAny<string>())).ReturnsAsync(pat);
        _githubCustomClientMock.Setup(x => x.CreateSecretAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<GithubSecret>()));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _githubService.CreateSecretAsync(secrets, userName, repoName));
    }

    [TestCase("test_userName", null!)]
    [TestCase(null!, "test_repoName")]
    public void CreateSecretAsync_Fails_WhenInputIsNull(string userName, string repoName)
    {
        // Arrange
        string pat = "12345";

        List<GithubSecret> secrets =
        [
            new GithubSecret
            {
                Name = "testName",
                Value = "testValue"
            }
        ];

        _awsSecretsManagerServiceMock.Setup(x => x.GetSecretAsync(It.IsAny<string>())).ReturnsAsync(pat);
        _githubCustomClientMock.Setup(x => x.CreateSecretAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<GithubSecret>()));

        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(async () => await _githubService.CreateSecretAsync(secrets, userName, repoName));
    }

    [Test]
    public async Task CreateBranchAsync_Succeeds()
    {
        // Arrange
        string owner = "test_owner";
        long repoId = 12345;
        string branchName = "test_branchName";

        Reference masterReference = Builder<Reference>.CreateNew().Build();
        Commit commit = Builder<Commit>.CreateNew().Build();
        IReadOnlyList<Reference> branches = new List<Reference>()
        {
            Builder<Reference>.CreateNew().Build()
        };
        Reference branch = Builder<Reference>.CreateNew().Build();

        _githubClientMock.Setup(x => x.Git.Reference.Get(It.IsAny<long>(), It.IsAny<string>())).ReturnsAsync(masterReference);
        _githubClientMock.Setup(x => x.Git.Commit.Get(It.IsAny<long>(), It.IsAny<string>())).ReturnsAsync(commit);
        _githubClientMock.Setup(x => x.Git.Reference.GetAll(It.IsAny<long>())).ReturnsAsync(branches);
        _githubClientMock.Setup(x => x.Git.Reference.Create(It.IsAny<long>(), It.IsAny<NewReference>())).ReturnsAsync(branch);
        // Act
        Reference reference = await _githubService.CreateBranchAsync(owner, repoId, branchName, commit);

        // Assert
        Assert.That(reference, Is.Not.Null);
        Assert.That(reference, Is.EqualTo(branch));
    }

    [Test]
    public async Task CreateBranchAsync_Succeeds_IfBranchAlreadyExists()
    {
        // Arrange
        string owner = "test_owner";
        long repoId = 12345;
        string branchName = "test_branchName";

        Reference masterReference = Builder<Reference>.CreateNew().Build();
        Commit commit = Builder<Commit>.CreateNew().Build();
        Reference existingBranch = Builder<Reference>.CreateNew().Build();
        Reference branch = Builder<Reference>.CreateNew().Build();

        // Manually set the read-only field using reflection
        var nameField = typeof(Reference).GetProperty(nameof(Reference.Ref), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        nameField?.SetValue(existingBranch, $"refs/heads/{branchName}");

        IReadOnlyList<Reference> branches = new List<Reference>()
        {
            existingBranch
        };

        _githubClientMock.Setup(x => x.Git.Reference.Get(It.IsAny<long>(), It.IsAny<string>())).ReturnsAsync(masterReference);
        _githubClientMock.Setup(x => x.Git.Commit.Get(It.IsAny<long>(), It.IsAny<string>())).ReturnsAsync(commit);
        _githubClientMock.Setup(x => x.Git.Reference.GetAll(It.IsAny<long>())).ReturnsAsync(branches);
        
        // Act
        Reference reference = await _githubService.CreateBranchAsync(owner, repoId, branchName, commit);

        // Assert
        Assert.That(reference, Is.EqualTo(branches[0]));
        _githubClientMock.Verify(x => x.Git.Reference.Create(It.IsAny<long>(), It.IsAny<NewReference>()), Times.Never);
    }

    [TestCase("test_owner", null!)]
    [TestCase(null!, "test_branchName")]
    public void CreateBranchAsync_Fails_WhenInputValuesAreNull(string owner, string branchName)
    {
        // Arrange
        long repoId = 12345;

        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(async () => await _githubService.CreateBranchAsync(owner, repoId, branchName, null));
    }
}
