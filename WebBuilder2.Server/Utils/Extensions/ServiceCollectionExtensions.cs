using Amazon.CostExplorer;
using Amazon.Route53;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.Util;
using Microsoft.Extensions.DependencyInjection;
using Octokit;
using WebBuilder2.Server.Services;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Server.Settings;

namespace WebBuilder2.Server.Utils.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGitHubClient(this IServiceCollection services, ConfigurationManager configuration)
        {
            var githubSettings = configuration.GetSection("GithubSettings").Get<GithubSettings>()!;
            services.AddSingleton(sp =>
            {
                var client = new GitHubClient(new ProductHeaderValue(githubSettings.OrganizationName));

                return client;
            });

            return services;
        }

        public static IServiceCollection AddAwsS3Client(this IServiceCollection services)
        {
            AmazonS3Config awsConfig = new()
            {
                UseAlternateUserAgentHeader = AwsConfig.UseAlternateUserAgentHeader,
                RegionEndpoint = AwsConfig.RegionEndpoint
            };

            var credentials = AwsAuthenticationHelper.LoadDefaultProfile();

            return services.AddSingleton(sp => new AmazonS3Client(credentials, awsConfig));
        }

        public static IServiceCollection AddAwsRoute53Client(this IServiceCollection services)
        {
            AmazonRoute53Config awsConfig = new()
            {
                UseAlternateUserAgentHeader = AwsConfig.UseAlternateUserAgentHeader,
                RegionEndpoint = AwsConfig.RegionEndpoint
            };

            var credentials = AwsAuthenticationHelper.LoadDefaultProfile();

            return services.AddSingleton(sp => new AmazonRoute53Client(credentials, awsConfig));
        }

        public static IServiceCollection AddAwsCostExplorerClient(this IServiceCollection services)
        {
            AmazonCostExplorerConfig awsConfig = new()
            {
                UseAlternateUserAgentHeader = AwsConfig.UseAlternateUserAgentHeader,
                RegionEndpoint = AwsConfig.RegionEndpoint
            };

            var credentials = AwsAuthenticationHelper.LoadDefaultProfile();

            return services.AddScoped(sp => new AmazonCostExplorerClient(credentials, awsConfig));
        }
    }
}
