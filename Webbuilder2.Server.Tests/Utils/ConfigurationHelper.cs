using Microsoft.Extensions.Configuration;

namespace Webbuilder2.Server.Tests.Utils;

public static class ConfigurationHelper
{
    public static IConfiguration Get() => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Ensure the directory is correct
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true) // Optional: include environment-specific settings
            .AddEnvironmentVariables() // Optional: add environment variables if needed
            .Build();
}
