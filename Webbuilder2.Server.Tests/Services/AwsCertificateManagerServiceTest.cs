using Moq;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Server.Services;
using Amazon.CertificateManager;
using WebBuilder2.Shared.Models.Projections;
using FizzWare.NBuilder;
using Amazon.CertificateManager.Model;
using Amazon.Runtime;

namespace Webbuilder2.Server.Tests.Services;

[TestFixture]
public class AwsCertificateManagerServiceTest
{
    private AwsCertificateManagerService _awsCertificateManagerService;
    private Mock<AmazonCertificateManagerClient> _amazonCertificateManagerClientMock;
    private Mock<IAwsRoute53Service> _awsRoute53ServiceMock;

    [SetUp]
    public void Setup()
    {
        _amazonCertificateManagerClientMock = new Mock<AmazonCertificateManagerClient>();
        _awsRoute53ServiceMock = new Mock<IAwsRoute53Service>();
        _awsCertificateManagerService = new(_amazonCertificateManagerClientMock.Object, _awsRoute53ServiceMock.Object);
    }

    [Test]
    public async Task ProvisionNewCertificateAsync_Succeeds()
    {
        // Arrange
        string domainName1 = "test_domainName1";
        string domainName2 = "test_domainName2";
        string domainName3 = "test_domainName3";

        AwsNewSSLCertificateRequest request = new()
        {
            AlternativeNames =
            [
                domainName1,
                domainName2,
                domainName3,
            ],
            DomainName = domainName1
        };
        IEnumerable<WebBuilder2.Shared.Models.HostedZone> hostedZones = [
            Builder<WebBuilder2.Shared.Models.HostedZone>.CreateNew()
                .With(x => x.Name, $"{domainName1}.")
                .With(x => x.Id, "test_hostedZoneId1")
                .Build(),
            Builder<WebBuilder2.Shared.Models.HostedZone>.CreateNew()
                .With(x => x.Name, $"{domainName2}.")
                .With(x => x.Id, "test_hostedZoneId2")
            .Build(),
            Builder<WebBuilder2.Shared.Models.HostedZone>.CreateNew()
                .With(x => x.Name, $"{domainName3}.")
                .With(x => x.Id, "test_hostedZoneId3")
                .Build(),
        ];
        RequestCertificateResponse requestCertificateResponse = new()
        {
            CertificateArn = "arn:aws:acm:Region:444455556666:certificate/certificate_ID",
            ContentLength = 10,
            HttpStatusCode = System.Net.HttpStatusCode.OK,
            ResponseMetadata = Builder<ResponseMetadata>.CreateNew().Build()
        };

        _awsRoute53ServiceMock.Setup(x => x.GetHostedZonesAsync()).ReturnsAsync(hostedZones);
        _amazonCertificateManagerClientMock.Setup(x => x.RequestCertificateAsync(It.IsAny<RequestCertificateRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(requestCertificateResponse);

        // Act
        var response = await _awsCertificateManagerService.ProvisionNewCertificateAsync(request);

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Arn, Is.EqualTo(requestCertificateResponse.CertificateArn));
    }

    [Test]
    public async Task ProvisionNewCertificateAsync_ReturnsNull_WhenValidationResultFails()
    {
        // Arrange
        AwsNewSSLCertificateRequest request = new()
        {
            AlternativeNames =
            [
                "DOESN'T MATTER"
            ],
            DomainName = "DOESN'T MATTER"
        };
        IEnumerable<WebBuilder2.Shared.Models.HostedZone> hostedZones = [
            Builder<WebBuilder2.Shared.Models.HostedZone>.CreateNew()
                .With(x => x.Name, "DOESN'T MATTER")
                .With(x => x.Id, "DOESN'T MATTER")
                .Build(),
        ];

        _awsRoute53ServiceMock.Setup(x => x.GetHostedZonesAsync()).ReturnsAsync(hostedZones);

        // Act
        var response = await _awsCertificateManagerService.ProvisionNewCertificateAsync(request);

        // Assert
        Assert.That(response, Is.Null);
    }

    [Test]
    public void ProvisionNewCertificateAsync_Fails_WhenResponseStatusCodeNot200OK()
    {
        // Arrange
        string domainName1 = "test_domainName1";
        string domainName2 = "test_domainName2";
        string domainName3 = "test_domainName3";

        AwsNewSSLCertificateRequest request = new()
        {
            AlternativeNames =
            [
                domainName1,
                domainName2,
                domainName3,
            ],
            DomainName = domainName1
        };
        IEnumerable<WebBuilder2.Shared.Models.HostedZone> hostedZones = [
            Builder<WebBuilder2.Shared.Models.HostedZone>.CreateNew()
                .With(x => x.Name, $"{domainName1}.")
                .With(x => x.Id, "test_hostedZoneId1")
                .Build(),
            Builder<WebBuilder2.Shared.Models.HostedZone>.CreateNew()
                .With(x => x.Name, $"{domainName2}.")
                .With(x => x.Id, "test_hostedZoneId2")
            .Build(),
            Builder<WebBuilder2.Shared.Models.HostedZone>.CreateNew()
                .With(x => x.Name, $"{domainName3}.")
                .With(x => x.Id, "test_hostedZoneId3")
                .Build(),
        ];
        RequestCertificateResponse requestCertificateResponse = new()
        {
            ContentLength = 10,
            HttpStatusCode = System.Net.HttpStatusCode.Forbidden,
        };

        _awsRoute53ServiceMock.Setup(x => x.GetHostedZonesAsync()).ReturnsAsync(hostedZones);
        _amazonCertificateManagerClientMock.Setup(x => x.RequestCertificateAsync(It.IsAny<RequestCertificateRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(requestCertificateResponse);

        // Act & Assert
       Assert.ThrowsAsync<AmazonCertificateManagerException>(async () => await _awsCertificateManagerService.ProvisionNewCertificateAsync(request));
    }
}
