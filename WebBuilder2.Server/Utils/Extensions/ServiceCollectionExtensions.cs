using Amazon.Route53;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.Util;
using WebBuilder2.Server.Settings;

namespace WebBuilder2.Server.Utils.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAwsS3Client(this IServiceCollection services, ConfigurationManager configuration)
        {
            var credentials = GetCredentials(configuration);

            AmazonS3Config awsConfig = new()
            {
                UseAlternateUserAgentHeader = AwsConfig.UseAlternateUserAgentHeader,
                RegionEndpoint = AwsConfig.RegionEndpoint
            };

            return services.AddScoped(sp => new AmazonS3Client(credentials, awsConfig));

        }

        public static IServiceCollection AddAwsRoute53Client(this IServiceCollection services, ConfigurationManager configuration)
        {
            var credentials = GetCredentials(configuration);

            AmazonRoute53Config awsConfig = new()
            {
                UseAlternateUserAgentHeader = AwsConfig.UseAlternateUserAgentHeader,
                RegionEndpoint = AwsConfig.RegionEndpoint
            };
            return services.AddScoped(sp => new AmazonRoute53Client(credentials, awsConfig));
        }

        private static BasicAWSCredentials GetCredentials(ConfigurationManager configuration)
        {
            AwsSettings awsSettings = configuration.GetSection("AwsSettings").Get<AwsSettings>()!;
            return new(awsSettings.AccessKey, awsSettings.SecretKey);
        }
    }
}
