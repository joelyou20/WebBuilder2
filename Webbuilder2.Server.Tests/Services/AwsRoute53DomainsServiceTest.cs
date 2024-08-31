using Amazon.CostExplorer;
using Amazon.Route53;
using Amazon.Route53Domains;
using Amazon.Route53Domains.Model;
using Amazon.Runtime;
using Azure;
using FizzWare.NBuilder;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBuilder2.Server.Services;

namespace Webbuilder2.Server.Tests.Services;

[TestFixture]
public class AwsRoute53DomainsServiceTest
{
    private AwsRoute53DomainsService _awsRoute53DomainsService;
    private Mock<AmazonRoute53DomainsClient> _awsRoute53ClientMock;

    private readonly string _domain = "test_domain";

    [SetUp]
    public void Setup()
    {
        _awsRoute53ClientMock = new Mock<AmazonRoute53DomainsClient>();
        _awsRoute53DomainsService = new AwsRoute53DomainsService(_awsRoute53ClientMock.Object);
    }

    [Test]
    public async Task CheckDomainAvailabilityAsync_Succeeds()
    {
        // Arrange
        CheckDomainAvailabilityResponse response = new()
        {
            Availability = DomainAvailability.AVAILABLE,
            HttpStatusCode = System.Net.HttpStatusCode.OK,
            ContentLength = 10,
            ResponseMetadata = Builder<ResponseMetadata>.CreateNew().Build()
        };

        _awsRoute53ClientMock.Setup(x => x.CheckDomainAvailabilityAsync(It.IsAny<CheckDomainAvailabilityRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        // Act
        var availability = await _awsRoute53DomainsService.CheckDomainAvailabilityAsync(_domain);

        // Assert
        Assert.That(availability, Is.Not.Null);
        Assert.That(availability, Is.EqualTo(response.Availability.Value));
    }

    [Test]
    public void CheckDomainAvailabilityAsync_Fails_WhenResponseIsNull()
    {
        // Arrange
        CheckDomainAvailabilityResponse response = new()
        {
            HttpStatusCode = System.Net.HttpStatusCode.BadRequest,
        };

        _awsRoute53ClientMock.Setup(x => x.CheckDomainAvailabilityAsync(It.IsAny<CheckDomainAvailabilityRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        // Act & Assert
        Assert.ThrowsAsync<AmazonRoute53DomainsException>(async () => await _awsRoute53DomainsService.CheckDomainAvailabilityAsync(_domain));
    }

    [Test]
    public void CheckDomainAvailabilityAsync_Fails_WhenResponseStatusCodeIsNot200OK()
    {
        // Arrange
        CheckDomainAvailabilityResponse response = null!;

        _awsRoute53ClientMock.Setup(x => x.CheckDomainAvailabilityAsync(It.IsAny<CheckDomainAvailabilityRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        // Act & Assert
        Assert.ThrowsAsync<AmazonRoute53DomainsException>(async () => await _awsRoute53DomainsService.CheckDomainAvailabilityAsync(_domain));
    }
}
