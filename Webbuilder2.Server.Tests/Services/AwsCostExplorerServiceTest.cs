using Moq;
using WebBuilder2.Server.Services;
using Amazon.CostExplorer;
using Amazon.CostExplorer.Model;
using FizzWare.NBuilder;
using Amazon.Runtime;

namespace Webbuilder2.Server.Tests.Services;

[TestFixture]
public class AwsCostExplorerServiceTest
{
    private AwsCostExplorerService _awsCostExplorerService;
    private Mock<AmazonCostExplorerClient> _awsCostExplorerClientMock;

    [SetUp]
    public void Setup()
    {
        _awsCostExplorerClientMock = new Mock<AmazonCostExplorerClient>();
        _awsCostExplorerService = new AwsCostExplorerService(_awsCostExplorerClientMock.Object);
    }

    [Test]
    public async Task GetForecastedCostAsync_Succeeds()
    {
        // Arrange
        GetCostForecastResponse response = new()
        {
            ForecastResultsByTime = [.. Builder<ForecastResult>.CreateListOfSize(3).Build()],
            ContentLength = 10,
            HttpStatusCode = System.Net.HttpStatusCode.OK,
            ResponseMetadata = Builder<ResponseMetadata>.CreateNew().Build(),
            Total = new MetricValue
            {
                Amount = "test_amount",
                Unit = "test_unit"
            }
        };

        _awsCostExplorerClientMock.Setup(x => x.GetCostForecastAsync(It.IsAny<GetCostForecastRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        // Act
        var result = await _awsCostExplorerService.GetForecastedCostAsync();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(response.Total.Amount));
    }

    [Test]
    public void GetForecastedCostAsync_Fails_WhenResponseCodeIsNot200OK()
    {
        // Arrange
        GetCostForecastResponse response = new()
        {
            HttpStatusCode = System.Net.HttpStatusCode.NoContent,
        };

        _awsCostExplorerClientMock.Setup(x => x.GetCostForecastAsync(It.IsAny<GetCostForecastRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        // Act && Assert
        Assert.ThrowsAsync<AmazonCostExplorerException>(async () => await _awsCostExplorerService.GetForecastedCostAsync());
    }

    [Test]
    public void GetForecastedCostAsync_Fails_WhenResponseIsNull()
    {
        // Arrange
        GetCostForecastResponse response = null!;

        _awsCostExplorerClientMock.Setup(x => x.GetCostForecastAsync(It.IsAny<GetCostForecastRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        // Act && Assert
        Assert.ThrowsAsync<AmazonCostExplorerException>(async () => await _awsCostExplorerService.GetForecastedCostAsync());
    }
}
