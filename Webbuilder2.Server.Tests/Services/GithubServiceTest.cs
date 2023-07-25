using Moq;
using NUnit.Framework;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBuilder2.Server.Services;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Shared.Validation;

namespace Webbuilder2.Server.Tests.Services;

public class GithubServiceTest
{
    private Mock<IGitHubClient> _githubClientMock;
    private GithubService _githubService;

    [SetUp]
    public void Setup()
    {
        _githubClientMock = new Mock<IGitHubClient>();
        _githubService = new GithubService(_githubClientMock.Object);
    }

    [TearDown] 
    public void Teardown()
    {

    }

    [Test]
    public void GetRepositoriesAsync_ShouldReturnValidResponse()
    {
        // Arrange
        //_githubServiceMock.Setup(x => x.)

        // Act

        // Assert
    }
}
