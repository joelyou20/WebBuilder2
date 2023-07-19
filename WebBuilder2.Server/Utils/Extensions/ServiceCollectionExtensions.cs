using Amazon.CostExplorer;
using Amazon.Route53;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.Util;
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

        public static IServiceCollection AddAwsS3Client(this IServiceCollection services, ConfigurationManager configuration)
        {
            AmazonS3Config awsConfig = new()
            {
                UseAlternateUserAgentHeader = AwsConfig.UseAlternateUserAgentHeader,
                RegionEndpoint = AwsConfig.RegionEndpoint
            };

            return services.AddScoped(sp => new AmazonS3Client(awsConfig));
        }

        public static IServiceCollection AddAwsRoute53Client(this IServiceCollection services, ConfigurationManager configuration)
        {
            AmazonRoute53Config awsConfig = new()
            {
                UseAlternateUserAgentHeader = AwsConfig.UseAlternateUserAgentHeader,
                RegionEndpoint = AwsConfig.RegionEndpoint
            };

            return services.AddScoped(sp => new AmazonRoute53Client(awsConfig));
        }

        public static IServiceCollection AddAwsCostExplorerClient(this IServiceCollection services, ConfigurationManager configuration)
        {
            AmazonCostExplorerConfig awsConfig = new()
            {
                UseAlternateUserAgentHeader = AwsConfig.UseAlternateUserAgentHeader,
                RegionEndpoint = AwsConfig.RegionEndpoint
            };

            //var credentials = GetAwsCredentials(configuration);

            //return services.AddScoped(sp => new AmazonCostExplorerClient(credentials, awsConfig));
            return services.AddScoped(sp => new AmazonCostExplorerClient(awsConfig));
        }

        private static BasicAWSCredentials GetAwsCredentials(ConfigurationManager configuration)
        {
            AwsSettings awsSettings = configuration.GetSection("AwsSettings").Get<AwsSettings>()!;
            return new(awsSettings.AccessKey, awsSettings.SecretKey);
        }
    }
}
