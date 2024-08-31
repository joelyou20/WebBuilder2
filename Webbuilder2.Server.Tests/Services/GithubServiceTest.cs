using Amazon.S3.Model;
using FizzWare.NBuilder;
using Moq;
using Octokit;
using System.Text;
using Webbuilder2.Server.Tests.Utils;
using WebBuilder2.Server.Clients.Contracts;
using WebBuilder2.Server.Services;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Models.Projections;

namespace Webbuilder2.Server.Tests.Services;

[TestFixture]
public class GithubServiceTest
{
    private Mock<IGitHubClient> _githubClientMock = default!;
    private Mock<IAwsSecretsManagerService> _awsSecretsManagerServiceMock = default!;
    private Mock<IGitHubCustomClient> _githubCustomClientMock = default!;
    private GithubService _githubService = default!;

    private string _owner = "test_owner";
    private string _repoName = "test_repoName";
    private string _userName = "test_userName";
    private string _githubPAT = "test_pat";
    private string _branchName = "test_branchName";
    private long _repoId = 12345;

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
            .With(x => x.RepoName = _repoName)
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
            .With(x => x.RepoName = _repoName)
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
        List<GithubSecret> secrets =
        [
            new GithubSecret
            {
                Name = "testName",
                Value = null
            }
        ];

