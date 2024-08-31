using WebBuilder2.Server.Services;
using Webbuilder2.Server.Tests.Utils;
using Microsoft.Extensions.Configuration;
using WebBuilder2.Shared.Models;
using FizzWare.NBuilder;

namespace Webbuilder2.Server.Tests.Services;

[TestFixture]
public class TokenServiceTest
{
    private TokenService _tokenService;
    private IConfiguration _configuration;

    [SetUp]
    public void Setup()
    {
        _configuration = ConfigurationHelper.Get();
        _tokenService = new TokenService(_configuration);
    }

    [Test]
    public void GenerateToken_Succeeds()
    {
        // Arrange
        ApplicationUser user = Builder<ApplicationUser>.CreateNew()
            .With(x => x.UserName, "test_userName")
            .Build();

        // Act
        var token = _tokenService.GenerateToken(user);

        // Assert
        Assert.That(token, Is.Not.Null);
        Assert.That(token, Has.Length.GreaterThan(1));
    }

    [Test]
    public void GenerateToken_Fails_WhenUserNameIsNull()
    {
        // Arrange
        ApplicationUser user = Builder<ApplicationUser>.CreateNew()
            .With(x => x.UserName, null!)
            .Build();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _tokenService.GenerateToken(user));
    }
}
