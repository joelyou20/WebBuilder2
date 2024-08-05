using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBuilder2.Server.Services;
using WebBuilder2.Server.Services.Contracts;

namespace WebBuilder2.Server.IntegrationTests.Services;

public class SqlServerIntegrationTest
{
    private SqlService _sqlService { get; set; } = default!;

    [SetUp]
    public void Setup()
    {
        _sqlService = new SqlService();
    }

    [Test]
    public async Task Test1()
    {
        try
        {
            // Arrange
            var serverName = "testServerName";
            var databaseName = "testDatabaseName";
            var connectionStringName = "test";

            // Act
            await _sqlService.CreateDatabaseAsync(databaseName, connectionStringName);
            await _sqlService.GetDatabaseListAsync(serverName, databaseName, connectionStringName);
            // Assert
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
        }

    }
}
