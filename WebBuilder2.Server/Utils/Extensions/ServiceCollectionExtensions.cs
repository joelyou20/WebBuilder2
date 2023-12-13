using Amazon.Amplify;
using Amazon.CostExplorer;
using Amazon.Route53;
using Amazon.Route53Domains;
using Amazon.S3;
using Amazon.SecretsManager;
using Amazon.CertificateManager;
using Google.Apis.Adsense.v2;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Http;
using Google.Apis.Services;
using Octokit;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Server.Settings;

namespace WebBuilder2.Server.Utils.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGitHubClient(this IServiceCollection services, Func<IServiceProvider, IAwsSecretsManagerService> serviceProvider, ConfigurationManager configuration)
        {
            var awsSecretsManagerService = serviceProvider.Invoke(services.BuildServiceProvider());
            
            var pat = awsSecretsManagerService.GetSecretAsync(AwsSecret.GithubPat).Result;

            var githubSettings = configuration.GetSection(nameof(GithubSettings)).Get<GithubSettings>()!;
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

        public static IServiceCollection AddAwsRoute53DomainsClient(this IServiceCollection services)
        {
            AmazonRoute53DomainsConfig awsConfig = new()
            {
                UseAlternateUserAgentHeader = AwsConfig.UseAlternateUserAgentHeader,
                RegionEndpoint = AwsConfig.RegionEndpoint
            };

            var credentials = AwsAuthenticationHelper.LoadDefaultProfile();

            return services.AddSingleton(sp => new AmazonRoute53DomainsClient(credentials, awsConfig));
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

        public static IServiceCollection AddAwsAmplifyClient(this IServiceCollection services)
        {
            AmazonAmplifyConfig awsConfig = new()
            {
                UseAlternateUserAgentHeader = AwsConfig.UseAlternateUserAgentHeader,
                RegionEndpoint = AwsConfig.RegionEndpoint
            };

            var credentials = AwsAuthenticationHelper.LoadDefaultProfile();

            return services.AddScoped(sp => new AmazonAmplifyClient(credentials, awsConfig));
        }

        public static IServiceCollection AddAdSenseService(this IServiceCollection services, Func<IServiceProvider, IAwsSecretsManagerService> serviceProvider, ConfigurationManager configuration)
        {
            var awsSecretsManagerService = serviceProvider.Invoke(services.BuildServiceProvider());

            var clientSecret = awsSecretsManagerService.GetSecretAsync(AwsSecret.GoogleClientSecret).Result;

            var googleSettings = configuration.GetSection(nameof(GoogleSettings)).Get<GoogleSettings>()!;

            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    new ClientSecrets
                    {
                        ClientId = googleSettings.ClientId,
                        ClientSecret = clientSecret,
                    },
                    new string[] { AdsenseService.Scope.Adsense },
                    "WebBuilder2",
                    CancellationToken.None).Result;

            return services.AddScoped(sp => new AdsenseService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            }));
        }

        public static IServiceCollection AddAwsCertificateManagerClient(this IServiceCollection services)
        {
            AmazonCertificateManagerConfig awsconfig = new()
            {
                UseAlternateUserAgentHeader = AwsConfig.UseAlternateUserAgentHeader,
                RegionEndpoint = AwsConfig.RegionEndpoint
            };

            var credentials = AwsAuthenticationHelper.LoadDefaultProfile();

            return services.AddScoped(sp => new AmazonCertificateManagerClient(credentials, awsconfig));
        }
    }
}
