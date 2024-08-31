using Amazon.Route53;
using Amazon.Route53.Model;
using Amazon.Route53Domains;
using Amazon.Runtime;
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
public class AwsRoute53ServiceTest
{
    private AwsRoute53Service _awsRoute53Service;
    private Mock<AmazonRoute53Client> _awsRoute53ClientMock;

    [SetUp]
    public void Setup()
    {
        _awsRoute53ClientMock = new Mock<AmazonRoute53Client>();
        _awsRoute53Service = new AwsRoute53Service(_awsRoute53ClientMock.Object);
    }

    [Test]
    public async Task GetHostedZonesAsync_Succeeds()
    {
        // Arrange
        ListHostedZonesResponse response = new()
        {
            ContentLength = 10,
            HostedZones = [.. Builder<HostedZone>.CreateListOfSize(3).Build()],
            HttpStatusCode = System.Net.HttpStatusCode.OK,
            IsTruncated = false,
            Marker = "test_marker",
            MaxItems = "test_maxItems",
            NextMarker = "test_nextMarker",
            ResponseMetadata = Builder<ResponseMetadata>.CreateNew().Build()
        };

        _awsRoute53ClientMock.Setup(x => x.ListHostedZonesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(response);

        // Act
        var hostedZones = (await _awsRoute53Service.GetHostedZonesAsync()).ToArray();

        // Assert
        Assert.That(hostedZones, Is.Not.Empty);
        Assert.That(hostedZones, Is.Not.Null);
        Assert.That(hostedZones.Count, Is.EqualTo(response.HostedZones.Count));

        Assert.Multiple(() =>
        {
            Assert.That(hostedZones[0].Id, Is.EqualTo(response.HostedZones[0].Id));
            Assert.That(hostedZones[0].Name, Is.EqualTo(response.HostedZones[0].Name));

            Assert.That(hostedZones[1].Id, Is.EqualTo(response.HostedZones[1].Id));
            Assert.That(hostedZones[1].Name, Is.EqualTo(response.HostedZones[1].Name));

            Assert.That(hostedZones[2].Id, Is.EqualTo(response.HostedZones[2].Id));
            Assert.That(hostedZones[2].Name, Is.EqualTo(response.HostedZones[2].Name));
        });
    }

    [Test]
    public void GetHostedZonesAsync_Fails_WhenResponseStatusCodeIsNot200OK()
    {
        // Arrange
        ListHostedZonesResponse response = new()
        {
            HttpStatusCode = System.Net.HttpStatusCode.NoContent
        };

        _awsRoute53ClientMock.Setup(x => x.ListHostedZonesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(response);

        // Act & Assert
        Assert.ThrowsAsync<AmazonRoute53Exception>(async () => await _awsRoute53Service.GetHostedZonesAsync());
    }

    [Test]
    public void GetHostedZonesAsync_Fails_WhenResponseIsNull()
    {
        // Arrange
        ListHostedZonesResponse response = null!;

        _awsRoute53ClientMock.Setup(x => x.ListHostedZonesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(response);

        // Act & Assert
        Assert.ThrowsAsync<AmazonRoute53Exception>(async () => await _awsRoute53Service.GetHostedZonesAsync());
    }
}