        _awsSecretsManagerServiceMock.Setup(x => x.GetSecretAsync(It.IsAny<string>())).ReturnsAsync(_githubPAT);
        _githubCustomClientMock.Setup(x => x.CreateSecretAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<GithubSecret>()));

        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(async () => await _githubService.CreateSecretAsync(secrets, _userName, _repoName));
    }

    [Test]
    public void CreateSecretAsync_Fails_WhenSecretsListIsNull()
    {
        // Arrange
        List<GithubSecret> secrets = null!;

        _awsSecretsManagerServiceMock.Setup(x => x.GetSecretAsync(It.IsAny<string>())).ReturnsAsync(_githubPAT);
        _githubCustomClientMock.Setup(x => x.CreateSecretAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<GithubSecret>()));

        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(async () => await _githubService.CreateSecretAsync(secrets, _userName, _repoName));
    }

    [Test]
    public void CreateSecretAsync_Fails_WhenSecretsListIsEmpty()
    {
        // Arrange
        List<GithubSecret> secrets = [];

        _awsSecretsManagerServiceMock.Setup(x => x.GetSecretAsync(It.IsAny<string>())).ReturnsAsync(_githubPAT);
        _githubCustomClientMock.Setup(x => x.CreateSecretAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<GithubSecret>()));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _githubService.CreateSecretAsync(secrets, _userName, _repoName));
    }

    [TestCase("test_userName", null!)]
    [TestCase(null!, "test_repoName")]
    public void CreateSecretAsync_Fails_WhenInputIsNull(string userName, string repoName)
    {
        // Arrange
        List<GithubSecret> secrets =
        [
            new GithubSecret
            {
                Name = "testName",
                Value = "testValue"
            }
        ];

        _awsSecretsManagerServiceMock.Setup(x => x.GetSecretAsync(It.IsAny<string>())).ReturnsAsync(_githubPAT);
        _githubCustomClientMock.Setup(x => x.CreateSecretAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<GithubSecret>()));

        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(async () => await _githubService.CreateSecretAsync(secrets, userName, repoName));
    }

    [Test]
    public async Task CreateBranchAsync_Succeeds()
    {
        // Arrange
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
        Reference reference = await _githubService.CreateBranchAsync(_owner, _repoId, _branchName, commit);

        // Assert
        Assert.That(reference, Is.Not.Null);
        Assert.That(reference, Is.EqualTo(branch));
    }

    [Test]
    public async Task CreateBranchAsync_Succeeds_IfBranchAlreadyExists()
    {
        // Arrange
        Reference masterReference = Builder<Reference>.CreateNew().Build();
        Commit commit = Builder<Commit>.CreateNew().Build();
        Reference existingBranch = Builder<Reference>.CreateNew().Build();
        Reference branch = Builder<Reference>.CreateNew().Build();

        // Manually set the read-only field using reflection
        var nameField = typeof(Reference).GetProperty(nameof(Reference.Ref), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        nameField?.SetValue(existingBranch, $"refs/heads/{_branchName}");

        IReadOnlyList<Reference> branches = new List<Reference>()
        {
            existingBranch
        };

        _githubClientMock.Setup(x => x.Git.Reference.Get(It.IsAny<long>(), It.IsAny<string>())).ReturnsAsync(masterReference);
        _githubClientMock.Setup(x => x.Git.Commit.Get(It.IsAny<long>(), It.IsAny<string>())).ReturnsAsync(commit);
        _githubClientMock.Setup(x => x.Git.Reference.GetAll(It.IsAny<long>())).ReturnsAsync(branches);
        
        // Act
        Reference reference = await _githubService.CreateBranchAsync(_owner, _repoId, _branchName, commit);

        // Assert
        Assert.That(reference, Is.EqualTo(branches[0]));
        _githubClientMock.Verify(x => x.Git.Reference.Create(It.IsAny<long>(), It.IsAny<NewReference>()), Times.Never);
    }

    [TestCase("test_owner", null!)]
    [TestCase(null!, "test_branchName")]
    public void CreateBranchAsync_Fails_WhenInputValuesAreNull(string owner, string branchName)
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(async () => await _githubService.CreateBranchAsync(owner, _repoId, branchName, null));
    }

    [Test]
    public async Task CreateCommitAsync_Succeeds()
    {
        // Arrange
        GithubCreateCommitRequest request = new GithubCreateCommitRequest
        {
            Branch = _branchName,
            Message = "test_message",
            Files =
            [
                Builder<NewFile>.CreateNew().Build(),
                Builder<NewFile>.CreateNew().Build(),
                Builder<NewFile>.CreateNew().Build()
            ]
        };
        _githubClientMock.Setup(x => x.Repository.Content.CreateFile(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<CreateFileRequest>()));

        // Act
        await _githubService.CreateCommitAsync(_owner, _repoId, request);

        // Assert
        _githubClientMock.Verify(x => x.Repository.Content.CreateFile(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<CreateFileRequest>()), Times.Exactly(request.Files.Count));
    }

    [Test]
    public async Task GetRepositoryContentAsync_Succeeds()
    {
        // Arrange
        string path = "test_path";
        
        string content = "text_content";
        byte[] contentBytes = Encoding.UTF8.GetBytes(content);
        string contentBase64 = Convert.ToBase64String(contentBytes);

        _githubClientMock.Setup(x => x.Repository.Content.GetRawContent(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(contentBytes);

        // Act
        IEnumerable<RepoContent> response = await _githubService.GetRepositoryContentAsync(_owner, _repoName, path);

        var responseArray = response.ToArray();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(responseArray[0].Content, Is.EqualTo(content));
            Assert.That(responseArray[0].Name, Is.EqualTo(path)); // Name is being set to path.
            Assert.That(responseArray[0].FileType.ToString().ToLower(), Is.EqualTo(ContentType.File.ToString().ToLower()));
            Assert.That(responseArray[0].Path, Is.EqualTo(path));
        });
    }

    [Test]
    public async Task GetRepositoryContentAsync_Succeeds_IfPathIsNull()
    {
        // Arrange
        string path = null!;

        string content = "text_content";
        byte[] contentBytes = Encoding.UTF8.GetBytes(content);
        string contentBase64 = Convert.ToBase64String(contentBytes);
        IReadOnlyList<RepositoryContent> repoContentList = new List<RepositoryContent>
        {
            new(name: "test_name",
                path: path,
                sha: "test_sha",
                size: 10,
                type: ContentType.File,
                downloadUrl: "test_downloadUrl",
                url: "test_url",
                gitUrl: "test_gitUrl",
                htmlUrl: "test_htmlUrl",
                encoding: "test_encoding",
                encodedContent: contentBase64,
                target: "test_target",
                submoduleGitUrl: "test_submoduleGitUrl")
        };

        _githubClientMock.Setup(x => x.Repository.Content.GetAllContents(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(repoContentList);

        // Act
        IEnumerable<RepoContent> response = await _githubService.GetRepositoryContentAsync(_owner, _repoName, path);

        var responseArray = response.ToArray();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(responseArray[0].Content, Is.EqualTo(content));
            Assert.That(responseArray[0].Name, Is.EqualTo(repoContentList[0].Name));
            Assert.That(responseArray[0].FileType.ToString().ToLower(), Is.EqualTo(repoContentList[0].Type.ToString().ToLower()));
            Assert.That(responseArray[0].Path, Is.EqualTo(repoContentList[0].Path));
        });
    }

    [Test]
    public void GetRepositoryContentAsync_Fails_WhenPathIsNull_And_RepoContentListIsNull()
    {
        // Arrange
        string path = null!;
        IReadOnlyList<RepositoryContent> repoContentList = null!;

        _githubClientMock.Setup(x => x.Repository.Content.GetAllContents(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(repoContentList);

        // Act & Assert
        Assert.ThrowsAsync<NullReferenceException>(async () => await _githubService.GetRepositoryContentAsync(_owner, _repoName, path));
    }

    [Test]
    public void GetRepositoryContentAsync_Fails_WhenContentBytesIsNull()
    {
        // Arrange
        string path = "test_path";
        byte[] contentBytes = null!;

        _githubClientMock.Setup(x => x.Repository.Content.GetRawContent(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(contentBytes);

        // Act
        Assert.ThrowsAsync<ArgumentNullException>(async () => await _githubService.GetRepositoryContentAsync(_owner, _repoName, path));
    }

    [Test]
    public async Task GetGitTreeAsync_Succeeds()
    {
        // Arrange
        TreeResponse treeResponse = new(
            sha: "test_sha",
            url: "test_url",
            tree: new List<TreeItem>
            {
                Builder<TreeItem>.CreateNew()
                    .With(x => x.Type, TreeType.Tree)
                    .Build(),
                Builder<TreeItem>.CreateNew()
                    .With(x => x.Type, TreeType.Blob)
                    .Build(),
                Builder<TreeItem>.CreateNew()
                    .With(x => x.Type, TreeType.Blob)
                    .Build(),
            },
            truncated: true);
        _githubClientMock.Setup(x => x.Git.Tree.GetRecursive(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(treeResponse);

        // Act
        var gitTree = await _githubService.GetGitTreeAsync(_owner, _repoName);
        var gitTreeArray = gitTree.ToArray();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(gitTreeArray.Count(), Is.EqualTo(1));

            Assert.That(gitTreeArray[0].Sha, Is.EqualTo(treeResponse.Tree[0].Sha));
            Assert.That(gitTreeArray[0].Mode, Is.EqualTo(treeResponse.Tree[0].Mode));
            Assert.That(gitTreeArray[0].Path, Is.EqualTo(treeResponse.Tree[0].Path));
            Assert.That(gitTreeArray[0].Items, Is.Not.Null);
        });
    }

    [Test]
    [Ignore("Gotta figure out how to mock the ScriptRunner.")]
    public async Task CopyRepoAsync_Succeeds()
    {
        // Arrange
        string clonedRepoName = "test_clonedRepoName";
        string newRepoName = "test_newRepoName";
        string path = "test_path";
        User user = Builder<User>.CreateNew().Build();

        _githubClientMock.Setup(x => x.User.Current()).ReturnsAsync(user);

        // Act
        var isSuccessful = await _githubService.CopyRepoAsync(clonedRepoName, newRepoName, path);

        // Assert
    }
}
