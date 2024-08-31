using Amazon.Route53Domains;
using FizzWare.NBuilder;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webbuilder2.Server.Tests.Fakes;
using WebBuilder2.Server.Services;
using WebBuilder2.Shared.Models;
using WebBuilder2.Shared.Models.Projections;

namespace Webbuilder2.Server.Tests.Services;

[TestFixture]
public class UserServiceTest
{
    private UserService _userService;
    private Mock<FakeUserManager> _userManagerMock;
    private Mock<FakeSignInManager> _signInManagerMock;

    [SetUp]
    public void Setup()
    {
        _userManagerMock = new Mock<FakeUserManager>();
        _signInManagerMock = new Mock<FakeSignInManager>();
        _userService = new UserService(_userManagerMock.Object, _signInManagerMock.Object);
    }

    [Test]
    public async Task LoginUserAsync_Succeeds()
    {
        // Arrange
        LoginUserRequest request = new()
        {
            Password = "test_password",
            RememberMe = true,
            Username = "test_userName"
        };

        // Act
        var result = await _userService.LoginUserAsync(request);

        // Assert
        _signInManagerMock.Verify(x => x.PasswordSignInAsync(request.Username, request.Password, request.RememberMe, It.IsAny<bool>()));
    }

    [TestCase("", "test_password")]
    [TestCase(null!, "test_password")]
    [TestCase("test_userName", "")]
    [TestCase("test_userName", null!)]
    public void LoginUserAsync_Fails_WhenRequestDataIsInvalid(string username, string password)
    {
        // Arrange
        LoginUserRequest request = new()
        {
            Password = string.Empty,
            RememberMe = true,
            Username = username
        };

        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(async () => await _userService.LoginUserAsync(request));
    }

    [Test]
    public async Task RegisterUserAsync_Succeeds()
    {
        // Arrange
        RegisterUserRequest request = new()
        {
            Email = "test_email",
            PasswordHash = "test_passwordHash"
        };

        IdentityResult identityResult = Builder<IdentityResult>.CreateNew()
            .With(x => x.Succeeded, true)
            .Build();

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(identityResult);
        _signInManagerMock.Setup(x => x.SignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<bool>(), It.IsAny<string?>()));

        // Act
        IdentityResult result = await _userService.RegisterUserAsync(request);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Succeeded, Is.True);
    }

    [Test]
    public void RegisterUserAsync_Fails_WhenSignInIsUnsuccessful()
    {
        // Arrange
        RegisterUserRequest request = new()
        {
            Email = "test_email",
            PasswordHash = "test_passwordHash"
        };

        IdentityResult identityResult = Builder<IdentityResult>.CreateNew()
            .With(x => x.Succeeded, false)
            .Build();

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(identityResult);

        // Act
        Assert.ThrowsAsync<Exception>(async () => await _userService.RegisterUserAsync(request));
    }
}
