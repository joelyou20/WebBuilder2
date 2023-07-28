using Amazon.CostExplorer;
using Amazon.Route53;
using Amazon.S3;
using Amazon.SecretsManager;
using Microsoft.Extensions.DependencyInjection;
using Octokit;
using WebBuilder2.Server.Services;
using WebBuilder2.Server.Services.Contracts;

namespace WebBuilder2.Server.Utils.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGitHubClient(this IServiceCollection services, Func<IServiceProvider, IAwsSecretsManagerService> serviceProvider, ConfigurationManager configuration)
        {
            var awsSecretsManagerService = serviceProvider.Invoke(services.BuildServiceProvider());
            
            var pat = awsSecretsManagerService.GetSecretAsync("github-pat").Result;

            var githubSettings = configuration.GetSection("GithubSettings").Get<GithubSettings>()!;
            services.AddSingleton<IGitHubClient, GitHubClient>(sp =>
            {
                var client = new GitHubClient(new ProductHeaderValue(githubSettings.OrganizationName))
                {
                    Credentials = new Credentials(pat)
                };
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

        public static IServiceCollection AddAwsSecretsManagerClient(this IServiceCollection services)
        {
            AmazonSecretsManagerConfig awsConfig = new()
            {
                UseAlternateUserAgentHeader = AwsConfig.UseAlternateUserAgentHeader,
                RegionEndpoint = AwsConfig.RegionEndpoint
            };

            var credentials = AwsAuthenticationHelper.LoadDefaultProfile();

            return services.AddScoped(sp => new AmazonSecretsManagerClient(credentials, awsConfig));
        }

    }
}
