using FizzWare.NBuilder;
using Google;
using Google.Apis.Adsense.v2;
using Google.Apis.Adsense.v2.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Options;
using Moq;
using System.Net.Http.Json;
using Webbuilder2.Server.Tests.Fakes;
using WebBuilder2.Server.Options;
using WebBuilder2.Server.Services;
using WebBuilder2.Server.Services.Wrappers.Contracts;
using WebBuilder2.Shared.Models;

namespace Webbuilder2.Server.Tests.Services;

[TestFixture]
public class GoogleAdSenseServiceTest
{
    private GoogleAdSenseService _googleAdSenseService;
    private Mock<IAdsenseServiceWrapper> _adsenseServiceMock;

    [SetUp]
    public void Setup()
    {
        _adsenseServiceMock = new Mock<IAdsenseServiceWrapper>();
        _googleAdSenseService = new GoogleAdSenseService(_adsenseServiceMock.Object);
    }

    [Test]
    public async Task GetSingleAccountByNameAsync_Succeeds()
    {
        // Arrange
        Account response = Builder<Account>.CreateNew()
            .With(x => x.Name, "test_accountName1")
            .With(x => x.DisplayName, "test_displayName1")
            .With(x => x.State, "test_state1")
            .Build();

        _adsenseServiceMock.Setup(x => x.GetSingleAccountAsync(response.Name)).ReturnsAsync(response);

        // Act
        GoogleAdSenseAccount account = await _googleAdSenseService.GetSingleAccountByNameAsync(response.Name);

        // Assert
        Assert.That(account, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(account.Name, Is.EqualTo(response.Name));
            Assert.That(account.DisplayName, Is.EqualTo(response.DisplayName));
            Assert.That(account.State, Is.EqualTo(response.State));
        });
    }

    [Test]
    public void GetSingleAccountByNameAsync_Fails_WhenResponseIsNull()
    {
        // Arrange
        Account response = null!;

        _adsenseServiceMock.Setup(x => x.GetSingleAccountAsync(It.IsAny<string>())).ReturnsAsync(response);

        // Act & Assert
        Assert.ThrowsAsync<GoogleApiException>(async () => await _googleAdSenseService.GetSingleAccountByNameAsync("test"));
    }

    [Test]
    public async Task GetAccountsAsync_Succeeds()
    {
        // Arrange
        IList<Account> accountList = new List<Account>()
        {
            Builder<Account>.CreateNew()
                .With(x => x.Name, "test_accountName1")
                .With(x => x.DisplayName, "test_displayName1")
                .With(x => x.State, "test_state1")
                .Build(),
            Builder<Account>.CreateNew()
                .With(x => x.Name, "test_accountName2")
                .With(x => x.DisplayName, "test_displayName2")
                .With(x => x.State, "test_state2")
                .Build(),
            Builder<Account>.CreateNew()
                .With(x => x.Name, "test_accountName3")
                .With(x => x.DisplayName, "test_displayName3")
                .With(x => x.State, "test_state3")
                .Build()
        };

        ListAccountsResponse response = new()
        {
            Accounts = accountList,
            ETag = "test_etag",
            NextPageToken = "test_nextPageToken"
        };

        _adsenseServiceMock.Setup(x => x.GetAccountsAsync()).ReturnsAsync(response);

        // Act
        GoogleAdSenseAccount[] accounts = (await _googleAdSenseService.GetAccountsAsync()).ToArray();

        // Assert
        Assert.That(accounts, Is.Not.Null);
        Assert.That(accounts.Count, Is.EqualTo(3));

        Assert.Multiple(() =>
        {
            Assert.That(accounts[0].Name, Is.EqualTo(accountList[0].Name));
            Assert.That(accounts[0].DisplayName, Is.EqualTo(accountList[0].DisplayName));
            Assert.That(accounts[0].State, Is.EqualTo(accountList[0].State));
        });
        Assert.Multiple(() =>
        {
            Assert.That(accounts[1].Name, Is.EqualTo(accountList[1].Name));
            Assert.That(accounts[1].DisplayName, Is.EqualTo(accountList[1].DisplayName));
            Assert.That(accounts[1].State, Is.EqualTo(accountList[1].State));
        });
        Assert.Multiple(() =>
        {
            Assert.That(accounts[2].Name, Is.EqualTo(accountList[2].Name));
            Assert.That(accounts[2].DisplayName, Is.EqualTo(accountList[2].DisplayName));
            Assert.That(accounts[2].State, Is.EqualTo(accountList[2].State));
        });
    }

    [Test]
    public void GetAccountsAsync_Fails_WhenResponseIsNull()
    {
        // Arrange
        ListAccountsResponse response = null!;

        _adsenseServiceMock.Setup(x => x.GetAccountsAsync()).ReturnsAsync(response);

        // Act & Assert
        Assert.ThrowsAsync<GoogleApiException>(async () => await _googleAdSenseService.GetSingleAccountByNameAsync("test"));
    }

    [Test]
    public async Task GetPaymentsAsync_Succeeds()
    {
        // Arrange
        ListPaymentsResponse response = new()
        {
            Payments = new[]
            {
                Builder<Payment>.CreateNew()
                    .With(x => x.Name, "test_payment1")
                    .With(x => x.Date, new Date { Year = 2024, Month = 8, Day = 24, ETag = "test_payment_etag1" })
                    .With(x => x.Amount, "$100.00")
                    .Build(),
                Builder<Payment>.CreateNew()
                    .With(x => x.Name, "test_payment2")
                    .With(x => x.Date, new Date { Year = 2023, Month = 9, Day = 25, ETag = "test_payment_etag2" })
                    .With(x => x.Amount, "$100.00")
                    .Build(),
                Builder<Payment>.CreateNew()
                    .With(x => x.Name, "test_payment3")
                    .With(x => x.Date, new Date { Year = 2022, Month = 10, Day = 25, ETag = "test_payment_etag3" })
                    .With(x => x.Amount, "$100.00")
                    .Build()
            },
            ETag = "test_etag"
        };

        _adsenseServiceMock.Setup(x => x.GetPaymentsAsync()).ReturnsAsync(response);

        // Act
        GooglePayment[] payments = (await _googleAdSenseService.GetPaymentsAsync()).ToArray();

        // Assert
        Assert.That(payments, Is.Not.Null);
        Assert.That(payments.Count, Is.EqualTo(3));

        Assert.Multiple(() =>
        {
            Assert.That(payments[0].Name, Is.EqualTo(response.Payments[0].Name));
            Assert.That(payments[0].Date, Is.Not.Null);
            Assert.That(payments[0].Date?.Year, Is.EqualTo(response.Payments[0].Date.Year));
            Assert.That(payments[0].Date?.Month, Is.EqualTo(response.Payments[0].Date.Month));
            Assert.That(payments[0].Date?.Day, Is.EqualTo(response.Payments[0].Date.Day));
            Assert.That(payments[0].Amount, Is.EqualTo(response.Payments[0].Amount));
        });
        Assert.Multiple(() =>
        {
            Assert.That(payments[1].Name, Is.EqualTo(response.Payments[1].Name));
            Assert.That(payments[1].Date, Is.Not.Null);
            Assert.That(payments[1].Date?.Year, Is.EqualTo(response.Payments[1].Date.Year));
            Assert.That(payments[1].Date?.Month, Is.EqualTo(response.Payments[1].Date.Month));
            Assert.That(payments[1].Date?.Day, Is.EqualTo(response.Payments[1].Date.Day));
            Assert.That(payments[1].Amount, Is.EqualTo(response.Payments[1].Amount));
        });
        Assert.Multiple(() =>
        {
            Assert.That(payments[2].Name, Is.EqualTo(response.Payments[2].Name));
            Assert.That(payments[2].Date, Is.Not.Null);
            Assert.That(payments[2].Date?.Year, Is.EqualTo(response.Payments[2].Date.Year));
            Assert.That(payments[2].Date?.Month, Is.EqualTo(response.Payments[2].Date.Month));
            Assert.That(payments[2].Date?.Day, Is.EqualTo(response.Payments[2].Date.Day));
            Assert.That(payments[2].Amount, Is.EqualTo(response.Payments[2].Amount));
        });
    }

    [Test]
    public void GetPaymentsAsync_Fails_WhenResponseIsNull()
    {
        // Arrange
        ListPaymentsResponse response = null!;

        _adsenseServiceMock.Setup(x => x.GetPaymentsAsync()).ReturnsAsync(response);

        // Act & Assert
        Assert.ThrowsAsync<GoogleApiException>(async () => await _googleAdSenseService.GetPaymentsAsync());
    }

    [Test]
    public async Task GetClientsAsync_Succeeds()
    {
        // Arrange
        ListAdClientsResponse response = new()
        {
            AdClients = new[]
            {
                Builder<AdClient>.CreateNew()
                    .With(x => x.Name, "test_client1")
                    .With(x => x.ProductCode, "test_productCode1")
                    .With(x => x.State, "test_state1")
                    .Build(),
                Builder<AdClient>.CreateNew()
                    .With(x => x.Name, "test_client2")
                    .With(x => x.ProductCode, "test_productCode2")
                    .With(x => x.State, "test_state2")
                    .Build(),
                Builder<AdClient>.CreateNew()
                    .With(x => x.Name, "test_client3")
                    .With(x => x.ProductCode, "test_productCode3")
                    .With(x => x.State, "test_state3")
                    .Build()
            },
            ETag = "test_etag",
            NextPageToken = "test_nextPageToken"
        };

        _adsenseServiceMock.Setup(x => x.GetClientsAsync()).ReturnsAsync(response);

        // Act
        GoogleAdClient[] clients = (await _googleAdSenseService.GetClientsAsync()).ToArray();

        // Assert
        Assert.That(clients, Is.Not.Null);
        Assert.That(clients.Count, Is.EqualTo(3));

        Assert.Multiple(() =>
        {
            Assert.That(clients[0].Name, Is.EqualTo(response.AdClients[0].Name));
            Assert.That(clients[0].ProductCode, Is.EqualTo(response.AdClients[0].ProductCode));
            Assert.That(clients[0].State, Is.EqualTo(response.AdClients[0].State));
        });
        Assert.Multiple(() =>
        {
            Assert.That(clients[1].Name, Is.EqualTo(response.AdClients[1].Name));
            Assert.That(clients[1].ProductCode, Is.EqualTo(response.AdClients[1].ProductCode));
            Assert.That(clients[1].State, Is.EqualTo(response.AdClients[1].State));
        });
        Assert.Multiple(() =>
        {
            Assert.That(clients[2].Name, Is.EqualTo(response.AdClients[2].Name));
            Assert.That(clients[2].ProductCode, Is.EqualTo(response.AdClients[2].ProductCode));
            Assert.That(clients[2].State, Is.EqualTo(response.AdClients[2].State));
        });
    }

    [Test]
    public void GetClientsAsync_Fails_WhenResponseIsNull()
    {
        // Arrange
        ListAdClientsResponse response = null!;

        _adsenseServiceMock.Setup(x => x.GetClientsAsync()).ReturnsAsync(response);

        // Act & Assert
        Assert.ThrowsAsync<GoogleApiException>(async () => await _googleAdSenseService.GetClientsAsync());
    }
}
