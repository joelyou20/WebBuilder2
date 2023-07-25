using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Shared.Validation;

namespace Webbuilder2.Server.Tests.Services;

public class GithubServiceTest
{
    private Mock<IGithubService> _githubServiceMock;

    [SetUp]
    public void Setup()
    {
        _githubServiceMock = new Mock<IGithubService>();
    }

    [TearDown] 
    public void Teardown()
    {

    }

    [Test]
    public void GetRepositoriesAsync_ShouldReturnValidResponse()
    {

    }
}
